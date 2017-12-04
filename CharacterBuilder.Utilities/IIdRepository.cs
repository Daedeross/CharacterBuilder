using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterBuilder.Utilities
{
    public interface IIdRepository
    {
        int RegisterId(int id);

        int GetNewId();
    }
}
