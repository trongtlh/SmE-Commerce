﻿using System.ComponentModel.DataAnnotations;
using SmE_CommerceModels.Enums;

namespace SmE_CommerceModels.RequestDtos.Product;

public class AddProductReqDto
{
    [Required(ErrorMessage = "Product name is required")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Primary image is required")]
    public required string PrimaryImage { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Product price is required")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Invalid price")]
    public decimal Price { get; set; }

    [RegularExpression(@"^\d+$", ErrorMessage = "Invalid stock quantity")]
    public int StockQuantity { get; set; } = 0;

    public bool IsTopSeller { get; set; } = false;

    public List<AddProductImageReqDto> Images { get; set; } = [];

    public List<AddProductAttributeReqDto> Attributes { get; set; } = [];

    public List<Guid> CategoryIds { get; set; } = [];

    public List<AddProductVariantReqDto> ProductVariants { get; set; } = [];

    public string Status { get; set; } = ProductStatus.Active;

    public string? MetaTitle { get; set; }

    public string? MetaDescription { get; set; }
}
