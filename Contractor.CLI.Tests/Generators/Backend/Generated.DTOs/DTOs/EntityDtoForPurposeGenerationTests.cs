using Contractor.CLI.Tests.Helpers;
using Contractor.Core.Generation.Backend.Generated.DTOs;
using Contractor.Core.MetaModell;

namespace Contractor.CLI.Tests.Generators.Backend.Generated.DTOs.DTOs
{
    [TestClass]
    public class EntityDtoForPurposeGenerationTests
    {
        [TestMethod]
        [DataRow("EntityB",                  "EntityC",                 new string[] { })]
        [DataRow("EntityB.EntityC",          "EntityB.EntityC.EntityD", new string[] { })]
        [DataRow("EntityB.EntityC.EntityD",  "EntityB.EntityC.EntityD", new string[] { })]
        [DataRow("EntityB.EntityC.EntityD",  "EntityB.EntityC.EntityE", new string[] { })]
        [DataRow("EntityB.EntityC",          "EntityB.EntityD",         new string[] { })]
        [DataRow("EntityB.EntityC",          "EntityD.EntityC",         new string[] { })]
        [DataRow("EntityB.EntityC",          "EntityD.EntityC.EntityF", new string[] { })]
        [DataRow("EntityB.EntityC.EntityF",  "EntityD.EntityC.EntityF", new string[] { })]
        [DataRow("EntityB.EntityC.EntityF.EntityC.EntityE",  "EntityA", new string[] { "EntityC" })]
        [DataRow("EntityB.EntityC.EntityE",  "EntityD.EntityC.EntityF", new string[] { "EntityC" })]
        public void DetermineEntitiesWithVia_IdentifiesEntitiesCorrectly(string propertyPath1, string propertyPath2, string[] expected)
        {
            // Arrange
            var builder = new PurposeDtoBuilder();
            var purposeDto = builder
                .SetEntity("EntityA")
                .WithPurpose("Test")
                .AddPropertyPath(propertyPath1)
                .AddPropertyPath(propertyPath2)
                .Build();

            var entityDtoForPurposeGeneration = new EntityDtoForPurposeGeneration(null, null, null, null, null, null, null);

            // Act
            var result = entityDtoForPurposeGeneration.DetermineEntitiesWithVia(purposeDto);

            // Assert
            var expectedEntitiesWithVia = expected.Select(e => new Entity { Name = e }).ToHashSet();
            Assert.AreEqual(expectedEntitiesWithVia.Count, result.Count, "Mismatch in expected entities count with VIA paths.");
            foreach (var entity in expectedEntitiesWithVia)
            {
                Assert.IsTrue(result.Any(e => e.Name == entity.Name), $"Expected entity {entity.Name} is missing.");
            }
        }
    }
}
