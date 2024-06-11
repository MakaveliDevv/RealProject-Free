using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private int maxAmount = 8; 
    [SerializeField] private GameObject prefab; 
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private float interval = 4f;
    [SerializeField] private float objectDuration = 10f;
    private List<GameObject> spawnedObjects = new();

    [SerializeField] private Transform gameManager;

    private Coroutine destroyCoroutine;
    private Dictionary<GameObject, Coroutine> destroyCoroutines = new Dictionary<GameObject, Coroutine>();

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }


    void Update()
    {
        foreach (var obj in spawnedObjects)
        {
            Star starObj = obj.GetComponent<Star>();
            if (starObj.interact && destroyCoroutines.ContainsKey(obj))
            {
                StopCoroutine(destroyCoroutines[obj]);
                destroyCoroutines.Remove(obj);
                break; // Assuming only one object should be destroyed per frame
            }
        }
    }

    void InstantiateObj() 
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        GameObject spawnedObj = Instantiate(prefab, randomSpawnPoint.position, Quaternion.identity);
        spawnedObjects.Add(spawnedObj);
        spawnedObj.transform.SetParent(gameManager);

        Coroutine destroyCoroutine = StartCoroutine(DestroyAfterTime(spawnedObj, objectDuration));
        destroyCoroutines.Add(spawnedObj, destroyCoroutine);
    }

    private IEnumerator SpawnObjects() 
    {
        while(true)
        {
            if (spawnedObjects.Count < maxAmount)
            {
                InstantiateObj();

                yield return new WaitForSeconds(interval);
            }
            else 
            {
                yield return new WaitForSeconds(interval);
            }
        }
    }

    private IEnumerator DestroyAfterTime(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        spawnedObjects.Remove(obj);
        destroyCoroutines.Remove(obj);
        Destroy(obj);  
    }
}
