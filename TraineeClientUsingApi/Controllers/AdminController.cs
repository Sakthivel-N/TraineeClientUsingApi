using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TraineeClientUsingApi.Models;

namespace TraineeClientUsingApi.Controllers
{
    public class AdminController : Controller
    {
        public static string baseURL;
        private readonly IConfiguration _configuration;
        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
            baseURL = _configuration.GetValue<string>("BaseURL");

        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                ViewBag.Message = HttpContext.Session.GetString("Admin");
                return View();

            }
            return RedirectToAction("Index", "Home");

        }
        public async Task<List<TraineeDetail>> GetTrainees()
        {
            List<TraineeDetail> trainee = new List<TraineeDetail>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(baseURL + "/api/TraineeDetails"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    trainee = JsonConvert.DeserializeObject<List<TraineeDetail>>(apiResponse);
                }

            }
            return (trainee);

        }
        public async Task<IActionResult> TraineesList()
        {
            List<TraineeDetail> trainee = await GetTrainees();
            return View(trainee);
        }

        public async Task<IActionResult> AssignRole(int id)
        {
            List<TraineeDetail> trainee = await GetTrainees();
            
            return View(trainee.FirstOrDefault(m => m.TraineeId == id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignRole(int id, TraineeDetail trainee)
        {
            List<TraineeDetail> prevData = await GetTrainees();
            TraineeDetail newTrainee = prevData.FirstOrDefault(m => m.TraineeId == id);

            
            trainee.TraineeId = id;
            trainee.TraineeName = newTrainee.TraineeName;
            trainee.Email = newTrainee.Email;
            trainee.PhoneNumber=newTrainee.PhoneNumber;
            trainee.Date = newTrainee.Date;
            trainee.Interests=newTrainee.Interests;
            trainee.Password=newTrainee.Password;

            
            
            using (var httpClient = new HttpClient())
            {
                


                StringContent contents = new StringContent(JsonConvert.SerializeObject(trainee), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(baseURL + "/api/TraineeDetails/" + id, contents))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    
                    if (apiResponse != null)
                        ViewBag.Message = "Role Assigned Successfully";
                    else
                        ViewBag.Message = "Role Assigning Failed";
                }

            }

            return View();

        }

        public async Task<IActionResult> Details(int id)
        {
            List<TraineeDetail> prevData = await GetTrainees();
            TraineeDetail newTrainee = prevData.FirstOrDefault(m => m.TraineeId == id);
            return View(newTrainee);
        }
        public async Task<ActionResult> Delete(int id)
        {
            List<TraineeDetail> trainees = await GetTrainees();
            return View(trainees.FirstOrDefault(t=>t.TraineeId==id));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id,TraineeDetail trainee)
        {
            
            using (var httpClient = new HttpClient())
            {
               
                using (var response = await httpClient.DeleteAsync(baseURL + "/api/TraineeDetails/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("TraineesList");
        }
        public async Task<IActionResult> JavaTrainees()
        {
            List<TraineeDetail> trainee = await GetTrainees();
            return View(trainee);
        }
        public async Task<IActionResult> NetTrainees()
        {
            List<TraineeDetail> trainee = await GetTrainees();
            return View(trainee);

        }

        

    }
}
