using Contractor.Core.MetaModell;

namespace Contractor.CLI.Tests.Helpers
{
    public class PurposeDtoBuilder
    {
        private PurposeDto purposeDto = new PurposeDto();
        private Dictionary<string, Entity> entities = new();

        public PurposeDtoBuilder SetEntity(string name)
        {
            var entity = GetOrCreateEntity(name);
            purposeDto.Entity = entity;
            purposeDto.EntityName = name;
            return this;
        }

        public PurposeDtoBuilder WithPurpose(string purpose)
        {
            purposeDto.Purpose = purpose;
            return this;
        }

        public PurposeDtoBuilder AddPropertyPath(string fullPath)
        {
            var pathSteps = fullPath.Split('.');
            var pathItems = new List<PurposeDtoPathItem>();
            Entity lastEntity = purposeDto.Entity;

            foreach (var step in pathSteps)
            {
                var nextEntity = GetOrCreateEntity(step);
                pathItems.Add(new PurposeDtoPathItem
                {
                    Entity = lastEntity,
                    OtherEntity = nextEntity,
                    PropertyName = step
                });
                lastEntity = nextEntity;
            }

            purposeDto.Properties ??= new List<PurposeDtoProperty>();
            purposeDto.Properties.Add(new PurposeDtoProperty
            {
                Path = fullPath,
                PathItems = pathItems
            });

            return this;
        }

        private Entity GetOrCreateEntity(string name)
        {
            if (!entities.ContainsKey(name))
            {
                entities[name] = new Entity { Name = name };
            }

            return entities[name];
        }

        public PurposeDto Build() => purposeDto;
    }
}