using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject blackPrefab;
    public GameObject whitePrefab;

    void Start()
    {
        GameObject playground = GameObject.FindGameObjectWithTag("playground");
        
        if (playground != null)
        {
            SpawnObjects(playground, "SpawnPoint");
        }
        else
        {
            Debug.LogError("Не найден объект с тегом 'playground'");
        }
    }

    GameObject InstantiateShashka(GameObject prefab, Transform point)
    {
        return Instantiate(prefab, point.position, point.rotation);
    }

    void SpawnObjects(GameObject parent, string childTag)
    {
        Transform[] spawnPoints = parent.GetComponentsInChildren<Transform>();
        
        foreach (Transform point in spawnPoints)
        {
            if (point.CompareTag(childTag))
            {
                GameObject spawnedObject = gameObject;
                if (point.name == "SP_black")
                {
                    spawnedObject = InstantiateShashka(blackPrefab, point);
                }
                if (point.name == "SP_white")
                {
                    spawnedObject = InstantiateShashka(whitePrefab, point);
                }

                Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();

                if (rb == null)
                {
                    rb = spawnedObject.AddComponent<Rigidbody>();
                }

                rb.useGravity = true;
                rb.isKinematic = false;
                rb.mass = 20;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
        }
    }
}
