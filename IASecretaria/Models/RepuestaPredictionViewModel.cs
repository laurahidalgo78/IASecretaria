namespace IASecretaria.Models
{
    public class Intent
    {
        public string category { get; set; }
        public double confidenceScore { get; set; }
    }

    public class Prediction2
    {
        public string topIntent { get; set; }
        public string projectKind { get; set; }
        public List<Intent> intents { get; set; }
        public List<object> entities { get; set; }
    }

    public class Result
    {
        public string query { get; set; }
        public Prediction2 prediction { get; set; }
    }

    public class RespuestaPrediction
    {
        public string kind { get; set; }
        public Result result { get; set; }
    }
}
