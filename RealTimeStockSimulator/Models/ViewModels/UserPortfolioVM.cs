namespace RealTimeStockSimulator.Models.ViewModels
{
    public class UserPortfolioVM
    {
        public UserAccount User;
        public List<OwnershipAsset> Tradables;

        public UserPortfolioVM(UserAccount user, List<OwnershipAsset> tradables)
        {
            User = user;
            Tradables = tradables;
        }
    }
}
