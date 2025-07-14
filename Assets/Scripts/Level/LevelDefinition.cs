namespace Assets.Scripts.Level
{
    public class LevelDefinition
    {
        public int Id { get; }
        public string DisplayName { get; }

        public LevelDefinition(int id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }
    }
}