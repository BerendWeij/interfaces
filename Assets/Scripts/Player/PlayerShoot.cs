using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private string bulletTag = "Bullet";
    [SerializeField] private Transform firePoint;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var bullet = ObjectPool.Instance.SpawnFromPool(bulletTag, firePoint.position, firePoint.rotation);
            if (bullet == null)
            {
                Debug.LogWarning("Geen beschikbare kogels in de pool.");
            }
        }
    }
}