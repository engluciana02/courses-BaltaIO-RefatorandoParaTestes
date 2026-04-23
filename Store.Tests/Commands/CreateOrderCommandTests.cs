using Store.Domain.Commands;

namespace Store.Tests;

[TestClass]
public class CreateOrderCommandTests
{
    [TestMethod]
    public void Dado_um_comando_invalido_o_pedido_nao_deve_ser_gerado()
    {
        var command = new CreateOrderCommand();
        command.Customer = "";
        command.ZipCode = "92500000";
        command.PromoCode = "PROMO10";
        command.Items.Add(new CreateOrderItemCommand { Product = Guid.NewGuid(), Quantity = 2});
        command.Items.Add(new CreateOrderItemCommand { Product = Guid.NewGuid(), Quantity = 1});
        command.Validate();

        Assert.AreEqual(command.Valid, false);
    }
}
