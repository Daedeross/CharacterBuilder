﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterBuilder.Data
{
    public interface ITraitScope
    {
        ICharacter Character { get; }
        ITrait Owner { get; }
    }
}
