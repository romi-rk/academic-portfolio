using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] myObjects;
    public float range = 10f;
    public int npcCount = 10;
    public float maxDistanceFromCamera = 30f;

    private int count = 0;
    private List<GameObject> spawnedNPCs = new List<GameObject>();

    void Update()
    {
        // Spawn NPCs up to the limit
        if (count < npcCount)
        {
            if (TrySpawnOnNavMesh(out Vector3 spawnPoint))
            {
                int randomIndex = Random.Range(0, myObjects.Length);
                GameObject npc = Instantiate(myObjects[randomIndex], spawnPoint, Quaternion.identity);
                spawnedNPCs.Add(npc);
                count++;
            }
        }

        // Despawn NPCs that are too far from the camera
        for (int i = spawnedNPCs.Count - 1; i >= 0; i--)
        {
            GameObject npc = spawnedNPCs[i];
            if (npc == null) continue;

            float distance = Vector3.Distance(transform.position, npc.transform.position);
            if (distance > maxDistanceFromCamera)
            {
                Destroy(npc);
                spawnedNPCs.RemoveAt(i);
                count--; // Reduce count so a new one can be spawned
            }
        }
    }

    bool TrySpawnOnNavMesh(out Vector3 result)
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * range;
        randomPoint.y = transform.position.y;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
