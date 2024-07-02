using BookStore.Domain.Domain;
using BookStore.Repository.Interface;
using BookStore.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service.Implementation
{
    public class BooksService : IBooksService
    {
        private readonly IRepository<Books> _repository;
        public BooksService(IRepository<Books> repository)
        {
            _repository = repository;
        }

        public void CreateNewBook(Books p)
        {
            _repository.Insert(p);
        }

        public void DeleteBook(Guid id)
        {
           var book = _repository.Get(id);
           _repository.Delete(book);
        }

        public bool DeleteBooksBoolean(BaseEntity id)
        {
            try
            {
                var book = GetDetailsWithBaseEntity(id);
                _repository.Delete(book);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Books> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public Books GetDetails(Guid? id)
        {
            return _repository.Get(id);
        }

        public Books GetDetailsWithBaseEntity(BaseEntity id)
        {
            return _repository.GetDetailsWithBaseEntity(id);
        }

        public bool UpdateBook(Books p)
        {
            try
            {
                _repository.Update(p);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void UpdateExistingBook(Books p)
        {
            _repository.Update(p);
        }
    }
}
