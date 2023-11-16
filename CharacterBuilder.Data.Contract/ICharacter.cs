using CharacterBuilder.Foundation;

namespace CharacterBuilder.Data
{
    public interface ICharacter : ICategorizedTraitContainer, INamedItem
    {
        new string Name { get; set; }
    }
}
