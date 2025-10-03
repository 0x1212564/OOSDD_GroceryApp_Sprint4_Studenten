using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class BoughtProductsView : ContentPage
{
    public BoughtProductsView(BoughtProductsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}