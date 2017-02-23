using System;
using System.ComponentModel;

namespace CharacterBuilder.Data.Contract
{
    public interface IBonusTag<TValue>: ITag
        where TValue : IEquatable<TValue>
    {
        TValue Value { get; }

        ITag Source { get; }
    }
}
