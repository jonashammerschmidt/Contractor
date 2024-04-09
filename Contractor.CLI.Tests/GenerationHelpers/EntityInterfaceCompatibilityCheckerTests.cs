using Contractor.CLI.Tests.Builder.MetaModell;
using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;

namespace Contractor.CLI.Tests.GenerationHelpers;

[TestClass]
public class ModuleEntityInterfaceCompatibilityTests
{

    private EntityInterfaceCompatibilityChecker checker = new EntityInterfaceCompatibilityChecker();

    private Entity employeeEntity;
    private Entity deskEntity;
    private Entity departmentEntity;
    private Module module;

    public ModuleEntityInterfaceCompatibilityTests()
    {
        // Erstellen der Department Entity
        this.departmentEntity = new EntityBuilder()
            .WithName("Department", "Departments")
            .AddProperty(new PropertyBuilder().WithName("Name").Build())
            .AddRelation1To1(new Relation1To1Builder().WithEntity("Employee").WithPropertyNameFrom("EmployeeOfTheMonth").WithPropertyNameTo("DepartmentOfTheMonth").Build())
            .AddRelation1To1(new Relation1To1Builder().WithEntity("Employee").WithPropertyNameFrom("BaddestEmployeeOfTheMonth").WithPropertyNameTo("BaddestDepartmentOfTheMonth").Build())
            .Build();

        // Erstellen der Employee Entity
        this.employeeEntity = new EntityBuilder()
            .WithName("Employee", "Employees")
            .AddProperty(new PropertyBuilder().WithName("Name").Build())
            .AddRelation1ToN(new Relation1ToNBuilder().WithEntity("Department").Build())
            .AddRelation1To1(new Relation1To1Builder().WithEntity("Desk").Build())
            .Build();

        // Erstellen der Employee Entity
        this.deskEntity = new EntityBuilder()
            .WithName("Desk", "Desks")
            .AddProperty(new PropertyBuilder().WithName("Name").Build())
            .Build();

        // Erstellen des Moduls, das die Entities enthält
        this.module = new ModuleBuilder()
            .WithName("HumanResources")
            .AddEntity(departmentEntity)
            .AddEntity(employeeEntity)
            .AddEntity(deskEntity)
            .Build();

        var generationOptionsBuilder = new GenerationOptionsBuilder()
            .AddModule(module)
            .Build();
        
        module.AddLinks(generationOptionsBuilder);
        module.AddLinksForChildren();
    }
    
    [TestMethod]
    public void TestModuleWithEntitiesPropertiesAndRelations_None_PropertyUnbekannt()
    {
        // Definition des Interfaces
        var interfaceItem = new Interface()
        {
            Name = "Test",
            Properties = new List<InterfaceProperty>()
            {
                new() { Name = "Id" },
                new() { Name = "Unbekannt" },
            },
            Relations = new List<InterfaceRelation>()
            {
                new() { TargetEntityName = "Department" },
            }
        };

        // Überprüfung der Kompatibilität
        var result = checker.IsInterfaceCompatible(employeeEntity, interfaceItem);

        // Assertion
        Assert.AreEqual(EntityInterfaceCompatibility.None, result);
    }

    [TestMethod]
    public void TestModuleWithEntitiesPropertiesAndRelations_None_RelationUnbekannt()
    {
        // Definition des Interfaces
        var interfaceItem = new Interface()
        {
            Name = "Test",
            Properties = new List<InterfaceProperty>()
            {
                new() { Name = "Id" },
                new() { Name = "Name" },
            },
            Relations = new List<InterfaceRelation>()
            {
                new() { TargetEntityName = "Unbekannt" },
            }
        };

        // Überprüfung der Kompatibilität
        var result = checker.IsInterfaceCompatible(employeeEntity, interfaceItem);

        // Assertion
        Assert.AreEqual(EntityInterfaceCompatibility.None, result);
    }

    [TestMethod]
    public void TestModuleWithEntitiesPropertiesAndRelations_DtoData()
    {
        // Definition des Interfaces
        var interfaceItem = new Interface()
        {
            Name = "Test",
            Properties = new List<InterfaceProperty>()
            {
                new() { Name = "Name" },
            },
        };

        // Überprüfung der Kompatibilität
        var result = checker.IsInterfaceCompatible(employeeEntity, interfaceItem);

        // Assertion
        Assert.AreEqual(EntityInterfaceCompatibility.DtoData, result);
    }

    [TestMethod]
    public void TestModuleWithEntitiesPropertiesAndRelations_DtoData_1To1_PerId()
    {
        // Definition des Interfaces
        var interfaceItem = new Interface()
        {
            Name = "Test",
            Properties = new List<InterfaceProperty>()
            {
                new() { Name = "Name" },
                new() { Name = "DeskId" },
            },
        };

        // Überprüfung der Kompatibilität
        var result = checker.IsInterfaceCompatible(employeeEntity, interfaceItem);

        // Assertion
        Assert.AreEqual(EntityInterfaceCompatibility.DtoData, result);
    }

