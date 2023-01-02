using IASecretaria.Models;
using IASecretaria.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace IASecretaria.Controllers
{
    public class QaAController : Controller
    {
        private readonly QaAServices _QaAservices;
        public QaAController(QaAServices qaAservices)
        {
            _QaAservices = qaAservices;
        }

        // Metodo que se encarga de convertir la voz a texto
        public async Task<string> ReconocimientoVoz(SpeechConfig speechConfig)
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

        // Metodo que se encarga de convertir el texto a voz
        public async Task ReconocimientoTexto(string respuesta1, SpeechConfig config)
        {
            // Permite Cambiar la voz del interprete
            config.SpeechSynthesisVoiceName = "es-MX-JorgeNeural";
            // Crea una nueva instancia del Speech
            using var synthesizer = new SpeechSynthesizer(config);
            // Convierte el texto a voz
            await synthesizer.SpeakTextAsync(respuesta1);
        }

        // Metodo que ejecuta la peticion a la API de preguntas y respuestas
        public string PeticionQaA(string respuestaVoz)
        {
            string url = "https://lr-luis.cognitiveservices.azure.com/language/:query-knowledgebases?projectName=QaA-clu&api-version=2021-10-01&deploymentName=production";
            // Se incertan datos en el modelo
            QaAModel modelQaA = new QaAModel()
            {
                top = 3,
                question = respuestaVoz,
                includeUnstructuredSources = true,
                confidenceScoreThreshold = "0.8",
                answerSpanRequest = new AnswerSpanRequest()
                {
                    enable = true,
                    topAnswersWithSpan = 1,
                    confidenceScoreThreshold = "0.8"
                }
            };
            // Se serializa el modelo
            string modelserialize = JsonConvert.SerializeObject(modelQaA);

            // Se ejecuta el el POST y guardamos la respuesta en una variable
            var respuesta = _QaAservices.EjecutarPostJson(modelserialize, url);
            // Retorna la variable respuesta
            return respuesta;
        }


        // Metodo que ejecuta la peticion a la API de predicciones
        public string PeticionPrediction(string respuestaPeticion)
        {
            string urlPrediction = "https://lr-luis.cognitiveservices.azure.com/language/:analyze-conversations?api-version=2022-10-01-preview";
            // Se incertan datos en el modelo
            Prediction prediction = new Prediction()
            {
                kind = "Conversation",
                analysisInput = new AnalysisInput()
                {
                    conversationItem = new ConversationItem()
                    {
                        id = "1",
                        text = respuestaPeticion,
                        modality = "text",
                        language = "en",
                        participantId = "1",
                    }
                },
                parameters = new Parameters()
                {
                    projectName = "Secretarie",
                    verbose = true,
                    deploymentName = "DeploySecretarie",
                    stringIndexType = "TextElement_V8",
                }
            };
            // Se serializa el modelo
            string serializePrediction = JsonConvert.SerializeObject(prediction);
            // Se ejecuta el el POST y guardamos la respuesta en una variable
            var resultadoPrediction = _QaAservices.EjecutarPostPrediction(serializePrediction, urlPrediction);
            Console.WriteLine(resultadoPrediction);
            return resultadoPrediction;
        }

        //Metodo que de acuerdo a la intencion retorna una imagen
        public string videoPeticion(string intencion)
        {
            // Se evalua la intencion y de acuerdo a la misma se retorna una imagen a la vista
            switch (intencion)
            {
                case "Saludo":
                    return ViewBag.saludo = "https://cdn.domestika.org/c_limit,dpr_auto,f_auto,q_auto,w_820/v1586839695/content-items/004/191/065/LOOP__SALUDANDO-original.gif?1586839695"; 
                    break;
                case "Despedida":
                    return ViewBag.saludo = "https://pa1.narvii.com/6955/5c74426ec29e927901e1f1152a88b317808fe3cdr1-500-280_hq.gif";
                    break;
                case "Preguntas empleados":
                    return ViewBag.saludo = "https://gifimage.net/wp-content/uploads/2018/04/personas-hablando-gif-animados.gif";
                    break;
                case "Preguntas empresa":
                    return ViewBag.saludo = "https://gifimage.net/wp-content/uploads/2018/04/personas-hablando-gif-animados.gif";
                    break;
                default:
                    return ViewBag.saludo = "https://images.emojiterra.com/google/android-11/512px/2753.png";
                    break;
            }

        }

    }
}
