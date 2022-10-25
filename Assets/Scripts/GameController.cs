using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float spawnRadius = 10;
    public GameObject atomPrefab;

    public AtomSpawnRate[] atomSpawner;

    private bool gameStarted = false;

    [SerializeField] private List<GameObject> arenas;

    [System.Serializable]
    public struct AtomSpawnRate
    {
        public Atom atomScriptable;
        public int atomSpawnRate;
    }

    public void Start()
    {
        gameStarted = true;

        SpawnAtoms();
    }

    [ContextMenu("SpawnAtoms")]
    public void SpawnAtoms()
    {
        for (int j = 0; j < atomSpawner.Length; j++)
        {
            for (int i = 0; i < atomSpawner[j].atomSpawnRate; i++)
            {
                var instance = Instantiate(atomPrefab);
                instance.GetComponent<AtomBehaviour>().atomProperties = atomSpawner[j].atomScriptable;
                instance.transform.parent = transform;
                instance.name = atomSpawner[j].atomScriptable.name + " " + (i + 1);
                instance.transform.localScale = new Vector3(1, 1, 1) * atomSpawner[j].atomScriptable.atomSize;
                float posX = Random.Range(spawnRadius / 2 * -1, spawnRadius / 2);
                float posY = Random.Range(spawnRadius / 2 * -1, spawnRadius / 2);
                float posZ = Random.Range(spawnRadius / 2 * -1, spawnRadius / 2);
                instance.transform.position = new Vector3(posX, posY, posZ);
            }
        }
    }

    [ContextMenu("DeleteAtoms")]
    public void DeleteAtoms()
    {
        var tmpCount = GameObject.FindGameObjectsWithTag("Atom");
        //Debug.Log("Total atoms = " + tmpCount.Length);

        for (int j = 0; j < tmpCount.Length; j++)
        {
            if (!Application.isPlaying)
            {
                //Debug.Log("Destroying atom = " + j);
                DestroyImmediate(tmpCount[j].gameObject);
            }  
            else
            {
                //Debug.Log("Destroying atom = " + j);
                Destroy(tmpCount[j].gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (arenas.Count != 0 && !gameStarted)
        {
            arenas[0].SetActive(true);
            arenas[0].GetComponent<MeshRenderer>().enabled = true;

            for (int i = 1; i < arenas.Count; i++)
            {
                arenas[i].SetActive(false);
                arenas[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }

        Gizmos.color = new Color32(255, 255, 255, 100);
        Gizmos.DrawCube(transform.position, new Vector3(spawnRadius, spawnRadius, spawnRadius));

        for (int i = 0; i < atomSpawner.Length; i++)
        {
            //Gizmos.color = atomSpawner[i].atomScriptable.atomColor;
            Gizmos.color = new Color(atomSpawner[i].atomScriptable.atomColor.r, atomSpawner[i].atomScriptable.atomColor.g, atomSpawner[i].atomScriptable.atomColor.b, atomSpawner[i].atomScriptable.atomColor.a);
            Gizmos.DrawCube(transform.position, atomSpawner[i].atomScriptable.colorVector);
        }
    }
}