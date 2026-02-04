using Store.Domain.Enums;

namespace Store.Domain.Entities
{
    public class Order: Entity
    {
        public Customer Customer { get; private set; }
        public DateTime Date { get; private set; }
        public IList<OrderItem> Items { get; private set; }= new List<OrderItem>();
        public decimal DeliveriyFee { get; private set; }
        public Discount Discount { get; private set; }
        public EnumOrderStatus Status { get; private set; }
        public string OrderNumber { get; private set; }

        public Order(Customer customer, decimal deliveriyFee, Discount discount)
        {
            AddNotifications(
                new Flunt.Validations.Contract()
                    .Requires()
                    .IsNotNull(customer,"Customer", "Invalid client!")
                    .IsGreaterThan(deliveriyFee,0,"DeliverFee","Delivery Fee is 0!"));

            Customer = customer;
            Date = DateTime.Now;
            DeliveriyFee = deliveriyFee;
            Discount = discount;
            Status = EnumOrderStatus.WaitingPayment;
            OrderNumber = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
        }

        public void AddItem(Product product, int qt)
        {
            AddNotifications(
                new Flunt.Validations.Contract()
                    .Requires()
                    .IsNotNull(product,"Product", "Invalid product!")
                    .IsGreaterThan(qt,0,"Qt","Quantity must be greater than 0!"));

            var item = new OrderItem(product, qt);
            if (item.Valid)
                Items.Add(item);
        }
        public decimal Total()
        {
            decimal total = 0;

            AddNotifications(
                new Flunt.Validations.Contract()
                    .Requires()
                    .IsGreaterThan(Items.Count, 0,"Items","The order must have at least one item!")
                    .IsLowerOrEqualsThan(total,0,"Total", "The total of order is lower or 0!" ));
            foreach (var item in Items)
            {
                total += item.Total();
            }
            total += DeliveriyFee;
            total -= Discount != null ? Discount.Value() : 0;
            return total;
        }       

        public void Pay(decimal amount)
        {
            if (amount == Total())
               this.Status = EnumOrderStatus.WaitingDelivery;
        }
         public void Cancel()
        {
            Status = EnumOrderStatus.Canceled;
        }
    }
}
