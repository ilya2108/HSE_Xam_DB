using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;

namespace HSE.MobileSchool
{
    public static class AddToDB
    {
        public static string ADO = "Server=tcp:hsemobile.database.windows.net,1433;Initial Catalog=TextDB;Persist Security Info=False;User ID=ryabuily;Password=!HseMobile;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private static void connectToDB(string name, string surname, string text,ILogger log)
        {
            using (var connection = new SqlConnection(ADO))  
			{  
				connection.Open();  
				log.LogInformation("Connection opened");
                string query = $@"insert into text(student_name, student_surname, student_text) values ('{name}', '{surname}', '{text}')";
                using(var cmd = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    log.LogInformation("Query executed");
                    reader.Close();
                }
            }
        }

        [FunctionName("AddToDB")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            log.LogInformation("data is read");
            string name = data?.name;
            log.LogInformation($"{name}");
            string surname = data?.surname;
            log.LogInformation($"{surname}");
            string text = data?.text;
            log.LogInformation($"{text}");

            connectToDB(name, surname, text, log);

            string responseMessage = $"Data has been received. You've sent {name} {surname} {text}";

            return new OkObjectResult(responseMessage);
        }
    }
}
