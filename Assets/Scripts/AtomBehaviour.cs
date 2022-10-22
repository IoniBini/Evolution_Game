using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomBehaviour : MonoBehaviour
{
    [SerializeField] private Atom atomProperties;

    [SerializeField] private Vector3 atomDir;

    void Start()
    {
        atomDir.x = Random.Range(-0.1f, 0.1f);
        atomDir.y = Random.Range(-0.1f, 0.1f);
        atomDir.z = Random.Range(-0.1f, 0.1f);

        var target = GetComponent<Renderer>();
        var propertyBlock = new MaterialPropertyBlock();

        propertyBlock.SetColor("_Color", atomProperties.atomColor);

        target.SetPropertyBlock(propertyBlock);
    }

    private void Update()
    {
        AtomMovement();
    }

    public void AtomDirection(Vector3 dir, Vector3 vel)
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        if (dir.x == -1) { atomDir.x = Mathf.Abs(atomDir.x) * -1; GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Abs(vel.x) * -1, vel.y, vel.z);}
        if (dir.x == 1) { atomDir.x = Mathf.Abs(atomDir.x); GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Abs(vel.x), vel.y, vel.z); }
        if (dir.y == -1) { atomDir.y = Mathf.Abs(atomDir.y) * -1; GetComponent<Rigidbody>().velocity = new Vector3(vel.x, Mathf.Abs(vel.y) * -1, vel.z); }
        if (dir.y == 1) { atomDir.y = Mathf.Abs(atomDir.y); GetComponent<Rigidbody>().velocity = new Vector3(vel.x, Mathf.Abs(vel.y), vel.z); }
        if (dir.z == -1) { atomDir.z = Mathf.Abs(atomDir.z) * -1; GetComponent<Rigidbody>().velocity = new Vector3(vel.x, vel.y, Mathf.Abs(vel.z) * -1); }
        if (dir.z == 1) { atomDir.z = Mathf.Abs(atomDir.z); GetComponent<Rigidbody>().velocity = new Vector3(vel.x, vel.y, Mathf.Abs(vel.z)); }
    }

    public void AtomMovement()
    {
        GetComponent<Rigidbody>().AddForce((atomDir) * atomProperties.atomSpeed, ForceMode.VelocityChange);
    }

    private void OnCollisionStay(Collision collision)
    {
        Vector3 dir = collision.GetContact(0).normal;
        Vector3 vel = GetComponent<Rigidbody>().velocity;

        AtomDirection(dir, vel);
    }
}
