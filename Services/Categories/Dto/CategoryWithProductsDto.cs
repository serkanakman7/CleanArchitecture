using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Services.Products;

namespace App.Services.Categories.Dto;

public record CategoryWithProductsDto(int Id, string Name, List<ProductDto> Products );

