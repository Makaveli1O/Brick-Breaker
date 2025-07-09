using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public static class BlockGrid
    {
        public static float Spacing = 0.3f;

        public static void InitializeFrom(GameObject blockPrefab)
        {
            var sr = blockPrefab.GetComponent<SpriteRenderer>();
            if (sr == null) throw new System.Exception("Block prefab missing SpriteRenderer");

            float size = Mathf.Max(sr.bounds.size.x, sr.bounds.size.y);
            Spacing = size * 1.05f; // 5% buffer to avoid touching
        }
    }
}