using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //this script is responsible for controlling the spawn of atoms and visuals of the arena

    [Tooltip("The area where the atoms will be spawned within at random positions")]
    public float spawnRadius = 10;
    [Tooltip("The color of the gizmo that shows the area of the spawn radius")]
    public Color spawnerGizmoColor = new Color(100,100,100,100);
    [Tooltip("The prefab which to spawn for each atom")]
    public GameObject atomPrefab;
    [Tooltip("The list of instructions of what to spawn")]
    public AtomSpawnRate[] atomSpawner;
    private bool showArena = false;

    [SerializeField] private List<GameObject> arenas;

    [System.Serializable]
    public struct AtomSpawnRate
    {
        [Tooltip("The scriptable obj which contains all the details to the atom")]
        public Atom atomScriptable;
        [Tooltip("The number of atoms which contain the scriptable obj specified in Atom Scriptable")]
        public int atomSpawnRate;
    }

    public void Start()
    {
        //upon start, spawn the atoms
        SpawnAtoms();

        //just for safety, make time regular at start
        Time.timeScale = 1;
    }

    //in case you want to preload some atoms for some reason, you cant do it via context menu
    [ContextMenu("SpawnAtoms")]
    public void SpawnAtoms()
    {
        //check through all the items within the atom spawner list 
        for (int j = 0; j < atomSpawner.Length; j++)
        {
            //within the struct, get a reference from the chosen atom scriptable obj
            for (int i = 0; i < atomSpawner[j].atomSpawnRate; i++)
            {
                //instantiate a copy of an atom prefab
                var instance = Instantiate(atomPrefab);
                //assign the instance its corresponding scriptable obj
                instance.GetComponent<AtomBehaviour>().atomProperties = atomSpawner[j].atomScriptable;
                //sets the parent to the spawner itself, so they are all grouped
                instance.transform.parent = transform;
                //gives it the name that corresponds to its scriptable obj + a number to differentiate it from the others
                instance.name = atomSpawner[j].atomScriptable.name + " " + (i + 1);
                //randomly picks a position within the radius defined by the player in the inspector, it is square shaped
                float posX = Random.Range(spawnRadius / 2 * -1, spawnRadius / 2);
                float posY = Random.Range(spawnRadius / 2 * -1, spawnRadius / 2);
                float posZ = Random.Range(spawnRadius / 2 * -1, spawnRadius / 2);
                //finally, set the newly set position
                instance.transform.position = new Vector3(posX, posY, posZ);
            }
        }
    }

    //just in case you end up creating atoms in edit mode, you can use this to delete them all
    [ContextMenu("DeleteAtoms")]
    public void DeleteAtoms()
    {
        //finds all the objs tagged as atom
        var tmpCount = GameObject.FindGameObjectsWithTag("Atom");

        //using the number of atoms found, delete each of them depending on what mode is currently used
        for (int j = 0; j < tmpCount.Length; j++)
        {
            if (!Application.isPlaying)
            {
                DestroyImmediate(tmpCount[j].gameObject);
            }
            else
            {
                Destroy(tmpCount[j].gameObject);
            }
        }
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        //to visually aid the user, create a gizmo that represents the area where the atoms will be spawned within
        Gizmos.color = spawnerGizmoColor;
        Gizmos.DrawCube(transform.position, new Vector3(spawnRadius, spawnRadius, spawnRadius));

        //below we have a loop that is used to visually represent the area that an atom will use as a reference when the user
        //sets its output to change color vector mode

        //checks the number of atoms that player has defined one by one
        for (int i = 0; i < atomSpawner.Length; i++)
        {            
            //get the reference to the scriptable obj
            for (int j = 0; atomSpawner[i].atomScriptable.atomicEvents.Length > j; j++)
            {
                //checks if the player has chosen the output to change color vector mode, if not, draw nothing
                if (atomSpawner[i].atomScriptable.atomicEvents[j].setColorBasedOnPosition == true)
                {
                    Gizmos.color = atomSpawner[i].atomScriptable.atomicEvents[j].colorVectorGizmo;
                    Gizmos.DrawCube(transform.position, atomSpawner[i].atomScriptable.atomicEvents[j].colorVector);
                }
            }
        }
    }

    private void Update()
    {
        //press v to turn the arena's visuals on and off
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!showArena)
            {
                arenas[0].GetComponent<MeshRenderer>().enabled = true;
                showArena = true;
            }

            else
            {
                arenas[0].GetComponent<MeshRenderer>().enabled = false;
                showArena = false;
            }
        }

        //press esc to quit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //press space to pause and unpause
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }
}