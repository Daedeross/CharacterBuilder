using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterBuilder.Data.Contract
{
    public interface IModifierScope
    {
        ICharacter Character { get; }
        ITrait Parent { get; }
        IModifier Owner { get; }
    }
}
