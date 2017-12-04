using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterBuilder.Data
{
    public interface IBonusScope<TValue>
        where TValue : IEquatable<TValue>
    {
        IBonusTag<TValue> Source { get; set; }

        ICharacter Character { get; set; }

        ITrait Target { get; set; }
    }
}
