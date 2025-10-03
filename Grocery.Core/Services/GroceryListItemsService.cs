using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            // Get all grocery list items
            var groceryListItems = _groceriesRepository.GetAll();
            
            // Group by ProductId and count the total quantity sold for each product
            var productSales = groceryListItems
                .GroupBy(g => g.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    TotalSold = group.Sum(g => g.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(topX)
                .ToList();
            
            // Create BestSellingProducts objects with ranking
            var bestSellingProducts = new List<BestSellingProducts>();
            int ranking = 1;
            
            foreach (var sale in productSales)
            {
                var product = _productRepository.Get(sale.ProductId);
                if (product != null)
                {
                    bestSellingProducts.Add(new BestSellingProducts(
                        product.Id,
                        product.Name,
                        product.Price, // Using Price as stock since we don't have a separate Stock field in Product
                        sale.TotalSold,
                        ranking++
                    ));
                }
            }
            
            return bestSellingProducts;
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
            }
        }
    }
}