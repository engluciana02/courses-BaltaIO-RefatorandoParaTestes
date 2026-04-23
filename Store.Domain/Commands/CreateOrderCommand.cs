using Flunt.Notifications;
using Store.Domain.Commands.Interfaces;

namespace Store.Domain.Commands
{
    public class CreateOrderCommand:Notifiable, ICommand
    {
        public string Customer { get; set; }    
        public string ZipCode { get; set; }
        public string PromoCode { get; set; }

        public List<CreateOrderItemCommand> Items { get; set; } = new List<CreateOrderItemCommand>();

        public CreateOrderCommand() 
        {
        }
        public CreateOrderCommand(string customer, string zipCode, string promoCode, List<CreateOrderItemCommand> items)
        {
            Customer = customer;
            ZipCode = zipCode;
            PromoCode = promoCode;
            Items = items;
        }

        public void Validate()
        {
            AddNotifications(new Flunt.Validations.Contract()
                .Requires()
                .HasMinLen(Customer, 11, "Customer", "Cliente inválido")
                .HasLen(ZipCode, 8, "ZipCode", "CEP inválido")
            );
        }
    }
}
