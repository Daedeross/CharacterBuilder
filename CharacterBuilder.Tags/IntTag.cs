using CharacterBuilder.Tags.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterBuilder.Tags
{
    public class IntTag<TScope> : ValueTag<int, TScope>, IIntTag
    {
        public override int DefaultValue { get { return 0; } }

        public override int FinalValue
        {
            get
            {
                return Value;
            }
        }

        public override bool ApplyBonus(IBonus<int> bonus)
        {
            throw new NotImplementedException();
        }

        public override void ClearBonus()
        {
            throw new NotImplementedException();
        }

        public IntTag(TScope scope)
        {
            this.scope = scope;
        }

        public IntTag(TScope scope, string text)
            : this(scope)
        {
            this.Text = text;
        }
    }
}
