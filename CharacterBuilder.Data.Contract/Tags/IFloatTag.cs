using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterBuilder.Data
{
    public interface IFloatTag: IValueTag<double>
    {
        bool Truncate { get; set; }
    }
}
