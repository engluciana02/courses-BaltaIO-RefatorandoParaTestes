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
    public void Dado_um_novo_pedido_valido_ele_deve_gerar_um_numero_com_8_caracteres()
    {
       var order = new Order(_customer, 0, null);
       Assert.AreEqual(8, order.OrderNumber.Length);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_novo_pedido_seu_status_dever_ser_aguardando_pagamento()
    {
        var order = new Order(_customer, 10, _discount);
        Assert.AreEqual(EnumOrderStatus.WaitingPayment, order.Status);        
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_pagamento_do_pedido_seu_status_deve_ser_aguardando_entrega()
    {
        var order = new Order(_customer, 1, _discount);
        order.AddItem(_product, 2);
        order.Total();
        order.Pay(11);
        Assert.AreEqual(EnumOrderStatus.WaitingDelivery, order.Status);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_pedido_cancelado_seu_status_deve_ser_cancelado()
    {
        var order = new Order(_customer, 10, _discount);
        order.Cancel();
        Assert.AreEqual(EnumOrderStatus.Canceled, order.Status);       
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_novo_item_sem_produto_o_mesmo_nao_deve_ser_adicionado()
    {
        var order = new Order(_customer, 10, _discount);
        order.AddItem(null, 5);
        Assert.AreEqual(0, order.Items.Count);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_novo_item_com_quantidade_zero_o_mesmo_nao_deve_ser_adicionado()
    {
        var order = new Order(_customer, 10, _discount);
        order.AddItem(_product, 0);
        Assert.AreEqual(0, order.Items.Count);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_pedido_valido_o_seu_total_deve_ser_50()
    {
        var order = new Order(_customer, 10, _discount);
        order.AddItem(_product, 5);
        var total = order.Total();
        Assert.AreEqual(50, total);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_desconto_expirado_o_valor_do_pedido_deve_ser_60()
    {
        var discount = new Discount(10, DateTime.Now.AddDays(-5));
        var order = new Order(_customer, 10, discount);
        order.AddItem(_product, 5);
        var total = order.Total();
        Assert.AreEqual(60, total);
    }
    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_desconto_invalido_o_valor_do_pedido_deve_ser_60()
    {
        var discount = new Discount(10, DateTime.Now.AddDays(-30));
        var order = new Order(_customer, 10, discount);
        order.AddItem(_product, 5);
        var total = order.Total();
        Assert.AreEqual(60, total);
    }

}
