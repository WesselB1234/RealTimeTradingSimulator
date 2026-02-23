namespace RealTimeStockSimulator.Models.ViewModels
{
    public class UserPortfolioVM
    {
        public UserAccount User;
        public List<OwnershipAsset> OwnershipAssets;

        public UserPortfolioVM(UserAccount user, List<OwnershipAsset> ownershipAssets)
        {
            User = user;
            OwnershipAssets = ownershipAssets;
        }
    }
}
