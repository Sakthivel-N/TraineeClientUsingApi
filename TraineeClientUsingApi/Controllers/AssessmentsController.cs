using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TraineeClientUsingApi.Models;

namespace TraineeClientUsingApi.Controllers
{
    public class AssessmentsController : Controller
    {
        public static string baseURL;
        private readonly IConfiguration _configuration;
        public AssessmentsController(IConfiguration configuration)
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

        public async Task<IActionResult> AssessmentList()
        {
            List<Assessment> assessment = await GetAssessments();
            return View(assessment);
        }

        public IActionResult AddAssessment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAssessment(Assessment assessment)
        {
            assessment.AssessmentStatus = "OnProgress";

            Assessment received = new Assessment();



            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(assessment), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(baseURL + "/api/Assessments", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    received = JsonConvert.DeserializeObject<Assessment>(apiResponse);
                    if (received != null)
                    {
                        ViewBag.Message = "Assessment Added Successfully";
                        return RedirectToAction("AssessmentList", "Assessments");
                    }
                }

            }
            ViewBag.Message = "Failed to Add Assessment";
            return RedirectToAction("AssessmentList");

        }
        public async Task<List<Assessment>> GetAssessments()
        {

            List<Assessment> received = new List<Assessment>();


            using (var httpClient = new HttpClient())
            {


                using (var response = await httpClient.GetAsync(baseURL + "/api/Assessments"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<List<Assessment>>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }

            }
            return received;

        }

        public async Task<Assessment> GetAssessments(int id)
        {

            Assessment received = new Assessment();


            using (var httpClient = new HttpClient())
            {


                using (var response = await httpClient.GetAsync(baseURL + "/api/Assessments/" + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<Assessment>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }

            }
            return received;

        }
        public async Task<IActionResult> Details(int id)
        {
            Assessment Assessmentdetails = await GetAssessments(id);
            return View(Assessmentdetails);
        }
        public async Task<ActionResult> Edit(int id)
        {
            Assessment assessment = await GetAssessments(id);
            return View(assessment);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Assessment updatedAssessments)
        {
            updatedAssessments.AssessmentId = id;

            using (var httpClient = new HttpClient())
            {
                StringContent contents = new StringContent(JsonConvert.SerializeObject(updatedAssessments), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(baseURL + "/api/Assessments/" + id, contents))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();


                    if (apiResponse != null)
                        ViewBag.Message = "Assessment Updated Successfully";
                    else
                        ViewBag.Message = "Assessment updation Failed";
                }

            }

            return RedirectToAction("AssessmentList");

        }



        // GET: ProductsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Assessment assessment = await GetAssessments(id);
            return View(assessment);
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.DeleteAsync(baseURL + "/api/Assessments/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("AssessmentList");
        }



    }
}
