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

            respuestaReconocimietoVoz reconocimiento = new respuestaReconocimietoVoz();
            // Crea una nueva instancia de la clase QaAServices
            QaAServices qaAServices = new QaAServices();
            SecretariaHokmaContext secretariaHokmaContext = new SecretariaHokmaContext();
            // Crea una nueva instancia de la clase QaAController
            QaAController qaAController = new QaAController(qaAServices, secretariaHokmaContext);
            var speechConfig = SpeechConfig.FromSubscription("1517f3feb1a04d6f81ed37a1bdf7cf4c", "eastus");
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
                reconocimiento.contacto = contacto.Result;
            }
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
            var speechConfig = SpeechConfig.FromSubscription("1517f3feb1a04d6f81ed37a1bdf7cf4c", "eastus");
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

        public async Task<JsonResult>ConfirmacionMensajeTeams()
        {
            respuestaReconocimietoVoz reconocimiento = new respuestaReconocimietoVoz();
            // Crea una nueva instancia de la clase QaAServices
            QaAServices qaAServices = new QaAServices();
            SecretariaHokmaContext secretariaHokmaContext = new SecretariaHokmaContext();
            // Crea una nueva instancia de la clase QaAController
            QaAController qaAController = new QaAController(qaAServices, secretariaHokmaContext);
            var speechConfig = SpeechConfig.FromSubscription("1517f3feb1a04d6f81ed37a1bdf7cf4c", "eastus");
            // Recibe la respuesta del del metodo que transforma la voz a texto
            Thread.Sleep(4000);
            var mensaje = await qaAController.ReconocimientoVoz(speechConfig);
            bool resultmessage = await qaAController.PeticionTeams(mensaje);
            if (resultmessage)
            {
                await qaAController.ReconocimientoTexto("Tu mensaje ha sido enviado con exito", speechConfig);
            }
            else
            {
                await qaAController.ReconocimientoTexto("Tu mensaje no ha sido enviado", speechConfig);
            }
            reconocimiento.controlTeamsMensaje = true;
            var jsonControl = JsonConvert.SerializeObject(reconocimiento);


            return new JsonResult(jsonControl);
        }

        public async Task<JsonResult>EnviarMensajeTexto(string contacto)
        {
            // Crea una nueva instancia de la clase QaAServices
            QaAServices qaAServices = new QaAServices();
            SecretariaHokmaContext secretariaHokmaContext = new SecretariaHokmaContext();
            // Crea una nueva instancia de la clase QaAController
            QaAController qaAController = new QaAController(qaAServices, secretariaHokmaContext);

            var speechConfig = SpeechConfig.FromSubscription("1517f3feb1a04d6f81ed37a1bdf7cf4c", "eastus");
            
            
            // Recibe la respuesta del del metodo que transforma la voz a texto
            Thread.Sleep(4000);
            var mensaje = await qaAController.ReconocimientoVoz(speechConfig);
            Thread.Sleep(1000);
            string mensajeconfirmacion = await qaAController.PeticionSMS(mensaje, contacto);
            if(mensajeconfirmacion == "Message has been successfully sent.")
            {
                await qaAController.ReconocimientoTexto("Tu mensaje ha sido enviado con exito", speechConfig);
            }
            else
            {
                await qaAController.ReconocimientoTexto("Tu mensaje no ha sido enviado", speechConfig);
            }
            respuestaReconocimietoVoz reconocimiento = new respuestaReconocimietoVoz();
            reconocimiento.controlMensajeTexto = true;
            var jsonControl = JsonConvert.SerializeObject(reconocimiento);

            return new JsonResult(jsonControl);
        }
    }
}
