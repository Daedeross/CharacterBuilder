using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterBuilder.Data.Contract
{
    public interface ICharacter
    {
        string Name { get; }
        
        Dictionary<string, ITrait> Attributes { get; }
        Dictionary<string, ITrait> Advantages { get; }
        Dictionary<string, ITrait> Disadvantages { get; }
        Dictionary<string, ITrait> Skills { get; }
        Dictionary<string, ITrait> Gear { get; }
    }
}
