using Store.Domain.Entities;
using Store.Domain.Queries;

namespace Store.Tests;

[TestClass]
public class ProductQueriesTests
{
    private IList<Product> _products = new List<Product>();

    public ProductQueriesTests()
    {
        _products.Add(new Product("Coca-cola", 5, true));
        _products.Add(new Product("Pepsi", 4, true));
        _products.Add(new Product("Fanta", 3, true));
        _products.Add(new Product("Sprite", 2, false));
        _products.Add(new Product("Fruk", 1, false));
    }


    [TestMethod]
    [TestCategory("Queries")]
    public void Dado_a_consulta_de_produtos_ativos_deve_retornar_3()
    {
        var activeProducts = _products.AsQueryable().Where(ProductQueries.GetActiveProducts());
        Assert.AreEqual(3, activeProducts.Count());
    }
        

    [TestMethod]
    [TestCategory("Queries")]
    public void Dato_a_consulta_de_produtos_inativos_deve_retornar_2()
    {
        var inactiveProducts = _products.AsQueryable().Where(ProductQueries.GetInactiveProducts());
        Assert.AreEqual(2, inactiveProducts.Count());        
    }
}
