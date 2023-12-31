﻿namespace Shop.Services.Dtos.ProductDtos
{
    public class ProductGetAllDto
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }

        public bool HasDiscount { get; set; }
        public decimal DiscountPercent { get; set; }

    }
}
