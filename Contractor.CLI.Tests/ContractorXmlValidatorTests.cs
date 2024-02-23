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
                    .WithName("Entity1")
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
                    .WithName("Entity1")
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
                    .WithName("Entity1")
                    .WithProperty("Property1")
                    .WithProperty("Property1")) // Duplicate
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
                    .WithName("Entity1")
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
                    .WithName("Entity1")
                    .WithRelation1ToN("Entity2", "Entity2s", "Entity1s"))
                .AddEntity(entity => entity
                    .WithName("Entity2"))
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
                    .WithName("Entity1")
                    .WithRelation1ToN("Entity2", "Entity2", "Entity1s")
                    .WithRelation1ToN("Entity2", "Entity2", "Entity1s")) // Duplicate
                .AddEntity(entity => entity
                    .WithName("Entity2"))
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
                    .WithName("Entity1")
                    .WithRelation1ToN("Entities2", "Entity2", "Entity1s")) // Pluralized might be intended as a mistake
                .AddEntity(entity => entity
                    .WithName("Entity2"))
                .Build();

            // Act & Assert
            ContractorXmlValidator.Validate(contractorXml);
        }
        
        [TestMethod]
        public void ValidateECommerceModel_WithComplexRelations_DoesNotThrowException()
        {
            // Arrange
            var contractorXml = new ContractorXmlBuilder()
                .AddEntity(entity => entity
                    .WithName("Customer")
                    .WithProperty("CustomerId")
                    .WithProperty("Name")
                    .WithProperty("Email"))
                .AddEntity(entity => entity
                    .WithName("Order")
                    .WithProperty("OrderId")
                    .WithRelation1ToN("Customer", "Customer", "Orders") // Assuming Customer has many Orders
                    .WithProperty("OrderDate"))
                .AddEntity(entity => entity
                    .WithName("Product")
                    .WithProperty("ProductId")
                    .WithProperty("Name")
                    .WithProperty("Price"))
                .AddEntity(entity => entity
                    .WithName("OrderDetail")
                    .WithProperty("OrderDetailId")
                    .WithRelation1ToN("Order", "Order", "OrderOrderDetails") // Assuming Order has many OrderDetails
                    .WithRelation1ToN("Product", "Product", "ProductOrderDetails") // Assuming Product can be in many OrderDetails
                    .WithProperty("Quantity")
                    .WithIndex("OrderId, ProductId, Quantity")) // Composite index for OrderId and ProductId
                .Build();

            // Act & Assert
            ContractorXmlValidator.Validate(contractorXml);
        }
    }
}
