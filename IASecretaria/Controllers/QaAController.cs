using IASecretaria.Models;
using IASecretaria.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;

namespace IASecretaria.Controllers
{
    public class QaAController : Controller
    {
        private readonly QaAServices _QaAservices;
        private readonly SecretariaHokmaContext _context;
        public QaAController(QaAServices qaAservices, SecretariaHokmaContext context)
        {
            _QaAservices = qaAservices;
            _context = context;
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
            config.SpeechSynthesisVoiceName = "es-co-GonzaloNeural";
            // Crea una nueva instancia del Speech
            using var synthesizer = new SpeechSynthesizer(config);
            // Convierte el texto a voz
            await synthesizer.SpeakTextAsync(respuesta1);
        }

        // Metodo que ejecuta la peticion a la API de preguntas y respuestas
        public async Task<string> PeticionQaA(string respuestaVoz)
        {
            var respuesta = "";
            string url = "https://secretariahokma.cognitiveservices.azure.com/language/:query-knowledgebases?projectName=QqaA-bot&api-version=2021-10-01&deploymentName=production";
            // Se incertan datos en el modelo
            if (respuestaVoz == "")
            {
                respuesta = "No tengo respuesta para esa pregunta";
            }
            else
            {
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
                respuesta = _QaAservices.EjecutarPostJson(modelserialize, url);
            }
            // Retorna la variable respuesta
            return respuesta;
        }


        // Metodo que ejecuta la peticion a la API de predicciones
        public string PeticionPrediction(string respuestaPeticion)
        {
            var resultadoPrediction = "";
            string urlPrediction = "https://secretariahokma.cognitiveservices.azure.com/language/:analyze-conversations?api-version=2022-10-01-preview";
            if (respuestaPeticion == "")
            {
                resultadoPrediction = "None";
            }
            else
            {
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
                        deploymentName = "Secretarie-deploy",
                        stringIndexType = "TextElement_V8",
                    }
                };
                // Se serializa el modelo
                string serializePrediction = JsonConvert.SerializeObject(prediction);
                // Se ejecuta el el POST y guardamos la respuesta en una variable
                resultadoPrediction = _QaAservices.EjecutarPostPrediction(serializePrediction, urlPrediction);
                Console.WriteLine(resultadoPrediction);
            }
            return resultadoPrediction;
        }

        public string PeticionPredictionNombre(string respuestaPeticion)
        {
            var resultadoPrediction = "";
            string urlPrediction = "https://secretariahokma.cognitiveservices.azure.com/language/:analyze-conversations?api-version=2022-10-01-preview";
            if (respuestaPeticion == "")
            {
                resultadoPrediction = "None";
            }
            else
            {
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
                        deploymentName = "Secretarie-deploy",
                        stringIndexType = "TextElement_V8",
                    }
                };
                // Se serializa el modelo
                string serializePrediction = JsonConvert.SerializeObject(prediction);
                // Se ejecuta el el POST y guardamos la respuesta en una variable
                resultadoPrediction = _QaAservices.EjecutarPostPredictionNombre(serializePrediction, urlPrediction);
                Console.WriteLine(resultadoPrediction);
            }
            return resultadoPrediction;
        }



        public bool PeticionTeams(string respuestaPeticion)
        {
            var resultadoPrediction = "";
            string urlPrediction = "https://hokma.webhook.office.com/webhookb2/3593d646-9840-4714-95f6-8e0fb1b3738e@a86fafe3-4e52-41e3-a67c-2fd14ec57d11/IncomingWebhook/689319ce689f483d8121edd27f8c594a/4d8d468f-8599-41fe-9187-34e9add3e075";
            if (respuestaPeticion == "")
            {
                resultadoPrediction = "None";
            }
            else
            {
                // Se incertan datos en el modelo
                MensajeTeamsViewModel mensaje = new MensajeTeamsViewModel()
                {
                    text = respuestaPeticion,
                };
                // Se serializa el modelo
                string serializePrediction = JsonConvert.SerializeObject(mensaje);
                // Se ejecuta el el POST y guardamos la respuesta en una variable
                _QaAservices.EjecutarPostTeams(serializePrediction, urlPrediction);
                Console.WriteLine(resultadoPrediction);
            }
            return true;
        }

        public async Task<string> PeticionSMS(string mensaje, string numero)
        {
            string urlPrediction = "http://api.labsmobile.com/json/send";


            // Se incertan datos en el modelo
            
            
            List<Recipent> recipenta = new List<Recipent>();
            recipenta.Add(new Recipent
            {
                msisdn= numero
            });
            SMSViewModel sms = new SMSViewModel()
            {
                message = mensaje,
                tpoa = "Sender",
                recipient= recipenta
            };

            
            
            // Se serializa el modelo
            string serializeSMS = JsonConvert.SerializeObject(sms);
            // Se ejecuta el el POST y guardamos la respuesta en una variable
            _QaAservices.EnviarSMS(serializeSMS, urlPrediction);
            return "";
        }
        

        //Metodo que de acuerdo a la intencion retorna una imagen
        public string videoPeticion(string intencion)
        {
            var secretariaHokmaContext = _context.Videos.Include(v => v.Intenciones);
            var listarVideos = secretariaHokmaContext.ToListAsync().Result;
            var objectJson = listarVideos.Count;
            var contador = 0;
            for (var i=0; i<listarVideos.Count; i++)
            {
                if(listarVideos[i].Intenciones.Descripcion == intencion)
                {
                    ViewBag.Saludo = listarVideos[i].Descripcion;
                    break;
                }
                contador++;
            }
            if (contador == listarVideos.Count)
            {
                ViewBag.Saludo = "https://user-images.githubusercontent.com/69684654/210588248-2c9489ba-7fd0-4434-a270-35891c50198a.mp4";
            }

            // Se valua la intencion y de acuerdo a la misma se retorna una imagen a la vista

            return ViewBag.Saludo;

        }

        public async Task<string> NombrePeticion(string nombre)
        {
            var secretariaHokmaContext = _context.Personas.Include(p => p.IdtipoContactoNavigation);
            var listarNombres = await secretariaHokmaContext.ToListAsync();
            var objectJson = listarNombres.Count;
            var personaContacto = "";
            var contador = 0;
            for (var i = 0; i < listarNombres.Count; i++)
            {
                if (listarNombres[i].Nombre == nombre)
                {
                    personaContacto = listarNombres[i].Contacto;
                    break;
                }
                else if (listarNombres[i].Apellido== nombre)
                {
                    personaContacto = listarNombres[i].Contacto;
                    break;
                }
            }



            // Se valua la intencion y de acuerdo a la misma se retorna una imagen a la vista

            return personaContacto;

        }


    }
}
