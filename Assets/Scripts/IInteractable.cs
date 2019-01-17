using UnityEngine;
interface IInteractable
{
    // Property signatures:
    bool isInteractable{
        get;
        set;
    }
    void Interact(GameObject interactor);
}