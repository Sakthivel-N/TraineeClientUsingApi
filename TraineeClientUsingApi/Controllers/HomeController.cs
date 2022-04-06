using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TraineeClientUsingApi.Models;

namespace TraineeClientUsingApi.Controllers
{

    public class HomeController : Controller
    {
        public static string baseURL;
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            baseURL = _configuration.GetValue<string>("BaseURL");

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TrDashBoard()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                ViewBag.Message = HttpContext.Session.GetString("Email");
                return View();
            }
            return RedirectToAction("Index");
            
        }
        

        public async Task<TraineeDetail> GetValidTrainee(string Email,string Password)
        {
            
            List<TraineeDetail> trainee = new List<TraineeDetail>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(baseURL + "/api/TraineeDetails"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    trainee = JsonConvert.DeserializeObject<List<TraineeDetail>>(apiResponse);
                }
                //trainee.ForEach(t => t.Email == Email && t.Password == Password).FirstOeDefault();
            }
            return (trainee.FirstOrDefault(m => m.Email == Email && m.Password == Password));

        }
        public async Task<AdminDetail> GetValidAdmin(string AdminName, string Password)
        {

            List<AdminDetail> admin = new List<AdminDetail>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(baseURL + "/api/AdminDetails"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    admin = JsonConvert.DeserializeObject<List<AdminDetail>>(apiResponse);
                }
                //trainee.ForEach(t => t.Email == Email && t.Password == Password).FirstOeDefault();
            }
            return (admin.FirstOrDefault(m => m.AdminName == AdminName && m.Password == Password));

        }

        [HttpGet]
        public IActionResult TraineeLogin()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TraineeLogin(TraineeDetail trainees)
        {
            if (trainees.Email != null && trainees.Password != null)
            {
                //HttpClientHandler clientHandler = new HttpClientHandler();
                //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                TraineeDetail trainee = await GetValidTrainee(trainees.Email,trainees.Password);
                if (trainee != null)
                {

                    ViewBag.Message = "Login Successfully";
                    HttpContext.Session.SetString("Email", trainees.Email);
                    return RedirectToAction("TrDashBoard");

                }
                else
                {

                    ViewBag.Message = "Incorrect Email and Password";
                    return View();

                }


            }
            ViewBag.Message = "Incorrect Email and Password";
            return View();

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(TraineeDetail trainees)
        {
            trainees.Department = "Not Assigned";
            trainees.BatchCode = "NA";

            TraineeDetail received = new TraineeDetail();

            //HttpClientHandler clientHandler = new HttpClientHandler();
            //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(trainees), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(baseURL + "/api/TraineeDetails", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    received = JsonConvert.DeserializeObject<TraineeDetail>(apiResponse);
                    if (received != null)
                    {
                        ViewBag.Message = "Trainee Registration Successfully";
                        return View();
                    }
                }

            }
            ViewBag.Message = "Registration Failed";
            return View();


        }

        [HttpGet]
        public IActionResult AdminLogin()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AdminLogin(AdminDetail admin)
        {
            if (admin.AdminName != null && admin.Password != null)
            {
                //HttpClientHandler clientHandler = new HttpClientHandler();
                //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                AdminDetail admins = await GetValidAdmin(admin.AdminName, admin.Password);
                if (admins != null)
                {

                    ViewBag.Message = "Login Successfully";
                    HttpContext.Session.SetString("Admin", admins.AdminName);
                    return RedirectToAction("Index","Admin");

                }
                else
                {

                    ViewBag.Message = "Incorrect Admin Credentials";
                    return View();

                }


            }
            ViewBag.Message = "Incorrect Admin Credentials";
            return View();

        }

        
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("Email")!=null)
            {
                if(HttpContext.Session.GetString("Admin") != null)
                {
                    HttpContext.Session.Remove("Admin");
                }
                HttpContext.Session.Remove("Email");
            }
            else if(HttpContext.Session.GetString("Admin") != null)
            {
                if(HttpContext.Session.GetString("Email") != null)
                {
                    HttpContext.Session.Remove("Email");
                }
                HttpContext.Session.Remove("Admin");
            }
            return RedirectToAction("Index");
        }
        

    }
}
