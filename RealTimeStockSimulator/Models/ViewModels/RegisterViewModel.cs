namespace RealTimeStockSimulator.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterViewModel(string userName, string email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }

        public RegisterViewModel()
        {

        }
    }
}
