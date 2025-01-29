using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace httpValidaCpf
{
    public static class fnvalidacpf
    {
        [FunctionName("fnvalidacpf")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,  "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Iniciando a validação do cpf.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if(data ==null)
                return new BadRequestObjectResult("Por favor , informe o CPF.");

            string cpf = data?.cpf;

            if(ValidaCPF(cpf))
                return new BadRequestObjectResult("CPF inválido.");
  
            return new OkObjectResult("CPF válido.");
        }

        public static bool ValidaCPF(string cpf){
            if(string.IsNullOrEmpty(cpf))
            return false;

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");

            string pattern = @"^(?!00000000000$)\d{11}$";
            return !Regex.IsMatch(cpf, pattern);
        }
    }
}