    [TestMethod]
    public void TestModuleWithEntitiesPropertiesAndRelations_DtoData_1ToN_PerId()
    {
        // Definition des Interfaces
        var interfaceItem = new Interface()
        {
            Name = "Test",
            Properties = new List<InterfaceProperty>()
            {
                new() { Name = "Name" },
                new() { Name = "DepartmentId" },
            },
        };

        // Überprüfung der Kompatibilität
        var result = checker.IsInterfaceCompatible(employeeEntity, interfaceItem);

        // Assertion
        Assert.AreEqual(EntityInterfaceCompatibility.DtoData, result);
    }

    [TestMethod]
    public void TestModuleWithEntitiesPropertiesAndRelations_Dto()
    {
        // Definition des Interfaces
        var interfaceItem = new Interface()
        {
            Name = "Test",
            Properties = new List<InterfaceProperty>()
            {
                new() { Name = "Id" },
                new() { Name = "Unbekannt" },
            }
        };

        // Überprüfung der Kompatibilität
        var result = checker.IsInterfaceCompatible(employeeEntity, interfaceItem);

        // Assertion
        Assert.AreEqual(EntityInterfaceCompatibility.None, result);
    }

    [TestMethod]
    public void TestModuleWithEntitiesPropertiesAndRelations_Expanded_1ToN_From()
    {
        // Definition des Interfaces
        var interfaceItem = new Interface()
        {
            Name = "Test",
            Properties = new List<InterfaceProperty>()
            {
                new() { Name = "Id" },
                new() { Name = "Name" },
            },
            Relations = new List<InterfaceRelation>()
            {
                new() { TargetEntityName = "Department" },
            }
        };

        // Überprüfung der Kompatibilität
        var result = checker.IsInterfaceCompatible(employeeEntity, interfaceItem);

        // Assertion
        Assert.AreEqual(EntityInterfaceCompatibility.DtoExpanded, result);
    }

    [TestMethod]
    public void TestModuleWithEntitiesPropertiesAndRelations_Expanded_1To1_From()
    {
        // Definition des Interfaces
        var interfaceItem = new Interface()
        {
            Name = "Test",
            Properties = new List<InterfaceProperty>()
            {
                new() { Name = "Id" },
                new() { Name = "Name" },
            },
            Relations = new List<InterfaceRelation>()
            {
                new() { TargetEntityName = "Department" },
            }
        };

        // Überprüfung der Kompatibilität
        var result = checker.IsInterfaceCompatible(employeeEntity, interfaceItem);

        // Assertion
        Assert.AreEqual(EntityInterfaceCompatibility.DtoExpanded, result);
    }

    [TestMethod]
    public void TestModuleWithEntitiesPropertiesAndRelations_Expanded_1To1_ToPerEntityName()
    {
        // Definition des Interfaces
        var interfaceItem = new Interface()
        {
            Name = "Test",
            Properties = new List<InterfaceProperty>()
            {
                new() { Name = "Id" },
                new() { Name = "Name" },
            },
            Relations = new List<InterfaceRelation>()
            {
                new() { TargetEntityName = "Desk" },
            }
        };

        // Überprüfung der Kompatibilität
        var result = checker.IsInterfaceCompatible(employeeEntity, interfaceItem);

        // Assertion
        Assert.AreEqual(EntityInterfaceCompatibility.DtoExpanded, result);
    }

    [TestMethod]
    public void TestModuleWithEntitiesPropertiesAndRelations_Expanded_1To1_ToPerEntityNameReverse()
    {
        // Definition des Interfaces
        var interfaceItem = new Interface()
        {
            Name = "Test",
            Properties = new List<InterfaceProperty>()
            {
                new() { Name = "Id" },
                new() { Name = "Name" },
            },
            Relations = new List<InterfaceRelation>()
            {
                new() { TargetEntityName = "Employee" },
            }
        };

        // Überprüfung der Kompatibilität
        var result = checker.IsInterfaceCompatible(deskEntity, interfaceItem);

        // Assertion
        Assert.AreEqual(EntityInterfaceCompatibility.DtoExpanded, result);
    }

    [TestMethod]
    public void TestModuleWithEntitiesPropertiesAndRelations_Expanded_1To1_ToPerPropertyName()
    {
        // Definition des Interfaces
        var interfaceItem = new Interface()
        {
            Name = "Test",
            Properties = new List<InterfaceProperty>()
            {
                new() { Name = "Id" },
                new() { Name = "Name" },
            },
            Relations = new List<InterfaceRelation>()
            {
                new() { TargetEntityName = "Employee", PropertyName = "EmployeeOfTheMonth"},
            }
        };

        // Überprüfung der Kompatibilität
        var result = checker.IsInterfaceCompatible(departmentEntity, interfaceItem);

        // Assertion
        Assert.AreEqual(EntityInterfaceCompatibility.DtoExpanded, result);
    }

    [TestMethod]
    public void TestModuleWithEntitiesPropertiesAndRelations_Expanded_1To1_To_NotFound()
    {
        // Definition des Interfaces
        var interfaceItem = new Interface()
        {
            Name = "Test",
            Properties = new List<InterfaceProperty>()
            {
                new() { Name = "Id" },
                new() { Name = "Name" },
            },
            Relations = new List<InterfaceRelation>()
            {
                new() { TargetEntityName = "Employee" },
            }
        };

        // Überprüfung der Kompatibilität
        var result = checker.IsInterfaceCompatible(departmentEntity, interfaceItem);

        // Assertion
        Assert.AreEqual(EntityInterfaceCompatibility.None, result);
    }
}
