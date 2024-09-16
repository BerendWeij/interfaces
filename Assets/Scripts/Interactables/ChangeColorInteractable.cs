using UnityEngine;

public class ChangeColorInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
}