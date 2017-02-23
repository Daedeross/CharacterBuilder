using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterBuilder.Data.Contract
{
    public interface ITrait
    {
        string Name { get; }

        IDictionary<string, IModifier> Mods { get; }

        IDictionary<string, ITag> Tags { get; set; }
    }
}
