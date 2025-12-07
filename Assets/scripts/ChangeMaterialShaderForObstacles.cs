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

        Renderer[] renderers = FindObjectsByType<Renderer>(FindObjectsSortMode.None);

        foreach (Renderer rendererComponent in renderers)
        {
            if (!IsInLayer(rendererComponent.gameObject.layer, targetLayers)) continue;
            
            Material[] rendererMaterials = rendererComponent.sharedMaterials;
            createdMaterials.Clear();
            createdMaterials.TrimExcess();
                
            foreach (Material material in rendererMaterials)
            {
                if (material.shader != fadeNearCamera)
                {
                    Color baseColor = material.GetColor(BaseColorProperty);
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
                    createdMaterials.Add(customTempMaterial);
                }
                else
                {
                    createdMaterials.Add(material);
                }
            }
                
            rendererComponent.SetSharedMaterials(createdMaterials);

        }
    }

    private static bool IsInLayer(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }

    private static Color32 ConvertColorToColor32(Color color)
    {
        Color32 color32 = new Color32(
            (byte)Mathf.Clamp(Mathf.RoundToInt(color.r * 255f), 0, 255),
            (byte)Mathf.Clamp(Mathf.RoundToInt(color.g * 255f), 0, 255),
            (byte)Mathf.Clamp(Mathf.RoundToInt(color.b * 255f), 0, 255),
            (byte)Mathf.Clamp(Mathf.RoundToInt(color.a * 255f), 0, 255));

        return color32;
    }
}
