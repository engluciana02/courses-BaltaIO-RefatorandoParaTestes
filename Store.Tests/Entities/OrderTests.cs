using Store.Domain.Entities;
using Store.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Store.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class OrderTests
{
    private readonly Customer _customer = new ("Cassiano Baltazar", "baltazer@email.com");
    private readonly Product _product = new ("Mouse Gamer", 10, true);
    private readonly Discount _discount = new (10, DateTime.Now.AddDays(5));
    
    [TestMethod]
    [TestCategory("Domain")]
    public void Given_a_new_valid_order_it_must_generate_a_number_with_8_characters()
    {
       var order = new Order(_customer, 0, null);
       Assert.AreEqual(8, order.OrderNumber.Length);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Given_a_new_order_its_status_must_be_waiting_for_payment()
    {
        var order = new Order(_customer, 10, _discount);
        Assert.AreEqual(EnumOrderStatus.WaitingPayment, order.Status);        
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Given_a_payment_for_the_order_its_status_must_be_waiting_for_delivery()
    {
        var order = new Order(_customer, 1, _discount);
        order.AddItem(_product, 2);
        order.Total();
        order.Pay(11);
        Assert.AreEqual(EnumOrderStatus.WaitingDelivery, order.Status);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Given_a_canceled_order_its_status_must_be_canceled()
    {
        var order = new Order(_customer, 10, _discount);
        order.Cancel();
        Assert.AreEqual(EnumOrderStatus.Canceled, order.Status);       
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Given_a_new_item_without_a_product_it_must_not_be_added()
    {
        var order = new Order(_customer, 10, _discount);
        order.AddItem(null, 5);
        Assert.AreEqual(0, order.Items.Count);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Given_a_new_item_with_quantity_zero_it_must_not_be_added()
    {
        var order = new Order(_customer, 10, _discount);
        order.AddItem(_product, 0);
        Assert.AreEqual(0, order.Items.Count);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Given_a_valid_order_its_total_must_be_50()
    {
        var order = new Order(_customer, 10, _discount);
        order.AddItem(_product, 5);
        var total = order.Total();
        Assert.AreEqual(50, total);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Given_an_expired_discount_the_order_value_must_be_60()
    {
        var discount = new Discount(10, DateTime.Now.AddDays(-5));
        var order = new Order(_customer, 10, discount);
        order.AddItem(_product, 5);
        var total = order.Total();
        Assert.AreEqual(60, total);
    }
    [TestMethod]
    [TestCategory("Domain")]
    public void Given_an_invalid_discount_the_order_value_must_be_60()
    {
        var order = new Order(_customer, 10, null);
        order.AddItem(_product, 5);
        var total = order.Total();
        Assert.AreEqual(60, total);
    }
    
    [TestMethod]
    [TestCategory("Domain")]
    public  void Given_a_10_discount_the_order_value_must_be_50()
    {
        var order = new Order(_customer, 10, _discount);
        order.AddItem(_product, 5);
        var total = order.Total();
        Assert.AreEqual(50, total);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Given_a_delivery_fee_of_10_the_order_value_must_be_50()
    {
        var order = new Order(_customer, 10, _discount);
        order.AddItem(_product, 5);
        var total = order.Total();
        Assert.AreEqual(50, total);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Given_a_order_without_client_it_must_be_invalid()
    {
        var order = new Order(null, 10, _discount);
        Assert.IsTrue(order.Invalid);
    }     
}
