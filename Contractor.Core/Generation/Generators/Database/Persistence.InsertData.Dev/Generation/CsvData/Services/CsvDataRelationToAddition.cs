using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System;

namespace Contractor.Core.Generation.Database.Persistence.InsertData.Dev
{
    public class CsvDataRelationToAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public CsvDataRelationToAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            Random random = new Random(IntHash.ComputeIntHash($"{relationSide.OtherEntity.Name}"));
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.InsertIntoLine($";{relationSide.Name}");
            stringEditor.Next();

            if (relationSide.IsCreatedByPreProcessor)
            {
                stringEditor.InsertIntoLine($";22222222-2222-2222-2222-222222222222");
                stringEditor.Next();
                stringEditor.InsertIntoLine($";22222222-2222-2222-2222-222222222222");
                stringEditor.Next();
                stringEditor.InsertIntoLine($";22222222-2222-2222-2222-222222222222");
                stringEditor.Next();
                stringEditor.InsertIntoLine($";22222222-2222-2222-2222-222222222222");
            }
            else
            {
                stringEditor.InsertIntoLine($";{TestValueGeneration.GenerateGuid(random)}");
                stringEditor.Next();
                stringEditor.InsertIntoLine($";{TestValueGeneration.GenerateGuid(random)}");
                stringEditor.Next();
                
                if (relationSide.IsOptional)
                {
                    stringEditor.InsertIntoLine($";NULL");
                    stringEditor.Next();
                    stringEditor.InsertIntoLine($";NULL");
                }
                else
                {
                    stringEditor.InsertIntoLine($";{TestValueGeneration.GenerateGuid(random)}");
                    stringEditor.Next();
                    stringEditor.InsertIntoLine($";{TestValueGeneration.GenerateGuid(random)}");
                }
            }

            return stringEditor.GetText();
        }
    }
}