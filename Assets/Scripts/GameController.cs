using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float spawnRadius = 10;
    public GameObject atomPrefab;

    public AtomSpawnRate[] atomSpawner;

    [System.Serializable]
    public struct AtomSpawnRate
    {
        public Atom atomScriptable;
        public int atomSpawnRate;
    }


    public void Start()
    {
        for (int j = 0; j < atomSpawner.Length; j++)
        {
            for (int i = 0; i < atomSpawner[j].atomSpawnRate; i++)
            {
                var instance = Instantiate(atomPrefab);
                instance.GetComponent<AtomBehaviour>().atomProperties = atomSpawner[j].atomScriptable;
                instance.transform.parent = transform;
                instance.name = atomSpawner[j].atomScriptable.name + " " + (i + 1);
                instance.transform.localScale = new Vector3(1,1,1) * atomSpawner[j].atomScriptable.atomSize;
                float posX = Random.Range(spawnRadius / 2 * -1, spawnRadius / 2);
                float posY = Random.Range(spawnRadius / 2 * -1, spawnRadius / 2);
                float posZ = Random.Range(spawnRadius / 2 * -1, spawnRadius / 2);
                instance.transform.position = new Vector3(posX, posY, posZ);
            }
        }
            
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color32(0, 255, 0, 100);
        Gizmos.DrawCube(transform.position, new Vector3(spawnRadius, spawnRadius, spawnRadius));
    }
}
