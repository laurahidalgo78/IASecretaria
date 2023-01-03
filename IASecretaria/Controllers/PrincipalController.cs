﻿using Microsoft.AspNetCore.Mvc;
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
            SecretariaHokmaContext secretariaHokmaContext = new SecretariaHokmaContext();
            // Crea una nueva instancia de la clase QaAController
            QaAController qaAController = new QaAController(qaAServices, secretariaHokmaContext);
            var speechConfig = SpeechConfig.FromSubscription("147d98b295e7495cae0589c5ce4d1cdd", "eastus");
            // Recibe la respuesta del del metodo que transforma la voz a texto
            var respuesta = await qaAController.ReconocimientoVoz(speechConfig);
            // Ejecuta el metodo PeticionPrediction
            respuestaPrediction = qaAController.PeticionPrediction(respuesta);
            ViewBag.resultado = respuestaPrediction;
            // Ejecuta el metodo videoPeticion
            respuestaVideo = qaAController.videoPeticion(respuestaPrediction);
            // Envia un mensaje al canal de Teams
            //qaAController.PeticionTeams(respuesta);
            respuestaReconocimietoVoz reconocimiento = new respuestaReconocimietoVoz();
            reconocimiento.respuesta = respuestaPrediction;
            reconocimiento.respuestaVideo = respuestaVideo;
            var jsonVideo = JsonConvert.SerializeObject(reconocimiento);
            ViewBag.hola = respuesta;
            return new JsonResult(jsonVideo);
            // Ejecuta el metodo PeticionQaA
            respuesta1 = await qaAController.PeticionQaA(respuesta);
            // Ejecuta el metodo que convierte el texto a voz
            await qaAController.ReconocimientoTexto(respuesta1, speechConfig);

            //var optionsParallelism = new ParallelOptions { MaxDegreeOfParallelism = 3 };

            //await Parallel.ForEachAsync(respuesta1, optionsParallelism, async (speechConfig, _) =>
            //{
            //    await qaAController.PeticionPrediction(respuesta);
            //});

        }
    }
}
