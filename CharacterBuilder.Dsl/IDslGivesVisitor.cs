using CharacterBuilder.Foundation;

namespace CharacterBuilder.Dsl
{
    public interface IDslGivesVisitor<T> : ICharacterBuilderVisitor<IntermediateParsedGives<T>>
        where T : class, INamedItem
    {
    }
}
