using Flunt.Validations;
using Refactoring.Enums;

namespace Refactoring.Models
{
  public class Order:Entity
  {
    public Customer Customer { get; private set; }
    public DateTime Date { get; private set; }
    public string Number { get; private set; }
    public IList<OrderItem> Items { get; private set; }
    public decimal DeliveryFee { get; private set; }
    public Discount Discount { get; private set; }
    public EOrderStatus Status { get; private set; }

    public Order(Customer customer, decimal deliveryFee, Discount discount)
    {
      AddNotifications(
        new Contract()
          .Requires()
          .IsNotNull(customer, "Custumer", "Cliente inv�lido")
      );

      Customer = customer;
      DeliveryFee = deliveryFee;
      Discount = discount;
      Items = new List<OrderItem>();
      Date = DateTime.Now;
      Number = Guid.NewGuid().ToString().Substring(0, 8);
      Status = EOrderStatus.WaitingPayment;
    }
    public void AddItems(Product product, decimal price, int quantity)
    {
      var item = new OrderItem(product, price, quantity);
      if (item.Valid)
        Items.Add(item);
    }

    public decimal Total()
    {
      decimal total = 0;
      foreach (var item in Items)
        total += item.Total();

      total += DeliveryFee;
      total -= Discount.Value();
      return total;
    }
    public void Pay(decimal amount)
    {
      if (amount == Total())
        this.Status = EOrderStatus.WaitingDelivery;
    }
    public void Cancel()
    {
      Status = EOrderStatus.Canceled;
    }
  }
}
