namespace CharacterBuilder.Utilities
{
    public interface IIdRepository
    {
        int RegisterId(int id);

        int GetNewId();
    }
}
