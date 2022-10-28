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

    private Renderer target;
    private MaterialPropertyBlock propertyBlock;
    [HideInInspector] public bool preventColorUpdate = false;

    void Start()
    {
        if (!atomProperties.drawAtom)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }

        RandomizeDirection();

        target = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        /*CalculateColorVector();

        if (atomProperties.alwaysUpdateColor && !preventColorUpdate)
        {
            SetColorVector();
        }*/

        if (isChild == false)
            if (GetComponent<Rigidbody>().isKinematic == false) AtomMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //goes through each event present inside the scriptable obj of the atom being used
        for (int i = 0; i < atomProperties.atomicEvents.Length; i++)
        {
            //if it detects one of these events to be a collision one, continue
            if (atomProperties.atomicEvents[i].triggerEvents.ToString() == "Collision")
            {
                //checks to see if the tag present in the event matches that of the obj collided with
                if (collision.gameObject.tag == atomProperties.atomicEvents[i].collisionTag)
                {
                    //if it detects one of these event outputs to be a scale, continue. Then do the same to check each output type
                    if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Change_Scale")
                    {
                        ChangeScale(i);
                    }
                    else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Change_Speed")
                    {
                        ChangeSpeed(i);
                    }
                    else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Change_Color")
                    {
                        SetColor(i);
                    }
                }
                //this makes it so that if there is nothing written in that field, collide with anything
                else if (atomProperties.atomicEvents[i].collisionTag == "")
                {

                }
            }
        }

        //I hard coded collisions to cause a reflect effect by default because I think it would be stupid for the player to have to specify
        //that they want to have things reflect not added by default

        Vector3 dir = collision.GetContact(0).normal;
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        Vector3 currentPos = transform.position;

        Reflection(dir, vel, currentPos);
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
                    else if (atomProperties.bondNum == other.GetComponent<AtomBehaviour>().atomProperties.bondNum)
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

    #region Custom Voids

    #region Calculate Color Vector
    public void CalculateColorVector()
    {
        /*float lerpR = Mathf.InverseLerp(atomProperties.colorVector.x * -1, atomProperties.colorVector.x, transform.position.x);
        float lerpG = Mathf.InverseLerp(atomProperties.colorVector.y * -1, atomProperties.colorVector.y, transform.position.y);
        float lerpB = Mathf.InverseLerp(atomProperties.colorVector.z * -1, atomProperties.colorVector.z, transform.position.z);

        atomProperties.atomColor = new Color(lerpR, lerpG, lerpB);*/
    }
    #endregion

    #region Set Color Vector
    public void SetColor(int i)
    {
        if (atomProperties.atomicEvents[i].setColorBasedOnPosition == 1)
        {
            atomProperties.atomColor = atomProperties.atomicEvents[i].fixedColor;
            propertyBlock.SetColor("_Color", atomProperties.atomColor);
        }
        else
        {
            float lerpR = Mathf.InverseLerp(atomProperties.atomicEvents[i].colorVector.x * -1, atomProperties.atomicEvents[i].colorVector.x, transform.position.x);
            float lerpG = Mathf.InverseLerp(atomProperties.atomicEvents[i].colorVector.y * -1, atomProperties.atomicEvents[i].colorVector.y, transform.position.y);
            float lerpB = Mathf.InverseLerp(atomProperties.atomicEvents[i].colorVector.z * -1, atomProperties.atomicEvents[i].colorVector.z, transform.position.z);

            atomProperties.atomColor = new Color(lerpR, lerpG, lerpB);
            propertyBlock.SetColor("_Color", new Color(lerpR, lerpG, lerpB));
        }
        
        target.SetPropertyBlock(propertyBlock);
    }
    #endregion

    #region Reflection
    public void Reflection(Vector3 dir, Vector3 vel, Vector3 currentPos)
    {
        //https://answers.unity.com/questions/377616/how-to-detect-which-side-of-a-box-collider-was-hit.html

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

    #region Randomize Direction
    public void RandomizeDirection()
    {
        atomDir.x = Random.Range(-0.01f, 0.01f);
        atomDir.y = Random.Range(-0.01f, 0.01f);
        atomDir.z = Random.Range(-0.01f, 0.01f);

        dirResetCounter = 0;
    }
    #endregion

    #region Atom Movement
    public void AtomMovement()
    {
        GetComponent<Rigidbody>().AddForce((atomDir) * atomProperties.atomSpeed, ForceMode.VelocityChange);
    }
    #endregion

    #region Other Into Child
    public void OtherIntoChild(Collider other)
    {
        if (isChild == false)
        {
            if (atomProperties.forceFixateColorChild)
            {
                var atomBe = other.GetComponent<AtomBehaviour>();

                atomBe.preventColorUpdate = true;
                atomBe.propertyBlock.SetColor("_Color", atomProperties.atomColor);
                atomBe.target.SetPropertyBlock(atomBe.propertyBlock);
            }

            Destroy(other.GetComponent<Rigidbody>());
            other.transform.parent = transform;
            other.GetComponent<AtomBehaviour>().isChild = true;
            other.transform.SetSiblingIndex(transform.childCount);
            var line = GetComponent<LineRenderer>();
            line.positionCount++;
            line.SetPosition(transform.childCount, other.transform.localPosition);
        }

        if (transform.childCount >= atomProperties.maxNumOfAtoms && atomProperties.maxNumOfAtoms != 0)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    #endregion

    #region Change Scale
    private void ChangeScale(int i)
    {
        //gonna have to make it so that it limits the minimum amount of scale it can reach, it should not be allowed to go lower than 0
        transform.localScale += atomProperties.atomicEvents[i].scaleAmount;
    }
    #endregion

    #region Change Speed
    private void ChangeSpeed(int i)
    {
        atomProperties.atomSpeed += atomProperties.atomicEvents[i].speedAmount;
    }
    #endregion

    #endregion
}