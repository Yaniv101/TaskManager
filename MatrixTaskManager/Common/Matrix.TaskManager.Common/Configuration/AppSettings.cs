namespace Matrix.TaskManager.Common.Configuration
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public EmailData Email { get; set; }
    }
    public class EmailData
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }

}