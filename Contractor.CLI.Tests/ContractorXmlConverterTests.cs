using Newtonsoft.Json;

namespace Contractor.CLI.Tests
{
    [TestClass]
    public class ContractorXmlConverterTests
    {
        [TestMethod]
        public void ComplexModel_ConvertsSuccessfully()
        {
            // Arrange
            var contractorXml = new ContractorXmlBuilder()
                .AddEntity(entity => entity
                    .WithName("Customer", "Customers")
                    .WithProperty("String", "Name")
                    .WithProperty("String", "Email"))
                .AddEntity(entity => entity
                    .WithName("Order", "Orders")
                    .WithRelation1ToN("Customer", "Customer", "Orders")
                    .WithProperty("DateTime", "OrderDate"))
                .AddEntity(entity => entity
                    .WithName("Product", "Products")
                    .WithProperty("String", "Name")
                    .WithProperty("Double", "Price"))
                .AddEntity(entity => entity
                    .WithName("OrderDetail", "OrderDetails")
                    .WithRelation1ToN("Order", "Order", "OrderOrderDetails")
                    .WithRelation1ToN("Product", "Product", "ProductOrderDetails")
                    .WithProperty("Integer", "Quantity")
                    .WithIndex("OrderId, ProductId, Quantity"))
                .Build();

            contractorXml.CustomDtos.CustomDtos.Add(new CustomDtoXml()
            {
                Entity = "Order",
                Purpose = "Detail",
                Properties = new List<CustomDtoPropertyXml>()
                {
                    new() { Path = "Customer" },
                    new() { Path = "OrderOrderDetails.Product" },
                }
            });

            // Act
            var contractorOptions = ContractorXmlConverter.ToContractorGenerationOptions(contractorXml, ".");
            contractorOptions.AddLinks();

            var test = JsonConvert.SerializeObject(contractorOptions, new JsonSerializerSettings 
            { 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            
            // Assert
            Assert.AreEqual(1, contractorOptions.Modules.Count());
            Assert.AreEqual(4, contractorOptions.Modules.First().Entities.Count());
            Assert.AreEqual(2, contractorOptions.Modules.First().Entities.First(e => e.Name == "Customer").Properties.Count());
            Assert.AreEqual(1, contractorOptions.Modules.First().Entities.First(e => e.Name == "Order").Properties.Count());
            Assert.AreEqual(1, contractorOptions.Modules.First().Entities.First(e => e.Name == "Order").Relations1ToN.Count());
            Assert.AreEqual(2, contractorOptions.Modules.First().Entities.First(e => e.Name == "Product").Properties.Count());
            Assert.AreEqual(1, contractorOptions.Modules.First().Entities.First(e => e.Name == "OrderDetail").Properties.Count());
            Assert.AreEqual(2, contractorOptions.Modules.First().Entities.First(e => e.Name == "OrderDetail").Relations1ToN.Count());
            Assert.AreEqual(1, contractorOptions.CustomDtos.Count());
            Assert.AreEqual(2, contractorOptions.CustomDtos.First().Properties.Count());
            Assert.AreEqual(1, contractorOptions.CustomDtos.First().Properties.First().PathItems.Count());
            Assert.AreEqual(2, contractorOptions.CustomDtos.First().Properties.Last().PathItems.Count());
        }
    }
}
