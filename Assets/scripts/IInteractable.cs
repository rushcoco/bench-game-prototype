using UnityEngine;

public interface IInteractable
{
    // An interactable is anything where an interaction between the player and this can be made. An example is a ladder for example.
    void Interact();

    void SwitchToActionState();
    void ShowHighlight(Material material);
    void HideHighlight(Material material);
}
