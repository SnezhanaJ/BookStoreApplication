using AdminBookStoreApp.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace AdminBookStoreApp.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44305/api/Admin/GetAllBooks";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Books>>().Result;

            return View(data);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            HttpClient client = new HttpClient();
            //added in next aud
            string URL = "https://localhost:44305/api/Admin/GetBooksDetails";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Books>().Result;

            HttpClient clientA = new HttpClient();
            string URLA = "https://localhost:44305/api/Admin/GetAllAuthors";

            HttpResponseMessage responseAuthors = client.GetAsync(URLA).Result;
            var data = responseAuthors.Content.ReadAsAsync<List<Models.Author>>().Result;
            // Fetch the authors from the database
            var authors = data.Select(a => new {
                Id = a.Id,
                FullName = a.FirstName + " " + a.LastName  // Combine first and last names
            }).ToList();

            // Create a SelectList with FullName as the text and Id as the value
            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName");


            HttpClient clientP = new HttpClient();
            string URLP = "https://localhost:44305/api/Admin/GetAllPublishers";

            HttpResponseMessage responsePublishers = clientP.GetAsync(URLP).Result;
            var publishers = responsePublishers.Content.ReadAsAsync<List<Publisher>>().Result;

            ViewData["PublisherId"] = new SelectList(publishers, "Id", "Name");


            return View(result);

        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            HttpClient client = new HttpClient();
            //added in next aud
            string URL = "https://localhost:44305/api/Admin/GetBooksDetails";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Books>().Result;


            return View(result);

        }


        [HttpPost]
        public IActionResult Update(Books book)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44305/api/Admin/EditBooks";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Handle the error response, for example, by displaying an error message
                ModelState.AddModelError(string.Empty, "Failed to update author");
                return View(book);
            }
        }

        public IActionResult Delete(string id)
        {

            HttpClient client = new HttpClient();
            //added in next aud
            string URL = "https://localhost:44305/api/Admin/DeleteBooks";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Books>().Result;
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Create()
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44305/api/Admin/GetAllAuthors";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Models.Author>>().Result;
            // Fetch the authors from the database
            var authors = data.Select(a => new {
                Id = a.Id,
                FullName = a.FirstName + " " + a.LastName  // Combine first and last names
            }).ToList();

            // Create a SelectList with FullName as the text and Id as the value
            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName");


            HttpClient clientP = new HttpClient();
            string URLP = "https://localhost:44305/api/Admin/GetAllPublishers";

            HttpResponseMessage responsePublishers = clientP.GetAsync(URLP).Result;
            var publishers = responsePublishers.Content.ReadAsAsync<List<Publisher>>().Result;

            ViewData["PublisherId"] = new SelectList(publishers, "Id", "Name");
            return View();

        }
        [HttpPost]
        public IActionResult Create(Books author)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44305/api/Admin/CreateBooks";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(author), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Handle the error response, for example, by displaying an error message
                ModelState.AddModelError(string.Empty, "Failed to create author");
                return View(author);
            }
        }

        public IActionResult ImportBooksIndex()
        {
            return View();
        }

        public IActionResult ImportBooks(IFormFile file)
        {
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            List<Books> books = getAllBooksFromFile(file.FileName);
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44305/api/Admin/ImportAllUsers";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(books), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<bool>().Result;

            return RedirectToAction("Index", "Books");
        }

        private List<Books> getAllBooksFromFile(string fileName)
        {
            List<Books> books = new List<Books>();
            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        books.Add(new Models.Books
                        {
                            Title = reader.GetValue(0).ToString(),
                            BookImage = reader.GetValue(1).ToString(),
                            Price = Convert.ToDouble(reader.GetValue(2).ToString()),
                            ReleaseDate = Convert.ToDateTime(reader.GetValue(3).ToString()),

                            Publisher = new Publisher
                            {
                                Name = reader.GetValue(4)?.ToString() // Assuming publisher name is in column 4
                            },

                            Author = new Models.Author
                            {
                                FirstName = reader.GetValue(5)?.ToString(), // Assuming first name is in column 5
                                LastName = reader.GetValue(6)?.ToString()   // Assuming last name is in column 6
                            }
                        });
                    }

                }
            }
            return books;
        }
    }
}
