namespace Assets.Scripts.Level
{
    public class LevelDefinition
    {
        public int Id { get; }
        public string DisplayName { get; }
        public string Description { get; }

        public LevelDefinition(int id, string displayName, string description)
        {
            Id = id;
            DisplayName = displayName;
            Description = description;
        }
    }
}