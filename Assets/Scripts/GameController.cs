using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public void Start()
    {
        int atomType = Random.Range(1, 4);

        switch (atomType)
        {
            case 1:
                //gameObject.AddComponent<Blue>();
                break;
            case 2:
                //gameObject.AddComponent<Red>();
                break;
            case 3:
                //gameObject.AddComponent<Green>();
                break;
        }
    }
}
