using Prism.Mvvm;
using System;
using System.Collections.Generic;
namespace JacksonVeroneze.Shopping.ViewModels
{
    public class ProductModelData : BindableBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }

        public double Price { get; set; }

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