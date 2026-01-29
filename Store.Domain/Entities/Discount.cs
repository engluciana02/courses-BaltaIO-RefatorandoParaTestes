namespace Store.Domain.Entities
{
    public class Discount: Entity
    {
        public decimal Amount { get; private set; }
        public DateTime ExpiryDate { get; private set; }

        public Discount(decimal amount, DateTime expiryDate)
        {
            Amount = amount;
            ExpiryDate = expiryDate;
        }
        
        public bool IsValid()
        {
            return DateTime.Now <= ExpiryDate;
        }

        public decimal Value()
        {
            return IsValid() ? Amount : 0;
        }
    }
}
