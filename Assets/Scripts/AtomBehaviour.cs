using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomBehaviour : MonoBehaviour
{
    public Atom atomProperties;

    [SerializeField] private Vector3 atomDir;
    private Vector3 oldPos;
    private int dirResetCounter = 0;
    [SerializeField] private bool isChild = false;

    void Start()
    {
        if (!atomProperties.drawAtom)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }

        RandomizeDirection();

        var target = GetComponent<Renderer>();
        var propertyBlock = new MaterialPropertyBlock();

        propertyBlock.SetColor("_Color", atomProperties.atomColor);

        target.SetPropertyBlock(propertyBlock);
    }

    private void Update()
    {
        if (isChild == false)
        AtomMovement();
    }

    #region AtomDirection
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
    #endregion

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
    }

    private void OnTriggerEnter(Collider other)
    {
        //checks if the obj collided with is another atom
        if (other.tag == "Atom")
        {
            //goes through the bonding chart to see which atoms this is allowed to bond with
            for (int i = 0; i < atomProperties.bondingChart.Count; i++)
            {
                //if the other atom's atomicNum is within the list of bonds, then continue
                if (other.GetComponent<AtomBehaviour>().atomProperties.bondNum == atomProperties.bondingChart[i])
                {
                    //to determine who becomes a child of who, check to see which atom has the larger atomicNum
                    if (atomProperties.bondNum > other.GetComponent<AtomBehaviour>().atomProperties.bondNum)
                    {
                        OtherIntoChild(other);
                    }
                    //in case they have the exact same atomicNum, then continue
                    else if(atomProperties.bondNum == other.GetComponent<AtomBehaviour>().atomProperties.bondNum)
                    {
                        //checks to see which of the two has more children, and decides which becomes a child based on who has more
                        if (transform.childCount > other.transform.childCount)
                        {
                            OtherIntoChild(other);
                        }
                        //in case they also happen to have the exact same number of children, continue
                        else if (transform.childCount == other.transform.childCount)
                        {
                            //the final condition is to check which of the two atoms has a higher sibling index to determine who is dominant
                            if (transform.GetSiblingIndex() > other.transform.GetSiblingIndex())
                            {
                                OtherIntoChild(other);
                            }
                        }
                    }

                    break;
                }
            }
        }
    }

    public void OtherIntoChild(Collider other)
    {
        if (isChild == false)
        {
            Destroy(other.GetComponent<Rigidbody>());
            other.transform.parent = transform;
            other.GetComponent<AtomBehaviour>().isChild = true;
            other.transform.SetSiblingIndex(transform.childCount);
            var line = GetComponent<LineRenderer>();
            line.positionCount++;
            line.SetPosition(transform.childCount, other.transform.localPosition);
        }
    }
}
