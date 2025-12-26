namespace RealTimeStockSimulator.Models
{
    public class UserAccount
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public decimal Money { get; set; }

        public UserAccount(int userId, string userName, string email, string password, decimal money)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            Password = password;
            Money = money;
        }

        public UserAccount(int userId, string userName, string email, decimal money)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            Money = money;
        }

        public UserAccount()
        {

        }
    }
}
