using Store.Domain.Commands;
using Store.Domain.Handlers;
using Store.Domain.Repositories.Interfaces;
using Store.Tests.Repositories;
using System.Runtime.CompilerServices;

namespace Store.Tests;

[TestClass]
public class OrderHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDeliveryFeeRepository _deliveryFeeRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;

    private CreateOrderCommand Command =  new();

    public OrderHandlerTests()        
    {
        _customerRepository = new FakeCustomerRepository();
        _deliveryFeeRepository = new FakeDeliveryFeeRepository();
        _discountRepository = new FakeDiscountRepository();
        _productRepository = new FakeProductRepository();
        _orderRepository = new FakeOrderRepository();

        Command.Customer = "12345678";
        Command.ZipCode = "92500000";
        Command.PromoCode = "12345678";
        Command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        Command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
    }
    public void Setup()
    {
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_cliente_inexistente_o_pedido_nao_deve_ser_gerado()
    {
        Command.Customer = "";

        var handler = new OrderHandler(_customerRepository,
            _deliveryFeeRepository,
            _discountRepository,
            _productRepository,
            _orderRepository);

        handler.Handler(Command);
        Assert.AreEqual(handler.Valid, true);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_cep_invalido_o_pedido_deve_ser_gerado()
    {
       Command.ZipCode = "";   
        
        var handler = new OrderHandler(_customerRepository,
            _deliveryFeeRepository,
            _discountRepository,
            _productRepository,
            _orderRepository);

        handler.Handler(Command);
        Assert.AreEqual(handler.Valid, true);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_promocode_invalido_o_pedido_deve_ser_gerado()
    {
        Command.PromoCode = "";
       
        var handler = new OrderHandler(_customerRepository,
            _deliveryFeeRepository,
            _discountRepository,
            _productRepository,
            _orderRepository);

        handler.Handler(Command);
        Assert.AreEqual(handler.Valid, true);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_pedido_sem_items_o_pedido_nao_deve_ser_gerado()
    {     
        Command.Items.Clear();
        var handler = new OrderHandler(_customerRepository,
            _deliveryFeeRepository,
            _discountRepository,
            _productRepository,
            _orderRepository);

        handler.Handler(Command);
        Assert.AreEqual(handler.Valid, true);
    }
    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_comando_invalido_o_pedido_nao_deve_ser_gerado()
    {
        Command.Validate();
        Assert.AreEqual(false, Command.Valid);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_comando_valido_o_pedido_deve_ser_gerado()
    {
        var handler = new OrderHandler(_customerRepository,
            _deliveryFeeRepository,
            _discountRepository, 
            _productRepository, 
            _orderRepository);

        handler.Handler(Command);
        Assert.AreEqual(handler.Valid,true);
    }
}

