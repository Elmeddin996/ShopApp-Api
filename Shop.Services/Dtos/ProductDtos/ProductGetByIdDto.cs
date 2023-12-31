﻿namespace Shop.Services.Dtos.ProductDtos
{
    public class ProductGetByIdDto
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }

        public decimal DiscountPercent { get; set; }
    }

    public class BrandInProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
