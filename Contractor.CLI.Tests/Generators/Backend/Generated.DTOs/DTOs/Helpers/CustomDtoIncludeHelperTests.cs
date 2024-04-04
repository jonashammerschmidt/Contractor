using Contractor.CLI.Tests.Helpers;
using Contractor.Core.Generation.Backend.Generated.DTOs;

namespace Contractor.CLI.Tests.Generators.Backend.Generated.DTOs.DTOs.Helpers
{
    [TestClass]
    public class CustomDtoIncludeHelperTests
    {
        [TestMethod]
        public void GetIncludeString_GeneratedCorrectly()
        {
            // Arrange
            var builder = new CustomDtoBuilder();
            var customDto = builder
                .SetEntity("EntityA")
                .WithPurpose("Test")
                .AddPropertyPath("EntityB.EntityC.EntityD")
                .AddPropertyPath("EntityB.EntityC.EntityE")
                .Build();

            // Act
            var result = CustomDtoIncludeHelper.GetIncludeString(customDto);

            // Assert
            Assert.AreEqual(
                ".Include(efEntityA => efEntityA.EntityB)\n" +
                "    .ThenInclude(efEntityB => efEntityB.EntityC)\n" +
                "        .ThenInclude(efEntityC => efEntityC.EntityD)\n" +
                ".Include(efEntityA => efEntityA.EntityB)\n" +
                "    .ThenInclude(efEntityB => efEntityB.EntityC)\n" +
                "        .ThenInclude(efEntityC => efEntityC.EntityE)\n",
                result);
        }
    }
}