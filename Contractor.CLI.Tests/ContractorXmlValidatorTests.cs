namespace Contractor.CLI.Tests
{
    [TestClass]
    public class ContractorXmlValidatorTests
    {
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateEntities_WithNonexistentScopeEntity_ThrowsFormatException()
        {
            // Arrange
            var contractorXml = new ContractorXmlBuilder()
                .AddEntity(entity => entity
                    .WithName("Entity1", "Entity1s")
                    .WithScopeEntityName("NonexistentEntity"))
                .Build();

            // Act & Assert
            ContractorXmlValidator.Validate(contractorXml);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateRelations_WithNonexistentEntityInRelation_ThrowsFormatException()
        {
            // Arrange
            var contractorXml = new ContractorXmlBuilder()
                .AddEntity(entity => entity
                    .WithName("Entity1", "Entity1s")
                    .WithRelation1ToN("NonexistentEntity", "NonexistentProperty", "Entity1s"))
                .Build();

            // Act & Assert
            ContractorXmlValidator.Validate(contractorXml);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateProperties_WithDuplicatePropertyNames_ThrowsFormatException()
        {
            // Arrange
            var contractorXml = new ContractorXmlBuilder()
                .AddEntity(entity => entity
                    .WithName("Entity1", "Entity1s")
                    .WithProperty("String", "Property1")
                    .WithProperty("String", "Property1")) // Duplicate
                .Build();

            // Act & Assert
            ContractorXmlValidator.Validate(contractorXml);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateIndices_WithNonexistentPropertyNameInIndex_ThrowsFormatException()
        {
            // Arrange
            var contractorXml = new ContractorXmlBuilder()
                .AddEntity(entity => entity
                    .WithName("Entity1", "Entity1s")
                    .WithIndex("NonexistentProperty"))
                .Build();

            // Act & Assert
            ContractorXmlValidator.Validate(contractorXml);
        }

        [TestMethod]
        public void ValidateUniqueRelations_WithUniqueRelations_DoesNotThrowException()
        {
            // Arrange
            var contractorXml = new ContractorXmlBuilder()
                .AddEntity(entity => entity
                    .WithName("Entity1", "Entity1s")
                    .WithRelation1ToN("Entity2", "Entity2s", "Entity1s"))
                .AddEntity(entity => entity
                    .WithName("Entity2", "Entity2s"))
                .Build();

            // Act & Assert
            ContractorXmlValidator.Validate(contractorXml);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateUniqueRelations_WithDuplicateRelations_ThrowsFormatException()
        {
            // Arrange
            var contractorXml = new ContractorXmlBuilder()
                .AddEntity(entity => entity
                    .WithName("Entity1", "Entity1s")
                    .WithRelation1ToN("Entity2", "Entity2", "Entity1s")
                    .WithRelation1ToN("Entity2", "Entity2", "Entity1s")) // Duplicate
                .AddEntity(entity => entity
                    .WithName("Entity2", "Entity2s"))
                .Build();

            // Act & Assert
            ContractorXmlValidator.Validate(contractorXml);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateUniqueRelations_WithPluralizedDuplicateRelations_ThrowsFormatException()
        {
            // Arrange
            var contractorXml = new ContractorXmlBuilder()
                .AddEntity(entity => entity
                    .WithName("Entity1", "Entity1s")
                    .WithRelation1ToN("Entities2", "Entity2", "Entity1s")) // Pluralized might be intended as a mistake
                .AddEntity(entity => entity
                    .WithName("Entity2", "Entity2s"))
                .Build();

            // Act & Assert
            ContractorXmlValidator.Validate(contractorXml);
        }
        
        [TestMethod]
        public void ValidateComplexModel_DoesNotThrowException()
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

            // Act & Assert
            ContractorXmlValidator.Validate(contractorXml);
        }
    }
}
