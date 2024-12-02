﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SmE_CommerceModels.Models;

[Table("DiscountProduct")]
public partial class DiscountProduct
{
    [Key]
    [Column("discountProductId")]
    public Guid DiscountProductId { get; set; }

    [Column("discountId")]
    public Guid? DiscountId { get; set; }

    [Column("productId")]
    public Guid? ProductId { get; set; }

    [ForeignKey("DiscountId")]
    [InverseProperty("DiscountProducts")]
    public virtual Discount? Discount { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("DiscountProducts")]
    public virtual Product? Product { get; set; }
}
