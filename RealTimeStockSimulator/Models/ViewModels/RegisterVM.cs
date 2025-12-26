namespace RealTimeStockSimulator.Models.ViewModels
{
    public class RegisterVM
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterVM(string userName, string email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }

        public RegisterVM()
        {

        }
    }
}
