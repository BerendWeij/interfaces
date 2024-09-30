using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    // Singleton instance
    public static ObjectPool Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [Header("Pools Configuration")]
    [SerializeField] private List<Pool> pools;

    private Dictionary<string, Queue<GameObject>> _poolDictionary;

    private void Awake()
    {
        // Singleton patroon implementatie
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialisatie van de pool dictionary
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            var objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                var newClone = Instantiate(pool.prefab, transform, true);
                newClone.SetActive(false);
                objectPool.Enqueue(newClone);
            }

            _poolDictionary.Add(pool.tag, objectPool);
        }
    }

    /// <summary>
    /// Spawn een object uit de pool.
    /// </summary>
    /// <param name="tag">Tag van de pool.</param>
    /// <param name="position">Spawn positie.</param>
    /// <param name="rotation">Spawn rotatie.</param>
    /// <returns>Het gespaawnde GameObject of null als geen beschikbaar.</returns>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        var currentPool = _poolDictionary[tag];

        if (currentPool.Count == 0)
        {
            Debug.LogWarning($"No available objects in pool with tag {tag}.");
            return null;
        }

        var objectToSpawn = currentPool.Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        var pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        pooledObj?.OnObjectSpawn();
        
        return objectToSpawn;
    }

    /// <summary>
    /// Retourneer een object naar de pool.
    /// </summary>
    /// <param name="tag">Tag van de pool.</param>
    /// <param name="objectToReturn">Het GameObject dat teruggegeven moet worden.</param>
    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return;
        }

        var pooledObj = objectToReturn.GetComponent<IPooledObject>();
        pooledObj?.OnObjectReturn();
        
        objectToReturn.SetActive(false);
        
        // Zet het object weer terug in de pool na gebruik
        _poolDictionary[tag].Enqueue(objectToReturn);
    }
}