
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        
        public List<BoughtProducts> Get(int? productId)
        {
            var boughtProductsList = new List<BoughtProducts>();
            
            if (productId == null)
            {
                return boughtProductsList;
            }
            
            // Get all grocery list items that contain the specified product
            var groceryListItemsWithProduct = _groceryListItemsRepository.GetAll()
                .Where(item => item.ProductId == productId.Value)
                .ToList();
            
            // Get unique grocery list IDs
            var groceryListIds = groceryListItemsWithProduct
                .Select(item => item.GroceryListId)
                .Distinct()
                .ToList();
            
            // For each grocery list, find the client and create BoughtProducts entry
            foreach (var groceryListId in groceryListIds)
            {
                var groceryList = _groceryListRepository.Get(groceryListId);
                if (groceryList != null)
                {
                    var client = _clientRepository.Get(groceryList.ClientId);
                    var product = _productRepository.Get(productId.Value);
                    
                    if (client != null && product != null)
                    {
                        boughtProductsList.Add(new BoughtProducts(client, groceryList, product));
                    }
                }
            }
            
            return boughtProductsList;
        }
    }
}