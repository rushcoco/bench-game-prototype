using UnityEngine;

public interface IInspectable
{
    // Any ItemData that can be inspected is either a wordData that can be added to a dictionary or an item that can be collected
    void Inspect();
    void ShowHighlight(Material material);
    void HideHighlight(Material material);
}
