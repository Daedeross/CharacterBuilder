using CharacterBuilder.Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterBuilder.Data
{
    class Bonus<TValue> : DynamicDataModel, IBonusTag<TValue>
        where TValue : IEquatable<TValue>
    {
        public Bonus(ITag source)
        {
            Source = source;
        }
        
        public int Id { get; }

        public string Text { get; set; }

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

        public bool IsValid { get { return true; } }
    }
}
