namespace RealTimeStockSimulator.Models
{
    public class Ownership
    {
        public User User;
        public List<OwnershipTradable> Tradables;

        public Ownership(User user, List<OwnershipTradable> tradables)
        {
            User = user;
            Tradables = tradables;
        }
    }
}
