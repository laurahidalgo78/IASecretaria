namespace IASecretaria.Models
{
    public class Recipent 
    {
        public string msisdn { get; set; }
        
    }

    public class SMSViewModel
    {
        public string message { get; set; }
        public string tpoa { get; set; }

        public List<Recipent> recipient { get; set; }
    }
}
