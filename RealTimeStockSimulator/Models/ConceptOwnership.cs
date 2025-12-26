namespace RealTimeStockSimulator.Models
{
    public class ConceptOwnership
    {
        public UserAccount User;
        public Dictionary<String, int> OwnedAmountOfSymbol;

        public ConceptOwnership(UserAccount user, Dictionary<string, int> ownedAmountOfSymbol)
        {
            User = user;
            OwnedAmountOfSymbol = ownedAmountOfSymbol;
        }
    }
}
