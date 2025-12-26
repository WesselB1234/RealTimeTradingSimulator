namespace RealTimeStockSimulator.Models
{
    public class Ownership
    {
        public UserAccount User;
        public List<OwnershipTradable> Tradables;
        public decimal TotalOwnershipValue
        {
            get
            {
                decimal total = 0;

                foreach (OwnershipTradable tradable in Tradables)
                {
                    total += tradable.TotalValue;
                }

                return total;
            }
        }

        public Ownership(UserAccount user, List<OwnershipTradable> tradables)
        {
            User = user;
            Tradables = tradables;
        }

        public Ownership() { }
    }
}
