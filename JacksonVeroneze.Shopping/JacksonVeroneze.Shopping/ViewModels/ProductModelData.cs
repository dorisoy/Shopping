﻿using Prism.Mvvm;
namespace JacksonVeroneze.Shopping.ViewModels
{
    public class ProductModelData : BindableBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }

        public int? CategoryId { get; set; }

        private double _originalPrice = 0;
        public double OriginalPrice
        {
            get => _originalPrice;
            set => SetProperty(ref _originalPrice, value);
        }

        private double _priceWithDiscount = 0;
        public double PriceWithDiscount
        {
            get => _priceWithDiscount;
            set => SetProperty(ref _priceWithDiscount, value);
        }

        private double _percentageDiscount = 0;
        public double PercentageDiscount
        {
            get => _percentageDiscount;
            set => SetProperty(ref _percentageDiscount, value);
        }

        private int _quantity = 0;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        private bool _isFavorite = false;
        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetProperty(ref _isFavorite, value);
        }

    }
}