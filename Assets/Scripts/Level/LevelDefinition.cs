namespace Assets.Scripts.Level
{
    public class LevelDefinition
    {
        public string Id { get; }
        public string DisplayName { get; }

        public LevelDefinition(string id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }
    }
}