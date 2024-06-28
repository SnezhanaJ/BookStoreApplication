using BookStore.Domain.Domain;
using BookStore.Repository.Implementation;
using BookStore.Repository.Interface;
using BookStore.Service.Interface;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service.Implementation
{
    public class PublisherService : IPublisherService
    {
        private readonly IRepository<Publisher> _publisherRepository;
        public PublisherService(IRepository<Publisher> publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }
        public void CreateNewPublisher(Publisher p)
        {
             _publisherRepository.Insert(p);
        }

        public void DeletePublisher(Guid id)
        {
            var publisher = _publisherRepository.Get(id);
             _publisherRepository.Delete(publisher);
        }

        public bool DeletePublisherBoolean(BaseEntity id)
        {
            try
            {
                var publisher = GetDetailsWithBaseEntity(id);
                _publisherRepository.Delete(publisher);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Publisher> GetAll()
        {
            return _publisherRepository.GetAll().ToList();
        }

        public Publisher GetDetails(Guid? id)
        {
            return _publisherRepository.Get(id);
        }

        public Publisher GetDetailsWithBaseEntity(BaseEntity id)
        {
            return _publisherRepository.GetDetailsWithBaseEntity(id);

        }

        public void UpdateExistingPublisher(Publisher p)
        {
            _publisherRepository.Update(p);
        }

        public bool UpdatePublisher(Publisher p)
        {
            try
            {
                _publisherRepository.Update(p);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
