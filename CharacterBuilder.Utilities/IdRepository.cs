using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterBuilder.Utilities
{
    public class IdRepository : IIdRepository
    {
        private List<int> Ids = new List<int>();

        public int GetNewId()
        {
            int next = Ids.Count;
            Ids.Add(next);
            return next;
        }

        public int RegisterId(int id)
        {
            return 0;
        }
    }
}
