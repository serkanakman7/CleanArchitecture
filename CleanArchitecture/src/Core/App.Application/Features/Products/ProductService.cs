using System.Net;
using App.Application.Contracts.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.ServiceBus;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Dto;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Domain.Entities;
using App.Domain.Events;
using AutoMapper;

namespace App.Application.Features.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IServiceBus _serviceBus;
        private const string ProductListCacheKey = "ProductListCacheKey";

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, IServiceBus serviceBus)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _serviceBus = serviceBus;
        }

        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductAsync(int count)
        {
            var products = await _productRepository.GetTopPriceProductsAsync(count);

            var productsAsDto = _mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productsAsDto!);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            // cache aside desing pattern
            // 1. any cache
            // 2. from db
            // 3. caching data

            var productListAsCached = await _cacheService.GetAsync<List<ProductDto>>(ProductListCacheKey);

            if(productListAsCached is not null)
            {
                return ServiceResult<List<ProductDto>>.Success(productListAsCached);
            }

            var products = await _productRepository.GetAllAsync();

            var productAsDto = _mapper.Map<List<ProductDto>>(products);

            await _cacheService.AddAsync(ProductListCacheKey, productAsDto, TimeSpan.FromMinutes(1));

            return ServiceResult<List<ProductDto>>.Success(productAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            var products = await _productRepository.GetAllPagedAsync(pageNumber,pageSize);

            var productsAsDto = _mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }
        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ServiceResult<ProductDto>.Fail("Product not found", HttpStatusCode.NotFound);
            }

            var productAsDto = _mapper.Map<ProductDto>(product);

            return ServiceResult<ProductDto>.Success(productAsDto)!;
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {
            var anyProduct = await _productRepository.AnyAsync(x => x.Name == request.Name);

            if (anyProduct)
            {
                return ServiceResult<CreateProductResponse>.Fail("Ürün ismi veritabanında bulunmaktadır.", HttpStatusCode.BadRequest);
            }

            //var product = new Product
            //{
            //    Name = request.Name,
            //    Price = request.Price,
            //    Stock = request.Stock
            //};

            var product = _mapper.Map<Product>(request);

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            await _serviceBus.PublishAsync(new ProductAddedEvent(product.Id, product.Name, product.Price));

            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id), $"api/products/{product.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            //Fast fail
            //Guard Clauses

            var isProductNameExist = await _productRepository.AnyAsync(x => x.Name == request.Name && x.Id != id);

            if (isProductNameExist)
            {
                return ServiceResult.Fail("Ürün ismi veritabanında bulunmaktadır.", HttpStatusCode.BadRequest);
            }

            //product.Name = request.Name;
            //product.Price = request.Price;
            //product.Stock = request.Stock;

            var product = _mapper.Map<Product>(request);
            product.Id = id;

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            product.Stock = request.Stock;

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            _productRepository.Delete(product!);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
