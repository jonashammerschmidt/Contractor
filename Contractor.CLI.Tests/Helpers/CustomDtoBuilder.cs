using Contractor.Core.MetaModell;

namespace Contractor.CLI.Tests.Helpers
{
    public class CustomDtoBuilder
    {
        private CustomDto customDto = new CustomDto();
        private Dictionary<string, Entity> entities = new();

        public CustomDtoBuilder SetEntity(string name)
        {
            var entity = GetOrCreateEntity(name);
            customDto.Entity = entity;
            customDto.EntityName = name;
            return this;
        }

        public CustomDtoBuilder WithPurpose(string purpose)
        {
            customDto.Purpose = purpose;
            return this;
        }

        public CustomDtoBuilder AddPropertyPath(string fullPath)
        {
            var pathSteps = fullPath.Split('.');
            var pathItems = new List<CustomDtoPathItem>();
            Entity lastEntity = customDto.Entity;

            foreach (var step in pathSteps)
            {
                var nextEntity = GetOrCreateEntity(step);
                pathItems.Add(new CustomDtoPathItem
                {
                    Entity = lastEntity,
                    OtherEntity = nextEntity,
                    PropertyName = step
                });
                lastEntity = nextEntity;
            }

            customDto.Properties ??= new List<CustomDtoProperty>();
            customDto.Properties.Add(new CustomDtoProperty
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

        public CustomDto Build() => customDto;
    }
}