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

            contractorXml.PurposeDtos.PurposeDtos.Add(new PurposeDtoXml()
            {
                Entity = "Order",
                Purpose = "Detail",
                Properties = new List<PurposeDtoPropertyXml>()
                {
                    new() { Path = "Customer" },
                    new() { Path = "OrderOrderDetails.Product" },
                }
            });

            contractorXml.Interfaces.Interfaces.Add(new InterfaceXml()
            {
                Name = "Nameable",
                Properties = new List<InterfacePropertyXml>()
                {
                    new() { Name = "Name" },
                },
                Relations = new List<InterfaceRelationXml>()
                {
                    new() { EntityNameFrom = "Order" },
                }
            });

            // Act
            var contractorOptions = ContractorXmlConverter.ToContractorGenerationOptions(contractorXml, ".");
            contractorOptions.AddLinks();

            // Assert
            Assert.AreEqual(1, contractorOptions.Modules.Count());
            Assert.AreEqual(4, contractorOptions.Modules.First().Entities.Count());
            Assert.AreEqual(2, contractorOptions.Modules.First().Entities.First(e => e.Name == "Customer").Properties.Count());
            Assert.AreEqual(1, contractorOptions.Modules.First().Entities.First(e => e.Name == "Order").Properties.Count());
            Assert.AreEqual(1, contractorOptions.Modules.First().Entities.First(e => e.Name == "Order").Relations1ToN.Count());
            Assert.AreEqual(2, contractorOptions.Modules.First().Entities.First(e => e.Name == "Product").Properties.Count());
            Assert.AreEqual(1, contractorOptions.Modules.First().Entities.First(e => e.Name == "OrderDetail").Properties.Count());
            Assert.AreEqual(2, contractorOptions.Modules.First().Entities.First(e => e.Name == "OrderDetail").Relations1ToN.Count());
            Assert.AreEqual(1, contractorOptions.PurposeDtos.Count());
            Assert.AreEqual(2, contractorOptions.PurposeDtos.First().Properties.Count());
            Assert.AreEqual(1, contractorOptions.PurposeDtos.First().Properties.First().PathItems.Count());
            Assert.AreEqual(2, contractorOptions.PurposeDtos.First().Properties.Last().PathItems.Count());
            Assert.AreEqual(1, contractorOptions.Interfaces.Count());
            Assert.AreEqual(1, contractorOptions.Interfaces.First().Properties.Count());
            Assert.AreEqual(1, contractorOptions.Interfaces.First().Relations.Count());
        }
    }
}
