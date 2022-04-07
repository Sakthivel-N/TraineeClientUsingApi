using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TraineeClientUsingApi.Models;

namespace TraineeClientUsingApi.Controllers
{
    public class AssmtRecordsController : Controller
    {
        public static string baseURL;
        private readonly IConfiguration _configuration;
        public AssmtRecordsController(IConfiguration configuration)
        {
            _configuration = configuration;
            baseURL = _configuration.GetValue<string>("BaseURL");

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
        public async Task<List<AssessmentRecord>> GetRecords()
        {

            List<AssessmentRecord> received = new List<AssessmentRecord>();


            using (var httpClient = new HttpClient())
            {


                using (var response = await httpClient.GetAsync(baseURL + "/api/AssessmentRecords"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<List<AssessmentRecord>>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }

            }
            return received;

        }
        public async Task<AssessmentRecord> GetRecords(int id)
        {

            AssessmentRecord received = new AssessmentRecord();


            using (var httpClient = new HttpClient())
            {


                using (var response = await httpClient.GetAsync(baseURL + "/api/AssessmentRecords/" + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<AssessmentRecord>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }

            }
            return received;

        }
        public async Task<IActionResult> GetAllMarks()
        {
            List<AssessmentRecord> received = await GetRecords();
            return View(received);
        }


        public async Task<IActionResult> TraineesList()
        {
            List<TraineeDetail> received = await GetTrainees();
            return View(received);
        }
        

        public IActionResult GiveScores(int id)
        {
            ViewBag.TraineeId = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GiveScores(AssessmentRecord assessment)
        {
            AssessmentRecord received = new AssessmentRecord();

            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(assessment), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(baseURL + "/api/AssessmentRecords", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    received = JsonConvert.DeserializeObject<AssessmentRecord>(apiResponse);
                    if (received != null)
                    {
                        ViewBag.Message = "Scores Added Successfully";
                        return RedirectToAction("GetAllMarks", "AssmtRecords");
                    }
                }

            }
            ViewBag.Message = "Failed to Add Scores";
            return View(assessment.TraineeId);


        }

        public async Task<IActionResult> Details(int id)
        {
            AssessmentRecord Assessmentdetails = await GetRecords(id);
            return View(Assessmentdetails);
        }
        public async Task<ActionResult> Edit(int id)
        {
            AssessmentRecord assessment = await GetRecords(id);
            return View(assessment);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AssessmentRecord updatedAssessments)
        {
            updatedAssessments.RecordId = id;

            using (var httpClient = new HttpClient())
            {
                StringContent contents = new StringContent(JsonConvert.SerializeObject(updatedAssessments), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(baseURL + "/api/AssessmentRecords/" + id, contents))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();


                    if (apiResponse != null)
                        ViewBag.Message = "Assessment Updated Successfully";
                    else
                        ViewBag.Message = "Assessment updation Failed";
                }

            }

            return RedirectToAction("GetAllMarks");

        }



        // GET: ProductsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            AssessmentRecord assessment = await GetRecords(id);
            return View(assessment);
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.DeleteAsync(baseURL + "/api/AssessmentRecords/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("GetAllMarks");
        }



    }
}
