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
        public async Task<IActionResult> Principal()
        {
           
            return View();
            
        }

        public async Task<IActionResult> LanzarConsola()
        {
            ViewBag.hola = null;
            return View();
        }
            [HttpPost]
        
        public async Task<JsonResult> LanzarConsola(int nose)
        {
            
            var respuesta1 = "";
            var respuestaPrediction = "";
            var respuestaVideo = "";

            //Find your key and resource region under the 'Keys and Endpoint' tab in your Speech resource in Azure Portal
            //Remember to delete the brackets <> when pasting your key and region!
            var speechConfig = SpeechConfig.FromSubscription("147d98b295e7495cae0589c5ce4d1cdd", "eastus");
            QaAServices qaAServices = new QaAServices();
            // Recibe la respuesta del del metodo que transforma la voz a texto
            var respuesta = await RecognizeFromMic(speechConfig);
            ViewBag.hola = respuesta;
            // Crea una nueva instancia de la clase QaAServices
            var consulta = new QaAServices();

            QaAController qaA = new QaAController(consulta);
            respuesta1 = qaA.PeticionQaA(respuesta);
            respuestaPrediction = qaA.PeticionPrediction(respuesta);
            respuestaVideo = qaA.videoPeticion(respuestaPrediction);
            Console.WriteLine(respuestaVideo);


            await SynthesizeToSpeaker(respuesta1, speechConfig);

            ViewBag.resultado = respuestaPrediction;
            ViewBag.video = respuestaVideo;

            
            respuestaReconocimietoVoz reconocimiento = new respuestaReconocimietoVoz();
            reconocimiento.respuesta = respuestaPrediction;
            reconocimiento.respuestaVideo = respuestaVideo;
            var jsonVideo = JsonConvert.SerializeObject(reconocimiento);
            return new JsonResult(jsonVideo);
            
        }

        static async Task<string> RecognizeFromMic(SpeechConfig speechConfig)
        {
            // Recibe la configuracion del microfono
            speechConfig.SpeechRecognitionLanguage = "es-MX";
            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            // Crea una nueva instancia del reconocedor de voz
            using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

            //Asks user for mic input and prints transcription result on screen
            Console.WriteLine("Speak into your microphone.");
            // Recibe la voz y la convierte en texto
            var result = await recognizer.RecognizeOnceAsync();
            // Imprime en pantalla el reconocimiento de voz
            Console.WriteLine($"RECOGNIZED: Text={result.Text}");
            // Retorna el resultado del reconocimiento de voz
            return result.Text;
        }

        // Metodo para convertir el texto a voz
        async static Task SynthesizeToSpeaker(string respuesta1, SpeechConfig config)
        {
            // Permite Cambiar la voz del interprete
            config.SpeechSynthesisVoiceName = "es-MX-JorgeNeural";
            // Crea una nueva instancia del Speech
            using var synthesizer = new SpeechSynthesizer(config);
            // Convierte el texto a voz
            await synthesizer.SpeakTextAsync(respuesta1);
        }

        // Metodo principal, que contiene la configuracion de los metodos anteriores
        //static async Task Main(string[] args)
        //{
            

        //}
    }
}
