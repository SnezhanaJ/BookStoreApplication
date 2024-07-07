using BookStore.Domain.Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BookStore.Web.Controllers
{
    public class PartnerPlatformController : Controller
    {
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://onlineeducationapp.azurewebsites.net/api/Admin/GetAllEnrollments";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Enrollment>>().Result;

            var transformedData = data
                .Select(e =>
                {
                    if(e.Course.Level == 1 || e.Course.Level == 2)
                    {
                        e.CourseDifficulty = "Beginner";
                    }else if(e.Course.Level == 3 || e.Course.Level == 4)
                    {
                        e.CourseDifficulty = "Intermediate";
                    }
                    else
                    {
                        e.CourseDifficulty = "Advanced";
                    }
                    return e;
                })
                .OrderBy(e=>e.Course.Level).ToList();
            return View(transformedData);
        }

        public IActionResult Details(string id)
        {
            HttpClient client = new HttpClient();

            string URL = "https://onlineeducationapp.azurewebsites.net/api/Admin/GetDetailsForEnrollment";
            var model = new
            {
               CourseId = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Enrollment>().Result;


            return View(result);
        }

        
    }
}
