using CharacterBuilder.Foundation;
using CharacterBuilder.Tags.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterBuilder.Tags
{
    class Bonus<TValue> : NotificationObject, IBonus<TValue>
        where TValue : IEquatable<TValue>
    {
        public Bonus(ITag source)
        {
            Source = source;
        }
        
        public ITag Source { get; protected set; }

        private TValue _bonus;
        public TValue Value
        {
            get { return _bonus; }
            set
            {
                if (!_bonus.Equals(value))
                {
                    _bonus = value;
                    RaisePropertyChanged(nameof(Value));
                }
            }
        }
    }
}
