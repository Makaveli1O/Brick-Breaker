using System.Collections.Generic;

namespace Assets.Scripts.Level
{
    public interface ILevelCatalog
    {
        IReadOnlyList<LevelDefinition> GetAvailableLevels();
    }
}