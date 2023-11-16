using CharacterBuilder.Data;
using CharacterBuilder.Foundation;

namespace CharacterBuilder.Dsl
{
    public interface IScope<out T>
        where T : class, INamedItem
    {
        T Owner { get; }

        T Me { get; }

        ICategorizedTraitContainer Traits { get; }
    }
}
