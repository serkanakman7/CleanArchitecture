using System.Net;
using App.Application;
using App.Application.Contracts.Persistence;
using App.Application.Features.Categories.Create;
using App.Application.Features.Categories.Dto;
using App.Application.Features.Categories.Update;
using App.Domain.Entities;
using AutoMapper;

namespace App.Application.Features.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<CategoryWithProductsDto>> GetCategoryWithProductsAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryWithProductsAsync(categoryId);

            if (category is null)
            {
                return ServiceResult<CategoryWithProductsDto>.Fail("kategori bulunamadı.", HttpStatusCode.NotFound);
            }

            var categoryAsDto = _mapper.Map<CategoryWithProductsDto>(category);

            return ServiceResult<CategoryWithProductsDto>.Success(categoryAsDto);
        }

        public async Task<ServiceResult<List<CategoryWithProductsDto>>> GetCategoryWithProductsAsync()
        {
            var category = await _categoryRepository.GetCategoryWithProductsAsync();


            var categoryAsDto = _mapper.Map<List<CategoryWithProductsDto>>(category);

            return ServiceResult<List<CategoryWithProductsDto>>.Success(categoryAsDto);
        }


        public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var categoriesAsDto = _mapper.Map<List<CategoryDto>>(categories);

            return ServiceResult<List<CategoryDto>>.Success(categoriesAsDto);
        }


        public async Task<ServiceResult<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return ServiceResult<CategoryDto>.Fail("kategori bulunamadı.", HttpStatusCode.NotFound);
            }

            var categoryAsDto = _mapper.Map<CategoryDto>(category);

            return ServiceResult<CategoryDto>.Success(categoryAsDto);
        }


        public async Task<ServiceResult<int>> CreateAsync(CreateCategoryRequest request)
        {
            var anyCategory = await _categoryRepository.AnyAsync(x => x.Name == request.Name);

            if (anyCategory)
            {
                return ServiceResult<int>.Fail("categori ismi veritabanında bulunmaktadır.",
                    HttpStatusCode.NotFound);
            }


            var newCategory = _mapper.Map<Category>(request);

            await _categoryRepository.AddAsync(newCategory);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<int>.SuccessAsCreated(newCategory.Id, $"api/categories/{newCategory.Id}");
        }


        public async Task<ServiceResult> UpdateAsync(int id, UpdateCategoryRequest request)
        {
            var isCategoryNameExist =
                await _categoryRepository.AnyAsync(x => x.Name == request.Name && x.Id != id);

            if (isCategoryNameExist)
            {
                return ServiceResult.Fail("kategori ismi veritabanında bulunmaktadır.",
                    HttpStatusCode.BadRequest);
            }

            var category = _mapper.Map<Category>(request);
            category.Id = id;

            _categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }


        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);


            _categoryRepository.Delete(category!);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
