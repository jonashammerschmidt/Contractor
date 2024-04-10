using System.Diagnostics;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Contractor.CLI.Commands._Helper;
using Contractor.CLI.Tools;
using Contractor.Core;
using Contractor.Core.MetaModell;

namespace Contractor.CLI;

public class GenerateCommand
{
    public static void HandleExecuteJob(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Der Pfad wurde nicht angegeben.");
            Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
            return;
        }

        string contractorXmlFilePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), args[1]));
        var contractorXmlFileInfo = new FileInfo(contractorXmlFilePath);
        if (!contractorXmlFileInfo.Exists)
        {
            contractorXmlFilePath =
                Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..", args[1]));
            contractorXmlFileInfo = new FileInfo(contractorXmlFilePath);

            if (!contractorXmlFileInfo.Exists)
            {
                Console.WriteLine("Die angegebene Datei konnte nicht gefunden werden.");
            }
        }

        var contractorXmlDocument = new XmlDocument();
        contractorXmlDocument.Load(File.OpenRead(contractorXmlFilePath));
        var contractorXmlReader = new XmlNodeReader(contractorXmlDocument);

        var contractorXmlSerializer = new XmlSerializer(typeof(ContractorXml));
        ContractorXml contractorXml = (ContractorXml)contractorXmlSerializer.Deserialize(contractorXmlReader);
        contractorXml.Includes ??= new(); 
        contractorXml.PurposeDtos ??= new(); 
        contractorXml.Interfaces ??= new(); 
        contractorXml.Modules ??= new();  

        if (Assembly.GetExecutingAssembly().GetName().Version
                .CompareTo(Version.Parse(contractorXml.MinContractorVersion)) < 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Es muss mindestens die Contractor Version {0} verwendet werden.",
                contractorXml.MinContractorVersion);
            Console.WriteLine("");
            Console.WriteLine("Update-Befehl: dotnet tool update --global contractor");
            Console.WriteLine("");
            Console.ResetColor();
            Environment.Exit(1);
        }
        
        try
        {
            ContractorXmlValidator.Validate(contractorXml);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine("");
            Console.ResetColor();
            Environment.Exit(1);
        }

        GenerationOptions generationOptions = ContractorXmlConverter
            .ToContractorGenerationOptions(contractorXml, contractorXmlFileInfo.Directory.FullName);

        // Includes
        if (contractorXml.Includes is not null)
        {
            foreach (var include in contractorXml.Includes.Includes)
            {
                string contractorIncludeXmlFilePath = Path.GetFullPath(Path.Combine(
                    contractorXmlFileInfo.Directory.FullName,
                    include.Src));

                var contractorIncludeXmlDocument = new XmlDocument();
                contractorIncludeXmlDocument.Load(File.OpenRead(contractorIncludeXmlFilePath));
                XmlReader contractorIncludeXmlReader = new XmlNodeReader(contractorIncludeXmlDocument);

                var contractorIncludeXmlSerializer = new XmlSerializer(typeof(ContractorIncludeXml));
                ContractorIncludeXml contractorIncludeXml =
                    (ContractorIncludeXml)contractorIncludeXmlSerializer.Deserialize(contractorIncludeXmlReader);
                contractorIncludeXml.Modules ??= new();  
                contractorIncludeXml.PurposeDtos ??= new(); 
                contractorIncludeXml.Interfaces ??= new(); 

                ContractorXmlConverter.AddToContractorGenerationOptions(generationOptions, contractorIncludeXml);
            }
        }

        // Add Links
        generationOptions.AddLinks();
        
        // Set Order
        ContractorXmlOrderSetter.SetOrder(generationOptions, contractorXmlDocument);
        if (contractorXml.Includes is not null)
        {
            foreach (var include in contractorXml.Includes.Includes)
            {
                string contractorIncludeXmlFilePath = Path.GetFullPath(Path.Combine(
                    contractorXmlFileInfo.Directory.FullName,
                    include.Src));

                var contractorIncludeXmlDocument = new XmlDocument();
                contractorIncludeXmlDocument.Load(File.OpenRead(contractorIncludeXmlFilePath));

                ContractorXmlOrderSetter.SetOrder(generationOptions, contractorIncludeXmlDocument);
            }
        }

        TagArgumentParser.AddTags(args, generationOptions);
        generationOptions.IsVerbose = ArgumentParser.HasArgument(args, "-v", "--verbose");

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        Console.WriteLine($"Started generation...");

        var contractorCoreApi = new GenerationFacade(generationOptions);
        contractorCoreApi.Generate();

        stopwatch.Stop();
        Console.WriteLine($"Finished generation after {stopwatch.ElapsedMilliseconds}ms");
        stopwatch.Reset();
        stopwatch.Start();
        Console.WriteLine($"Started saving...");

        contractorCoreApi.SaveChanges();

        stopwatch.Stop();
        Console.WriteLine($"Finished saving after {stopwatch.ElapsedMilliseconds}ms");
    }
}