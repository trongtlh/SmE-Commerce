﻿using System;
using System.Collections.Generic;

namespace SmE_CommerceModels.Objects;

public partial class Discount
{
    public uint DiscountId { get; set; }

    public string DiscountName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsPercentage { get; set; }

    public decimal DiscountValue { get; set; }

    public decimal? MinimumOrderAmount { get; set; }

    public decimal? MaximumDiscount { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    /// <summary>
    /// Values: active, inactive, deleted
    /// </summary>
    public string Status { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public uint? CreatedById { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public uint? ModifiedById { get; set; }

    public virtual ICollection<DiscountCode> DiscountCodes { get; set; } = new List<DiscountCode>();

    public virtual ICollection<DiscountProduct> DiscountProducts { get; set; } = new List<DiscountProduct>();
}
