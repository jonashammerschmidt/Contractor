using System.Xml;
using System.Xml.Serialization;
using Contractor.Core.MetaModell;

namespace Contractor.CLI.Tests
{
    [TestClass]
    public class XmlIntegrationTest
    {
        [TestMethod]
        public void Test_ToContractorGenerationOptions_ShouldConvertCorrectly()
        {
            // Arrange
            var xmlString = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<Contractor minContractorVersion=""2.8.3"">
  <Paths>
    <BackendDestinationFolder>../Backend</BackendDestinationFolder>
    <BackendGeneratedDestinationFolder>..\Backend.Generated</BackendGeneratedDestinationFolder>
    <DbDestinationFolder>..\Database</DbDestinationFolder>
    <FrontendDestinationFolder>../Frontend</FrontendDestinationFolder>
    <ProjectName>MyProject</ProjectName>
    <GeneratedProjectName>MyProject.Generated</GeneratedProjectName>
    <DbProjectName>MyProject.Database</DbProjectName>
    <DbContextName>MyProjectDbContext</DbContextName>
  </Paths>
  <Replacements>
    <Replacement pattern=""old"" replaceWith=""new""/>
  </Replacements>
  <Modules>
    <Module name=""SampleModule"">
      <Entity name=""SampleEntity"" namePlural=""SampleEntities"">
        <Property name=""BezeichnungKurz"" type=""String:256""/>
        <Property name=""BezeichnungLang"" type=""String:5000"" minLength=""0""/>
        <Relation1ToN entityNameFrom=""OtherEntity"" propertyNameFrom=""OtherProperty""/>
        <Index property=""Name"" unique=""true""/>
        <Check name=""CheckName"" query=""SomeQuery""/>
      </Entity>
    </Module>
  </Modules>
  <PurposeDtos>
    <PurposeDto entity=""SampleEntity"" purpose=""SomePurpose"">
      <Property path=""Name""/>
    </PurposeDto>
  </PurposeDtos>
  <Interfaces>
    <Interface name=""SampleInterface"">
      <Property name=""Name""/>
    </Interface>
  </Interfaces>
</Contractor>";

            var contractorXmlSerializer = new XmlSerializer(typeof(ContractorXml));

            // Act
            ContractorXml contractorXml;
            using var stringReader = new StringReader(xmlString);
            using var xmlReader = XmlReader.Create(stringReader);
            contractorXml = (ContractorXml)contractorXmlSerializer.Deserialize(xmlReader)!;

            contractorXml.Includes ??= new();
            contractorXml.PurposeDtos ??= new();
            contractorXml.Interfaces ??= new();
            contractorXml.Modules ??= new();

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            GenerationOptions generationOptions = ContractorXmlConverter
                .ToContractorGenerationOptions(contractorXml, baseDirectory);

            // Assert
            Assert.IsNotNull(generationOptions);

            // Assert Modules
            Assert.AreEqual(1, generationOptions.Modules.Count);
            var module = generationOptions.Modules[0];
            Assert.AreEqual("SampleModule", module.Name);
            Assert.AreEqual(1, module.Entities.Count);
            var entity = module.Entities[0];
            Assert.AreEqual("SampleEntity", entity.Name);
            Assert.AreEqual(2, entity.Properties.Count);
            Assert.AreEqual("BezeichnungKurz", entity.Properties[0].Name);
            Assert.AreEqual("String", entity.Properties[0].Type);
            Assert.AreEqual("256", entity.Properties[0].TypeExtra);
            Assert.AreEqual(1, entity.Properties[0].MinLength);
            Assert.AreEqual("BezeichnungLang", entity.Properties[1].Name);
            Assert.AreEqual("String", entity.Properties[1].Type);
            Assert.AreEqual("5000", entity.Properties[1].TypeExtra);
            Assert.AreEqual(0, entity.Properties[1].MinLength);

            // Assert Relations
            Assert.AreEqual(1, entity.Relations1ToN.Count);
            Assert.AreEqual("OtherProperty", entity.Relations1ToN[0].PropertyNameInSource);

            // Assert Indexes
            Assert.AreEqual(1, entity.Indices.Count);
            Assert.IsTrue(entity.Indices[0].IsUnique);

            // Assert Checks
            Assert.AreEqual(1, entity.Checks.Count);
            Assert.AreEqual("CheckName", entity.Checks[0].Name);
            Assert.AreEqual("SomeQuery", entity.Checks[0].Query);

            // Assert PurposeDtos
            Assert.AreEqual(1, generationOptions.PurposeDtos.Count);
            var purposeDto = generationOptions.PurposeDtos[0];
            Assert.AreEqual("SampleEntity", purposeDto.EntityName);
            Assert.AreEqual("SomePurpose", purposeDto.Purpose);
            Assert.AreEqual(1, purposeDto.Properties.Count);
            Assert.AreEqual("Name", purposeDto.Properties[0].Path);

            // Assert Interfaces
            Assert.AreEqual(1, generationOptions.Interfaces.Count);
            var interfaceItem = generationOptions.Interfaces[0];
            Assert.AreEqual("SampleInterface", interfaceItem.Name);
            Assert.AreEqual(1, interfaceItem.Properties.Count);
            Assert.AreEqual("Name", interfaceItem.Properties[0].Name);
        }
    }
}