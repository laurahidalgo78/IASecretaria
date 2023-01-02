using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;
using IASecretaria.Services;
using IASecretaria.Models;
using Newtonsoft.Json;

namespace IASecretaria.Controllers
{
    public class PrincipalController : Controller
    {
        // Metodo que retorna la vista principal
        public async Task<IActionResult> Principal()
        {
           
            return View();
            
        }

        //Metodo que retorna la vista que va a mostrar el resultado
        public async Task<IActionResult> LanzarConsola()
        {
            ViewBag.hola = null;
            return View();
        }
        
        // Metodo principal que contiene todos los metodos y las instancias
        [HttpPost]
        public async Task<JsonResult> LanzarConsola(int nose)
        {
            
            var respuesta1 = "";
            var respuestaPrediction = "";
            var respuestaVideo = "";

            // Crea una nueva instancia de la clase QaAServices
            QaAServices qaAServices = new QaAServices();
            // Crea una nueva instancia de la clase QaAController
            QaAController qaAController = new QaAController(qaAServices);
            var speechConfig = SpeechConfig.FromSubscription("147d98b295e7495cae0589c5ce4d1cdd", "eastus");
            // Recibe la respuesta del del metodo que transforma la voz a texto
            var respuesta = await qaAController.ReconocimientoVoz(speechConfig);

            // Ejecuta el metodo PeticionQaA
            respuesta1 = qaAController.PeticionQaA(respuesta);
            // Ejecuta el metodo PeticionPrediction
            respuestaPrediction = qaAController.PeticionPrediction(respuesta);
            // Ejecuta el metodo videoPeticion
            respuestaVideo = qaAController.videoPeticion(respuestaPrediction);
            // Ejecuta el metodo que convierte el texto a voz
            await qaAController.ReconocimientoTexto(respuesta1, speechConfig);

            ViewBag.resultado = respuestaPrediction;
            ViewBag.video = respuestaVideo;

            
            respuestaReconocimietoVoz reconocimiento = new respuestaReconocimietoVoz();
            reconocimiento.respuesta = respuestaPrediction;
            reconocimiento.respuestaVideo = respuestaVideo;
            var jsonVideo = JsonConvert.SerializeObject(reconocimiento);
            ViewBag.hola = respuesta;
            return new JsonResult(jsonVideo);

        }
    }
}
