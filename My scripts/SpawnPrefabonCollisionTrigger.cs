using UnityEngine;

public class SpawnPrefabOnTrigger : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject prefabToSpawn;

    [Header("Spawn Location")]
    public Transform spawnLocation;

    public string carTag = "laCar";

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag(carTag))
        {
            Debug.Log("Collision !!!");
            if (prefabToSpawn != null && spawnLocation != null)
            {
                Instantiate(prefabToSpawn, spawnLocation.position, spawnLocation.rotation);
                // Debug.Log("Prefab spawned after triggering with 'laCar'.");
            }
            else
            {
                // Debug.LogWarning("Prefab or Spawn Location is not assigned.");
            }
        }
    }
}
