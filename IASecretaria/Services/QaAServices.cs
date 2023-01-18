using Azure.Core;
using IASecretaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace IASecretaria.Services
{
    public class QaAServices
    {
        // Realiza la peticion enviando un JSON a la API CLU Bot
        public string EjecutarPostJson(string DatosAcceso, string urlApi)
        {
            try
            {
                // Crea una nueva instancia de HttpClient
                HttpClient client = new HttpClient();
                // Configura la Url a la que se le va a hacer la peticion
                client.BaseAddress = new Uri(urlApi);
                // Añade los headers a la Api para hacer la peticion
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "b29ad334b7dd4102a17012758674b63d");
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                // ACCEPT header

                var request = new HttpRequestMessage(HttpMethod.Post, urlApi);

                // CONTENT-TYPE header

                request.Content = new StringContent(DatosAcceso,
                Encoding.UTF8, "application/json");

                // Realiza la peticion y recibe la respuesta
                var ejemplo = client.SendAsync(request).ContinueWith(responseTask =>
               responseTask.Result).Result;

                // Lee el contenido de la peticion y lo guarda en una variable
                var respuestabonita = ejemplo.Content.ReadAsStringAsync();
                // Convierte el contenido de la peticion en string
                var deserializacion = respuestabonita.Result.ToString();
                // Deserializa el Json y lo guarda en el modelo RespuestaQaA
                var jsonserialize = JsonConvert.DeserializeObject<RespuestaQaA>(deserializacion);

                // Retorna la respuesta de la api
                return jsonserialize.answers[0].answer;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        // Realiza la peticion enviando un JSON a la API de Prediccion de intenciones
        public string EjecutarPostPrediction(string DatosAcceso, string urlApi)
        
        {
            try
            {

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "b29ad334b7dd4102a17012758674b63d");
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                //client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                // ACCEPT header

                var request = new HttpRequestMessage(HttpMethod.Post, urlApi);

                // CONTENT-TYPE header

                request.Content = new StringContent(DatosAcceso,
                Encoding.UTF8, "application/json");


                var ejemplo = client.SendAsync(request).ContinueWith(responseTask =>
               responseTask.Result).Result;

                var respuestabonita = ejemplo.Content.ReadAsStringAsync();
                var deserializacion = respuestabonita.Result.ToString();
                var jsonserialize = JsonConvert.DeserializeObject<RespuestaPrediction>(deserializacion);


                return jsonserialize.result.prediction.topIntent;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string EjecutarPostPredictionNombre(string DatosAcceso, string urlApi)
        {
            try
            {

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "b29ad334b7dd4102a17012758674b63d");
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                //client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                // ACCEPT header

                var request = new HttpRequestMessage(HttpMethod.Post, urlApi);

                // CONTENT-TYPE header

                request.Content = new StringContent(DatosAcceso,
                Encoding.UTF8, "application/json");


                var ejemplo = client.SendAsync(request).ContinueWith(responseTask =>
               responseTask.Result).Result;

                var respuestabonita = ejemplo.Content.ReadAsStringAsync();
                var deserializacion = respuestabonita.Result.ToString();
                var jsonserialize = JsonConvert.DeserializeObject<RespuestaPrediction>(deserializacion);
                var entidades = jsonserialize.result.prediction.entities[0].text;
                


                return entidades;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        // Realiza la peticion enviando un JSON a la API de Prediccion de intenciones
        public async Task<bool> EjecutarPostTeams(string DatosAcceso, string urlApi)
        {
            try
            {

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(urlApi);
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                // ACCEPT header

                var request = new HttpRequestMessage(HttpMethod.Post, urlApi);

                // CONTENT-TYPE header

                request.Content = new StringContent(DatosAcceso,
                Encoding.UTF8, "application/json");


                var ejemplo = client.SendAsync(request).ContinueWith(responseTask =>
               responseTask.Result).Result;

                if (ejemplo.ReasonPhrase == "OK")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public string EnviarSMS(string DatosAcceso, string url)
        {
            RestRequest request = new RestRequest();
            RestClient client = new RestClient();
            client = new RestClient(url);
            client.Options(new RestRequest() { Timeout = -1 });
            request = new RestRequest(url, RestSharp.Method.Post);
            request.AddHeader("Authorization", "Basic cGF1bG8uZGVsYWNydXpAaG9rbWEuYWk6UGNkbGNlMzIxTGxicjMyMTBvJA==");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", DatosAcceso, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            var responsedeserialize = JsonConvert.DeserializeObject<RespuestaSMSViewModel>(response.Content);
            var responseMessage = responsedeserialize.message;
            Console.WriteLine(responseMessage);
            return responseMessage;
        }
    }
}
