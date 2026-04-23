using Flunt.Notifications;
using Store.Domain.Commands;
using Store.Domain.Commands.Interfaces;
using Store.Domain.Handlers.Interfaces;
using Store.Domain.Repositories.Interfaces;
using Store.Domain.Utils;

namespace Store.Domain.Handlers
{
    public class OrderHandler : Notifiable, IHandler<CreateOrderCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDeliveryFeeRepository _deliveryFeeRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderHandler(ICustomerRepository customerRepository,
            IDeliveryFeeRepository deliveryFeeRepository,
            IDiscountRepository discountRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _deliveryFeeRepository = deliveryFeeRepository;
            _discountRepository = discountRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }
        public ICommandResult Handler(CreateOrderCommand command)
        {
            command.Validate();
            if(command.Invalid)
            {
                return new GenericCommandResult(false, "Pedido inválido", command.Notifications);
            }
            var customer = _customerRepository.Get(command.Customer);
            var deliveryFee = _deliveryFeeRepository.Get(command.ZipCode);
            var discount = _discountRepository.Get(command.PromoCode);
            var order = new Entities.Order(customer, deliveryFee, discount);
            var products = _productRepository.Get(ExtractGuids.Extract(command.Items)).ToList();
            
            foreach (var item in command.Items)
            {
                var product = products.Where(x => x.Id == item.Product).FirstOrDefault();
                order.AddItem(product, item.Quantity);
            }

            AddNotifications(order.Notifications);

            if(Invalid)
                return new GenericCommandResult(false, "Pedido inválido", Notifications);

            _orderRepository.Save(order);
            return new GenericCommandResult(true, $"Pedido {order.OrderNumber} criado com sucesso", order);
        }
    }  
}
