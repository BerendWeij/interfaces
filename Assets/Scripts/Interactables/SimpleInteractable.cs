using UnityEngine;

public class SimpleInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}