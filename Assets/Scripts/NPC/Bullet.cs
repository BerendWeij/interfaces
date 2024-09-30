using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;

    private float _timeAlive;

    public void OnObjectSpawn()
    {
        _timeAlive = 0f;
    }

    public void OnObjectReturn()
    {
        // Reset eventuele andere properties hier
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        _timeAlive += Time.deltaTime;

        if (_timeAlive > lifetime)
        {
            ObjectPool.Instance.ReturnToPool("Bullet", gameObject);
        }
    }
}

