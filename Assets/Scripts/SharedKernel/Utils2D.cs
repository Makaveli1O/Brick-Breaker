using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.SharedKernel
{
    public class Utils2D
    {
        public class PositionConvertor2D
        {
            public static Vector2 ToVector2(int2 value)
            {
                return new Vector2(value.x, value.y);
            }
            public static Vector2 ToVector2(float2 value)
            {
                return new Vector2(value.x, value.y);
            }

            public static int2 ToInt2(Vector2 value)
            {
                return new int2((int)value.x, (int)value.y);
            }
        }

        public static Color BlendColours(List<Color> colours)
        {
            if (colours.Count == 0)
                return Color.gray;

            float r = 0f, g = 0f, b = 0f;
            foreach (var c in colours)
            {
                r += c.r;
                g += c.g;
                b += c.b;
            }

            return new Color(r / colours.Count, g / colours.Count, b / colours.Count);
        }

        /// <summary>
        /// Returns a random point inside the visible camera viewport in world space.
        /// </summary>
        /// <param name="camera">Camera to use, defaults to main camera if null.</param>
        public static Vector3 GetRandomVisiblePoint(Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            // Random point in viewport (x,y between 0 and 1)
            float x = UnityEngine.Random.Range(0f, 1f);
            float y = UnityEngine.Random.Range(0f, 1f);

            Vector3 viewportPoint = new Vector3(x, y, camera.nearClipPlane);

            // Convert viewport to world point
            Vector3 worldPoint = camera.ViewportToWorldPoint(viewportPoint);

            // Set z = 0 for 2D world space
            worldPoint.z = 0f;

            return worldPoint;
        }

        // Reteurn world units bounds of given GO
        public static Bounds GetWorldBounds(GameObject go)
        {
            SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
            if (renderers.Length == 0)
                return new Bounds(go.transform.position, Vector3.zero);

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
                bounds.Encapsulate(renderers[i].bounds);

            return bounds;
        }

        public static Vector3 GetAxisAlignedVisiblePoint(Vector3 origin)
        {
            Camera cam = Camera.main;
            Vector3 viewPos = cam.WorldToViewportPoint(origin);
            Vector3 targetViewport = viewPos;

            if (UnityEngine.Random.value < 0.5f)
            {
                // Horizontal movement
                targetViewport.x = UnityEngine.Random.Range(0.1f, 0.9f);
            }
            else
            {
                // Vertical movement
                targetViewport.y = UnityEngine.Random.Range(0.1f, 0.9f);
            }

            Vector3 world = cam.ViewportToWorldPoint(new Vector3(targetViewport.x, targetViewport.y, viewPos.z));
            world.z = origin.z;
            return world;
        }

        public static void ReduceScale(Transform transform, float factor)
        {
            if (factor <= 0f || factor >= 1f)
                throw new ArgumentOutOfRangeException(nameof(factor), "Factor must be between 0 and 1 (exclusive).");

            transform.localScale *= factor;
        }
     
    }
}
