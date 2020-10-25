using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace pizzatrackerdonorv2
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string Status = String_Status(name);
            string fname = String_fname(name);
            string lname = String_lname(name);

            string Donor = donor(Status, fname, lname);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            name = name ?? data?.name;
            Donor = Donor ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"{Donor}";

            return new OkObjectResult(responseMessage);
        }

        private static string donor(string Status, string fname, string lname )
        {
            return $@"{{""status"":""{Status}"", ""fname"":""{fname}"", ""lname"":""{lname}""}}";
        }

        private static string String_Status(string name)
        {
            var connStr = "Server=tcp:mail-server.database.windows.net,1433;Initial Catalog=Results;Persist Security Info=False;User ID=bitshift;Password=bitpizz@12;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sqlStr = $"SELECT status FROM [dbo].[donor] WHERE donor_id = " + name;
                using (SqlCommand cmd = new SqlCommand(sqlStr, conn))
                {
                    return cmd.ExecuteScalar().ToString();
                }
                //conn.Close();

            }

        }

        private static string String_fname(string name)
        {
            var connStr = "Server=tcp:mail-server.database.windows.net,1433;Initial Catalog=Results;Persist Security Info=False;User ID=bitshift;Password=bitpizz@12;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sqlStr = $"SELECT fname FROM [dbo].[donor] WHERE donor_id = " + name;
                using (SqlCommand cmd = new SqlCommand(sqlStr, conn))
                {
                    return cmd.ExecuteScalar().ToString();
                }
                //conn.Close();

            }

        }

        private static string String_lname(string name)
        {
            var connStr = "Server=tcp:mail-server.database.windows.net,1433;Initial Catalog=Results;Persist Security Info=False;User ID=bitshift;Password=bitpizz@12;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sqlStr = $"SELECT lname FROM [dbo].[donor] WHERE donor_id = " + name;
                using (SqlCommand cmd = new SqlCommand(sqlStr, conn))
                {
                    return cmd.ExecuteScalar().ToString();
                }
                //conn.Close();

            }

        }


    }


}
