using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public sealed class ChangeMaterialShaderForObstacles : MonoBehaviour
{
    private static readonly int BaseColorProperty = Shader.PropertyToID("_BaseColor");
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private Shader fadeNearCamera;

    private readonly Dictionary<Color32, Material> cachedByColor = new();
    private readonly List<Material> createdMaterials = new();

    [ContextMenu("Remap Materials")]
    public void RemapMaterials()
    {
        cachedByColor.Clear();
        // targetLayers = LayerMask.GetMask("Obstacle", "Decorative");

        Renderer[] renderers = FindObjectsByType<Renderer>(FindObjectsSortMode.None);

        foreach (Renderer rendererComponent in renderers)
        {
            // Check if renderComponent Game object is on layer in the target layer
            if (IsInLayer(rendererComponent.gameObject.layer, targetLayers))
            {
                Material currMaterial = rendererComponent.sharedMaterial;
                
                if (currMaterial.shader != fadeNearCamera)
                {
                    Color baseColor = currMaterial.GetColor(BaseColorProperty);
                    Color32 key = ConvertColorToColor32(baseColor);

                    if (!cachedByColor.TryGetValue(key, out Material customTempMaterial))
                    {
                        customTempMaterial = new Material(fadeNearCamera)
                        {
                            name = $"{fadeNearCamera.name}_{baseColor}"
                        };
                        
                        customTempMaterial.SetColor(BaseColorProperty,baseColor);
                        cachedByColor.Add(key,customTempMaterial);
                        createdMaterials.Add(customTempMaterial);
                    }

                    rendererComponent.sharedMaterial = customTempMaterial;
                }
            }
            
        }
    }

    private bool IsInLayer(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }

    private Color32 ConvertColorToColor32(Color color)
    {
        Color32 color32 = new Color32(
            (byte)Mathf.Clamp(Mathf.RoundToInt(color.r * 255f), 0, 255),
            (byte)Mathf.Clamp(Mathf.RoundToInt(color.g * 255f), 0, 255),
            (byte)Mathf.Clamp(Mathf.RoundToInt(color.b * 255f), 0, 255),
            (byte)Mathf.Clamp(Mathf.RoundToInt(color.a * 255f), 0, 255));

        return color32;
    }
}
