using System.Collections.Generic;

namespace CharacterBuilder.Data
{
    public interface ITrait
    {
        string Name { get; }

        IDictionary<string, IModifier> Mods { get; }

        IDictionary<string, ITag> Tags { get; }
    }
}
