using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;
using IASecretaria.Services;
using IASecretaria.Models;
using Newtonsoft.Json;
using System.Threading;

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
            SecretariaHokmaContext secretariaHokmaContext = new SecretariaHokmaContext();
            // Crea una nueva instancia de la clase QaAController
            QaAController qaAController = new QaAController(qaAServices, secretariaHokmaContext);
            var speechConfig = SpeechConfig.FromSubscription("edc1030e34044a2ab44f0ce1f5be4aa7", "eastus");
            // Recibe la respuesta del del metodo que transforma la voz a texto
            var respuesta = await qaAController.ReconocimientoVoz(speechConfig);
            // Ejecuta el metodo PeticionPrediction
            respuestaPrediction = qaAController.PeticionPrediction(respuesta);
            ViewBag.resultado = respuestaPrediction;
            // Ejecuta el metodo videoPeticion
            respuestaVideo = qaAController.videoPeticion(respuestaPrediction);
            if(respuestaPrediction == "Enviar mensaje de texto")
            {
                var respuestaNombre = qaAController.PeticionPredictionNombre(respuesta);
                var contacto = qaAController.NombrePeticion(respuestaNombre);
                EnviarMensajeTexto(contacto.Result);
            }
            
            respuestaReconocimietoVoz reconocimiento = new respuestaReconocimietoVoz();
            reconocimiento.respuesta = respuestaPrediction;
            reconocimiento.respuestaVideo = respuestaVideo;

            var jsonVideo = JsonConvert.SerializeObject(reconocimiento);

            ViewBag.hola = respuesta;
           
            return new JsonResult(jsonVideo);
            
            //var optionsParallelism = new ParallelOptions { MaxDegreeOfParallelism = 3 };

            //await Parallel.ForEachAsync(respuesta1, optionsParallelism, async (speechConfig, _) =>
            //{
            //    await qaAController.PeticionPrediction(respuesta);
            //});

        }



        public async Task<JsonResult> BotFunction()
        {
            // Crea una nueva instancia de la clase QaAServices
            QaAServices qaAServices = new QaAServices();
            SecretariaHokmaContext secretariaHokmaContext = new SecretariaHokmaContext();
            // Crea una nueva instancia de la clase QaAController
            QaAController qaAController = new QaAController(qaAServices, secretariaHokmaContext);
            var speechConfig = SpeechConfig.FromSubscription("edc1030e34044a2ab44f0ce1f5be4aa7", "eastus");
            var respuesta = qaAController.ReconocimientoVoz(speechConfig);
            // Ejecuta el metodo PeticionQaA
            var respuesta1 = qaAController.PeticionQaA(respuesta.Result);
            Thread.Sleep(1000);
            // Ejecuta el metodo que convierte el texto a voz
            await qaAController.ReconocimientoTexto(respuesta1.Result, speechConfig);
            respuestaReconocimietoVoz reconocimiento = new respuestaReconocimietoVoz();
            reconocimiento.control = true;
            var jsonControl = JsonConvert.SerializeObject(reconocimiento);

            return new JsonResult(jsonControl);
        }

        public async Task<JsonResult>EnviarMensajeTeams()
        {
            // Crea una nueva instancia de la clase QaAServices
            QaAServices qaAServices = new QaAServices();
            SecretariaHokmaContext secretariaHokmaContext = new SecretariaHokmaContext();
            // Crea una nueva instancia de la clase QaAController
            QaAController qaAController = new QaAController(qaAServices, secretariaHokmaContext);
            var speechConfig = SpeechConfig.FromSubscription("edc1030e34044a2ab44f0ce1f5be4aa7", "eastus");
            // Recibe la respuesta del del metodo que transforma la voz a texto
            Thread.Sleep(3000);
            var mensaje = await qaAController.ReconocimientoVoz(speechConfig);
            qaAController.PeticionTeams(mensaje);
            respuestaReconocimietoVoz reconocimiento = new respuestaReconocimietoVoz();
            reconocimiento.controlTeams = true;
            var jsonControl = JsonConvert.SerializeObject(reconocimiento);
            await qaAController.ReconocimientoTexto("Tu mensaje ha sido enviado con exito", speechConfig);
            reconocimiento.controlTeamsMensaje = true;

            return new JsonResult(jsonControl);

        }

        public async Task<JsonResult>EnviarMensajeTexto(string contacto)
        {
            // Crea una nueva instancia de la clase QaAServices
            QaAServices qaAServices = new QaAServices();
            SecretariaHokmaContext secretariaHokmaContext = new SecretariaHokmaContext();
            // Crea una nueva instancia de la clase QaAController
            QaAController qaAController = new QaAController(qaAServices, secretariaHokmaContext);

            var speechConfig = SpeechConfig.FromSubscription("edc1030e34044a2ab44f0ce1f5be4aa7", "eastus");
            
            
            // Recibe la respuesta del del metodo que transforma la voz a texto
            Thread.Sleep(3000);
            var mensaje = await qaAController.ReconocimientoVoz(speechConfig);
            qaAController.PeticionSMS(mensaje, contacto);

            Thread.Sleep(2000);
            await qaAController.ReconocimientoTexto("Tu mensaje ha sido enviado con exito", speechConfig);
            respuestaReconocimietoVoz reconocimiento = new respuestaReconocimietoVoz();
            reconocimiento.controlMensajeTexto = true;
            var jsonControl = JsonConvert.SerializeObject(reconocimiento);

            return new JsonResult(jsonControl);
        }
    }
}
