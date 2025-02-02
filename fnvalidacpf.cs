using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace httpValidaCpf
{
    public static class fnvalidacpf
    {
        [FunctionName("fnvalidacpf")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Iniciando a validação do CPF.");

            // Lê o corpo da requisição
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            // Verifica se o CPF foi informado
            if (data == null || string.IsNullOrEmpty(data?.cpf?.ToString()))
            {
                return new BadRequestObjectResult("Por favor, informe o CPF.");
            }

            // Obtém o CPF do corpo da requisição
            string cpf = data.cpf.ToString();

            // Valida o CPF
            bool isValid = ValidaCPF(cpf);

            if (!isValid)
            {
                return new OkObjectResult(new
                {
                    cpf = cpf,
                    isValid = false,
                    message = "CPF inválido."
                });
            }

            // Simula a verificação na base de dados de fraudes
            bool constaNaBaseDeFraudes = VerificaBaseDeFraudes(cpf);

            if (constaNaBaseDeFraudes)
            {
                return new OkObjectResult(new
                {
                    cpf = cpf,
                    isValid = true,
                    message = "CPF válido, mas consta na base de dados de fraudes."
                });
            }

            // Simula a verificação na base de dados de débitos
            bool constaNaBaseDeDebitos = VerificaBaseDeDebitos(cpf);

            if (constaNaBaseDeDebitos)
            {
                return new OkObjectResult(new
                {
                    cpf = cpf,
                    isValid = true,
                    message = "CPF válido, mas consta na base de dados de débitos."
                });
            }

            // Retorna a resposta se o CPF é válido e não consta em nenhuma base
            return new OkObjectResult(new
            {
                cpf = cpf,
                isValid = true,
                message = "CPF válido, não consta na base de dados de fraudes e não consta na base de débitos."
            });
        }

        public static bool ValidaCPF(string cpf)
        {
            // Remove caracteres não numéricos
            cpf = cpf.Replace(".", "").Replace("-", "");

            // Verifica se o CPF tem 11 dígitos ou se é uma sequência de números repetidos
            if (cpf.Length != 11 || !long.TryParse(cpf, out _) || new string(cpf[0], 11) == cpf)
            {
                return false;
            }

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            }
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            }
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            // Verifica se os dígitos calculados são iguais aos informados
            return cpf.EndsWith(digito1.ToString() + digito2.ToString());
        }

        // Simula a verificação na base de dados de fraudes
        public static bool VerificaBaseDeFraudes(string cpf)
        {
            // Aqui você pode integrar com uma base de dados real
            // Por enquanto, estamos simulando que alguns CPFs constam na base de fraudes
            string[] cpfFraudes = { "11111111111", "22222222222", "12345678909" };
            return Array.Exists(cpfFraudes, x => x == cpf);
        }

        // Simula a verificação na base de dados de débitos
        public static bool VerificaBaseDeDebitos(string cpf)
        {
            // Aqui você pode integrar com uma base de dados real
            // Por enquanto, estamos simulando que alguns CPFs constam na base de débitos
            string[] cpfDebitos = { "33333333333", "44444444444", "12345678909" };
            return Array.Exists(cpfDebitos, x => x == cpf);
        }
    }
}