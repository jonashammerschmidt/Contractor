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
            var contractorXml = new ContractorXml
            {
                Modules = new ModulesXml()
                {
                    Modules = new List<ModuleXml>
                    {
                        new ModuleXml
                        {
                            Entities = new List<EntityXml>
                            {
                                new EntityXml { Name = "Entity1", ScopeEntityName = "NonexistentEntity" }
                            }
                        }
                    }
                }
            };

            // Act
            ContractorXmlValidator.Validate(contractorXml);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateRelations_WithNonexistentEntityInRelation_ThrowsFormatException()
        {
            // Arrange
            var contractorXml = new ContractorXml
            {
                Modules = new ModulesXml()
                {
                    Modules = new List<ModuleXml>
                    {
                        new ModuleXml
                        {
                            Entities = new List<EntityXml>
                            {
                                new EntityXml
                                {
                                    Name = "Entity1",
                                    Relation1ToN = new List<Relation1ToNXml>
                                    {
                                        new Relation1ToNXml { EntityNameFrom = "NonexistentEntity" }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            ContractorXmlValidator.Validate(contractorXml);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateProperties_WithDuplicatePropertyNames_ThrowsFormatException()
        {
            // Arrange
            var contractorXml = new ContractorXml
            {
                Modules = new ModulesXml()
                {
                    Modules = new List<ModuleXml>
                    {
                        new ModuleXml
                        {
                            Entities = new List<EntityXml>
                            {
                                new EntityXml
                                {
                                    Name = "Entity1",
                                    Properties = new List<PropertyXml>
                                    {
                                        new PropertyXml { Name = "Property1" },
                                        new PropertyXml { Name = "Property1" }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            ContractorXmlValidator.Validate(contractorXml);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateIndices_WithNonexistentPropertyNameInIndex_ThrowsFormatException()
        {
            // Arrange
            var contractorXml = new ContractorXml
            {
                Modules = new ModulesXml()
                {
                    Modules = new List<ModuleXml>
                    {
                        new ModuleXml
                        {
                            Entities = new List<EntityXml>
                            {
                                new EntityXml
                                {
                                    Name = "Entity1",
                                    Indices = new List<IndexXml>
                                    {
                                        new IndexXml { PropertyNames = "NonexistentProperty" }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            ContractorXmlValidator.Validate(contractorXml);

            // Assert is handled by ExpectedException
        }
    }
}