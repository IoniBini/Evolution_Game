using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int numOfInstances = 10;
    public float spawnRadius = 10;
    public GameObject atomPrefab;

    public void Start()
    {
        for (int i = 0; i < numOfInstances; i++)
        {
            var instance = Instantiate(atomPrefab);
            instance.transform.parent = transform;
            instance.name = "Atom " + (i + 1);
            float posX = Random.Range(spawnRadius/2 * -1, spawnRadius/2);
            float posY = Random.Range(spawnRadius / 2 * -1, spawnRadius / 2);
            float posZ = Random.Range(spawnRadius / 2 * -1, spawnRadius / 2);
            instance.transform.localPosition = new Vector3(posX, posY, posZ);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color32(0, 255, 0, 100);
        Gizmos.DrawCube(transform.position, new Vector3(spawnRadius, spawnRadius, spawnRadius));
    }
}
