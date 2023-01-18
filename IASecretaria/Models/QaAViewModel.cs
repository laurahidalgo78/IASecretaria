namespace IASecretaria.Models
{
    public class AnswerSpanRequest
    {
        public bool enable { get; set; }
        public int topAnswersWithSpan { get; set; }
        public string confidenceScoreThreshold { get; set; }
    }

    public class QaAModel
    {
        public int top { get; set; }
        public string question { get; set; }
        public bool includeUnstructuredSources { get; set; }
        public string confidenceScoreThreshold { get; set; }
        public AnswerSpanRequest answerSpanRequest { get; set; }
    }

    public class respuestaReconocimietoVoz
    {
        public string respuesta { get; set; }
        public string respuestaVideo { get; set; }
        public bool control { get; set; }
        public bool controlTeams { get; set; }
        public bool controlTeamsMensaje { get; set; }
        public bool controlMensajeTexto { get; set; }
        public bool controlMensajeTextoMensaje { get; set; }
        public string? contacto { get; set; }
    }
}
