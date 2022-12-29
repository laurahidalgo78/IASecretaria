
namespace IASecretaria.Models
{
    public class AnalysisInput
    {
        public ConversationItem conversationItem { get; set; }
    }

    public class ConversationItem
    {
        public string id { get; set; }
        public string text { get; set; }
        public string modality { get; set; }
        public string language { get; set; }
        public string participantId { get; set; }
    }

    public class Parameters
    {
        public string projectName { get; set; }
        public bool verbose { get; set; }
        public string deploymentName { get; set; }
        public string stringIndexType { get; set; }
    }

    public class Prediction
    {
        public string kind { get; set; }
        public AnalysisInput analysisInput { get; set; }
        public Parameters parameters { get; set; }
    }
}
