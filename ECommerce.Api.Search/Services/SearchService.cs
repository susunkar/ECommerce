using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;

        public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomersService customersService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var ordersResult = await ordersService.GetOrderAsync(customerId);
            var poductsResult = await productsService.GetProductsAsync();
            var customersResult = await customersService.GetCustomerAsync(customerId);
            
            if (ordersResult.IsSuccess)
            {
                foreach (var order in ordersResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = poductsResult.IsSuccess 
                            ?poductsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name
                            :"Product information is not available";
                    }
                }
                var result = new {
                    Customer = customersResult.IsSuccess ?
                                customersResult.Customer : new Customer { Name = "Customer information is not available" },
                    Order = ordersResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }

    }
}
