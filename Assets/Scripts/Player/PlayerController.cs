using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float interactionDistance = 2f;

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontal, 0, vertical).normalized * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E)) Interact();
    }

    private void Interact()
    {
        var colliders = Physics.OverlapSphere(transform.position, interactionDistance);
        foreach (var collider in colliders)
        {
            var interactable = collider.GetComponent<IInteractable>();
            if (interactable == null) continue;
            
            interactable.Interact();
            break; // Interact with the first found interactable
        }
    }
}