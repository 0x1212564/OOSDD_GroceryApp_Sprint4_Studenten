using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class BoughtProductsViewModel : BaseViewModel
    {
        private readonly IBoughtProductsService _boughtProductsService;

        [ObservableProperty]
        Product selectedProduct;
        
        public ObservableCollection<BoughtProducts> BoughtProductsList { get; set; } = [];
        public ObservableCollection<Product> Products { get; set; }

        public BoughtProductsViewModel(IBoughtProductsService boughtProductsService, IProductService productService)
        {
            _boughtProductsService = boughtProductsService;
            Products = new(productService.GetAll());
        }

        partial void OnSelectedProductChanged(Product? oldValue, Product newValue)
        {
            // Clear the current list
            BoughtProductsList.Clear();
            
            // If no product is selected, return early
            if (newValue == null)
                return;
                
            // Get all clients who bought this product
            var boughtProducts = _boughtProductsService.Get(newValue.Id);
            
            // Add to the observable collection
            foreach (var boughtProduct in boughtProducts)
            {
                BoughtProductsList.Add(boughtProduct);
            }
        }

        [RelayCommand]
        public void NewSelectedProduct(Product product)
        {
            SelectedProduct = product;
        }
        
        public override void OnAppearing()
        {
            base.OnAppearing();
            
            // Auto-select first product if available and none selected
            if (SelectedProduct == null && Products.Count > 0)
            {
                SelectedProduct = Products[0];
            }
        }
    }
}