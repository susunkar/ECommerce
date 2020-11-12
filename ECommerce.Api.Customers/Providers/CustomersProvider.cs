﻿using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext dbContext,
            ILogger<CustomersProvider> logger,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }
        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Suresh", Address = "Hyd"});
                dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "Kumar", Address = "VSKP" });
                dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "SDSurya", Address = "Hyd" });
                dbContext.Customers.Add(new Db.Customer() { Id = 4, Name = "Dhakshith", Address = "Hyd" });
                dbContext.Customers.Add(new Db.Customer() { Id = 5, Name = "Sunkar", Address = "Hyderabad" });
                dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await dbContext.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomersAsync(int id)
        {
            try
            {
                var customers = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (customers != null)
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(customers);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
