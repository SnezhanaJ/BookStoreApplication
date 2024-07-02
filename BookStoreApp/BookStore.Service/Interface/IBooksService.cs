using BookStore.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service.Interface
{
    public interface IBooksService
    {
        List<Books> GetAll();
        Books GetDetails(Guid? id);
        void CreateNewBook(Books p);
        void UpdateExistingBook(Books p);
        void DeleteBook(Guid id);

        Books GetDetailsWithBaseEntity(BaseEntity id);
        bool UpdateBook(Books p);
        bool DeleteBooksBoolean(BaseEntity id);
    }
}
