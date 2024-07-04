using BookStore.Domain.Domain;
using BookStore.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {

        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entities = context.Set<Order>();
        }
        public List<Order> GetAllOrders()
        {
            return entities
                .Include(z => z.bookInOrders)
                .Include(z => z.User)
                .Include("bookInOrders.Book")
                .ToList();
        }

        public Order GetOrderDetails(BaseEntity model)
        {
            return entities

                .Include(z => z.bookInOrders)
                .Include(z => z.User)
                .Include("bookInOrders.Book")
                .SingleOrDefault(z => z.Id == model.Id);
        }
    }
}
