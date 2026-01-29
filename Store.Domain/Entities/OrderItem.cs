namespace Store.Domain.Entities
{
    public class OrderItem:Entity
    {
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        
        public OrderItem(Product product, int quantity)
        {
            AddNotifications(
                new Flunt.Validations.Contract()
                    .Requires()
                    .IsNotNull(product,"Product", "Invalid product!")
                    .IsGreaterThan(quantity,0,"Quantity", "Quantity must be greater than 0!"));

            Product = product;
            Quantity = quantity;
            Price = product != null ? product.Price : 0;
            
        }
        public decimal Total()
        {
            return Price * Quantity;
        }
    }
}
