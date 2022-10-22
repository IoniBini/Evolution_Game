using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomBehaviour : MonoBehaviour
{
    [SerializeField] private Atom atomProperties;

    [SerializeField] private Vector3 atomDir;
    private Vector3 oldPos;
    private int dirResetCounter = 0;

    void Start()
    {
        RandomizeDirection();

        var target = GetComponent<Renderer>();
        var propertyBlock = new MaterialPropertyBlock();

        propertyBlock.SetColor("_Color", atomProperties.atomColor);

        target.SetPropertyBlock(propertyBlock);
    }

    private void Update()
    {
        AtomMovement();
    }

    public void AtomDirection(Vector3 dir, Vector3 vel, Vector3 currentPos)
    {
        if (dir.x == -1) { atomDir.x = Mathf.Abs(atomDir.x) * -1; GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Abs(vel.x) * -1, vel.y, vel.z); }
        if (dir.x == 1) { atomDir.x = Mathf.Abs(atomDir.x); GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Abs(vel.x), vel.y, vel.z); }
        if (dir.y == -1) { atomDir.y = Mathf.Abs(atomDir.y) * -1; GetComponent<Rigidbody>().velocity = new Vector3(vel.x, Mathf.Abs(vel.y) * -1, vel.z); }
        if (dir.y == 1) { atomDir.y = Mathf.Abs(atomDir.y); GetComponent<Rigidbody>().velocity = new Vector3(vel.x, Mathf.Abs(vel.y), vel.z); }
        if (dir.z == -1) { atomDir.z = Mathf.Abs(atomDir.z) * -1; GetComponent<Rigidbody>().velocity = new Vector3(vel.x, vel.y, Mathf.Abs(vel.z) * -1); }
        if (dir.z == 1) { atomDir.z = Mathf.Abs(atomDir.z); GetComponent<Rigidbody>().velocity = new Vector3(vel.x, vel.y, Mathf.Abs(vel.z)); }

        if (currentPos == oldPos && dirResetCounter < 2)
        {
            dirResetCounter++;
        }
        else
        {
            if (atomProperties.stuckPrevention) RandomizeDirection();
        }

        //Debug.Log(gameObject.name + " contact");
    }

    public void RandomizeDirection()
    {
        atomDir.x = Random.Range(-0.01f, 0.01f);
        atomDir.y = Random.Range(-0.01f, 0.01f);
        atomDir.z = Random.Range(-0.01f, 0.01f);

        dirResetCounter = 0;
    }

    public void AtomMovement()
    {
        GetComponent<Rigidbody>().AddForce((atomDir) * atomProperties.atomSpeed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 dir = collision.GetContact(0).normal;
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        Vector3 currentPos = transform.position;

        AtomDirection(dir, vel, currentPos);

        /*if (collision.gameObject.transform.tag == "Atom")
        {
            Debug.Log("contact");
        }*/
    }
}
