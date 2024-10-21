namespace WEB_253504_Sapronenko.UI.Models
{
    public class UserCredentials
    {
        public string Type { get; set; } = "password";
        public bool Temporary { get; set; } = false;
        public string Value { get; set; }
    }
}
