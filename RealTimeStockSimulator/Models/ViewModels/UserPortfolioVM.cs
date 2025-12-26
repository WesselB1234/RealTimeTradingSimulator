namespace RealTimeStockSimulator.Models.ViewModels
{
    public class UserPortfolioVM
    {
        public UserAccount User;
        public List<OwnershipTradable> Tradables;

        public UserPortfolioVM(UserAccount user, List<OwnershipTradable> tradables)
        {
            User = user;
            Tradables = tradables;
        }
    }
}
