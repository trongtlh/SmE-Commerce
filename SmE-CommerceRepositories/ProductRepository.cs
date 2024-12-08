﻿using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.EntityFrameworkCore;
using SmE_CommerceModels.DBContext;
using SmE_CommerceModels.Enums;
using SmE_CommerceModels.Models;
using SmE_CommerceModels.ReturnResult;
using SmE_CommerceRepositories.Interface;

namespace SmE_CommerceRepositories;

public class ProductRepository(SmECommerceContext dbContext) : IProductRepository
{
    #region Product

    public async Task<Return<Product>> GetProductByIdAsync(Guid productId)
    {
        try
        {
            var product = await dbContext
                .Products
                // .Where(x => x.Status != ProductStatus.Deleted)
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductAttributes)
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            if (product is null)
                return new Return<Product>
                {
                    Data = null,
                    IsSuccess = false,
                    StatusCode = ErrorCode.ProductNotFound,
                    TotalRecord = 0,
                };

            return new Return<Product>
            {
                Data = product,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = 1,
            };
        }
        catch (Exception ex)
        {
            return new Return<Product>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,
                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    public async Task<Return<Product>> GetProductByIdForUpdateAsync(Guid productId)
    {
        try
        {
            var product = await dbContext
                .Products.Include(x => x.ProductCategories)
                .ThenInclude(x => x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductAttributes)
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            if (product is null)
                return new Return<Product>
                {
                    Data = null,
                    IsSuccess = false,
                    StatusCode = ErrorCode.ProductNotFound,
                    TotalRecord = 0,
                };

            await dbContext.Database.ExecuteSqlRawAsync(
                "SELECT * FROM public.\"Products\" WHERE public.\"Products\".\"productId\" = {0} FOR UPDATE",
                productId
            );

            return new Return<Product>
            {
                Data = product,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = 1,
            };
        }
        catch (Exception ex)
        {
            return new Return<Product>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,
                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    public async Task<Return<Product>> AddProductAsync(Product product)
    {
        try
        {
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            return new Return<Product>
            {
                Data = product,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = 1,
            };
        }
        catch (Exception ex)
        {
            return new Return<Product>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,
                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    public async Task<Return<string>> GetProductSlugAsync(string slug)
    {
        try
        {
            var product = await dbContext
                .Products.Where(x => x.Status != ProductStatus.Deleted)
                .FirstOrDefaultAsync(x => x.Slug == slug);

            if (product is null)
                return new Return<string>
                {
                    Data = null,
                    IsSuccess = false,
                    StatusCode = ErrorCode.ProductNotFound,
                    TotalRecord = 0,
                };

            return new Return<string>
            {
                Data = product.Slug,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = 1,
            };
        }
        catch (Exception ex)
        {
            return new Return<string>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,
                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    public async Task<Return<Product>> UpdateProductAsync(Product product)
    {
        try
        {
            dbContext.Products.Update(product);
            await dbContext.SaveChangesAsync();

            return new Return<Product>
            {
                Data = product,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = 1,
            };
        }
        catch (Exception ex)
        {
            return new Return<Product>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,
                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    #endregion

    #region Product Attribute

    public async Task<Return<ProductAttribute>> GetProductAttributeByIdAsync(Guid attributeId)
    {
        try
        {
            var productAttribute = await dbContext.ProductAttributes.FirstOrDefaultAsync(x =>
                x.AttributeId == attributeId
            );

            if (productAttribute is null)
                return new Return<ProductAttribute>
                {
                    Data = null,
                    IsSuccess = false,
                    StatusCode = ErrorCode.ProductAttributeNotFound,
                    TotalRecord = 0,
                };

            return new Return<ProductAttribute>
            {
                Data = productAttribute,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = 1,
            };
        }
        catch (Exception ex)
        {
            return new Return<ProductAttribute>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,
                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    public async Task<Return<ProductAttribute>> AddProductAttributeAsync(
        ProductAttribute productAttribute
    )
    {
        try
        {
            await dbContext.ProductAttributes.AddAsync(productAttribute);
            await dbContext.SaveChangesAsync();

            return new Return<ProductAttribute>
            {
                Data = productAttribute,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = 1,
            };
        }
        catch (Exception ex)
        {
            return new Return<ProductAttribute>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,
                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    public async Task<Return<ProductAttribute>> UpdateProductAttributeAsync(
        ProductAttribute productAttributes
    )
    {
        try
        {
            dbContext.ProductAttributes.Update(productAttributes);
            await dbContext.SaveChangesAsync();

            return new Return<ProductAttribute>
            {
                Data = productAttributes,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = 1,
            };
        }
        catch (Exception ex)
        {
            return new Return<ProductAttribute>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,
                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    public async Task<Return<ProductAttribute>> DeleteProductAttributeAsync(Guid attributeId)
    {
        try
        {
            var productAttribute = await dbContext.ProductAttributes.FirstOrDefaultAsync(x =>
                x.AttributeId == attributeId
            );
            if (productAttribute is null)
                return new Return<ProductAttribute>
                {
                    Data = null,
                    IsSuccess = false,
                    StatusCode = ErrorCode.ProductAttributeNotFound,
                    TotalRecord = 0,
                };

            dbContext.ProductAttributes.Remove(productAttribute);
            await dbContext.SaveChangesAsync();

            return new Return<ProductAttribute>
            {
                Data = null,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = 1,
            };
        }
        catch (Exception ex)
        {
            return new Return<ProductAttribute>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,
                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    #endregion

    #region Product Category

    public async Task<Return<List<ProductCategory>>> GetProductCategoriesAsync(Guid productId)
    {
        try
        {
            var productCategories = await dbContext
                .ProductCategories.Where(x => x.ProductId == productId)
                .ToListAsync();

            return new Return<List<ProductCategory>>
            {
                Data = productCategories,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = productCategories.Count,
            };
        }
        catch (Exception ex)
        {
            return new Return<List<ProductCategory>>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,
                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    public async Task<Return<List<ProductCategory>>> AddProductCategoriesAsync(
        List<ProductCategory> productCategories
    )
    {
        try
        {
            await dbContext.ProductCategories.AddRangeAsync(productCategories);
            await dbContext.SaveChangesAsync();

            return new Return<List<ProductCategory>>
            {
                Data = productCategories,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = productCategories.Count,
            };
        }
        catch (Exception ex)
        {
            return new Return<List<ProductCategory>>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,
                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    public async Task<Return<ProductCategory>> DeleteProductCategoryAsync(
        Guid productId,
        List<Guid> categoryIds
    )
    {
        try
        {
            var productCategories = await dbContext
                .ProductCategories.Where(x =>
                    x.ProductId == productId && categoryIds.Contains(x.CategoryId)
                )
                .ToListAsync();

            dbContext.ProductCategories.RemoveRange(productCategories);
            await dbContext.SaveChangesAsync();

            return new Return<ProductCategory>
            {
                Data = null,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = productCategories.Count,
            };
        }
        catch (Exception ex)
        {
            return new Return<ProductCategory>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,

                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    #endregion

    #region Product Image

    public async Task<Return<List<ProductImage>>> GetProductImagesAsync(Guid productId)
    {
        try
        {
            var productImages = await dbContext
                .ProductImages.Where(x => x.ProductId == productId)
                .ToListAsync();

            return new Return<List<ProductImage>>
            {
                Data = productImages,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = productImages.Count,
            };
        }
        catch (Exception ex)
        {
            return new Return<List<ProductImage>>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,
                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    public async Task<Return<ProductImage>> GetProductImageByIdAsync(Guid productImageId)
    {
        try
        {
            var productImage = await dbContext.ProductImages.FirstOrDefaultAsync(x =>
                x.ImageId == productImageId
            );

            if (productImage is null)
                return new Return<ProductImage>
                {
                    Data = null,
                    IsSuccess = false,
                    StatusCode = ErrorCode.ProductImageNotFound,
                    TotalRecord = 0,
                };

            return new Return<ProductImage>
            {
                Data = productImage,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = 1,
            };
        }
        catch (Exception ex)
        {
            return new Return<ProductImage>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,

                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    public async Task<Return<ProductImage>> AddProductImageAsync(ProductImage productImage)
    {
        try
        {
            await dbContext.ProductImages.AddAsync(productImage);
            await dbContext.SaveChangesAsync();

            return new Return<ProductImage>
            {
                Data = productImage,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,

                TotalRecord = 1,
            };
        }
        catch (Exception ex)
        {
            return new Return<ProductImage>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,

                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    public async Task<Return<ProductImage>> UpdateProductImageAsync(ProductImage productImage)
    {
        try
        {
            dbContext.ProductImages.Update(productImage);
            await dbContext.SaveChangesAsync();

            return new Return<ProductImage>
            {
                Data = productImage,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = 1,
            };
        }
        catch (Exception ex)
        {
            return new Return<ProductImage>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,

                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    public async Task<Return<ProductImage>> DeleteProductImageAsync(Guid productImageId)
    {
        try
        {
            var productImage = await dbContext.ProductImages.FirstOrDefaultAsync(x =>
                x.ImageId == productImageId
            );
            if (productImage is null)
                return new Return<ProductImage>
                {
                    Data = null,
                    IsSuccess = false,
                    StatusCode = ErrorCode.ProductImageNotFound,
                    TotalRecord = 0,
                };

            dbContext.ProductImages.Remove(productImage);
            await dbContext.SaveChangesAsync();

            return new Return<ProductImage>
            {
                Data = null,
                IsSuccess = true,
                StatusCode = ErrorCode.Ok,
                TotalRecord = 1,
            };
        }
        catch (Exception ex)
        {
            return new Return<ProductImage>
            {
                Data = null,
                IsSuccess = false,
                StatusCode = ErrorCode.InternalServerError,

                InternalErrorMessage = ex,
                TotalRecord = 0,
            };
        }
    }

    #endregion
}
