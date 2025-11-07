using Flunt.Validations;

namespace Refactoring.Models
{
  public class OrderItem:Entity
  {
    public Product Product { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }

    public OrderItem(Product product, decimal price, int quantity)
    {
      AddNotifications(
        new Contract()
          .Requires()
          .IsNotNull(product, "Product", "Produto inválido")
          .IsGreaterThan(quantity,0,"Quantidade", "A quantidade tem que ser maior que 0")          
      );

      Product = product;
      Price = price;
      Quantity = quantity;
    }

    public decimal Total()
    {
      return Price * Quantity;
    }
  }
}
