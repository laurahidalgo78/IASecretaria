namespace IASecretaria.Models
{
    public class Answer
    {
        public List<string> questions { get; set; }
        public string answer { get; set; }
        public double confidenceScore { get; set; }
        public int id { get; set; }
        public string source { get; set; }
        public Metadata metadata { get; set; }
        public Dialog dialog { get; set; }
    }

    public class Dialog
    {
        public bool isContextOnly { get; set; }
        public List<object> prompts { get; set; }
    }

    public class Metadata
    {
        public string system_metadata_qna_edited_manually { get; set; }
    }

    public class RespuestaQaA
    {
        public List<Answer> answers { get; set; }
    }
}
