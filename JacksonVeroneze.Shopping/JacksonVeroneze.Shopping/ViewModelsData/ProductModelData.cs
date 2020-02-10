using Prism.Mvvm;

namespace JacksonVeroneze.Shopping.ViewModelsData
{
    public class ProductModelData : BindableBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }

        public int? CategoryId { get; set; }

        private string _promotion;
        public string Promotion
        {
            get => _promotion;
            set => SetProperty(ref _promotion, value);
        }

        private double _originalPrice = 0;
        public double OriginalPrice
        {
            get => _originalPrice;
            set => SetProperty(ref _originalPrice, value);
        }

        private double _finalPrice = 0;
        public double FinalPrice
        {
            get => _finalPrice;
            set => SetProperty(ref _finalPrice, value);
        }

        private double _total = 0;
        public double Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
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