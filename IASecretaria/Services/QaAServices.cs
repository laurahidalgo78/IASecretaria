using IASecretaria.Models;
using Newtonsoft.Json;
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
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "77fe100e81eb4e9fbd8696f69a5fe4bb");
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
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "77fe100e81eb4e9fbd8696f69a5fe4bb");
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
    }
}
