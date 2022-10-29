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

    #region Atom Triggers

        #region Start
    void Start()
    {
        RandomizeDirection();

        target = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

        //goes through each event present inside the scriptable obj of the atom being used
        for (int i = 0; i < atomProperties.atomicEvents.Length; i++)
        {
            //if it detects one of these events to be a start one, continue
            if (atomProperties.atomicEvents[i].triggerEvents.ToString() == "Start")
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
                //I skipped this one because if you dont have a collision, there is nothing to bond with
                /*else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Atomic_Bond")
                {
                    AtomicBond(collision, i);
                }*/
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Change_Kinematic")
                {
                    ChangeKinematic(i);
                }
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Hide_Unhide")
                {
                    HideUnhide(i);
                }
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Apply_Force")
                {
                    ApplyForce(i);
                }
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Particles")
                {
                    PlayParticle();
                }
            }
        }
    }
    #endregion

        #region Always Update
    private void Update()
    {
        if (isChild == false)
            if (GetComponent<Rigidbody>().isKinematic == false) AtomMovement();

        //goes through each event present inside the scriptable obj of the atom being used
        for (int i = 0; i < atomProperties.atomicEvents.Length; i++)
        {
            //if it detects one of these events to be a start one, continue
            if (atomProperties.atomicEvents[i].triggerEvents.ToString() == "Always_Update")
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
                //I skipped this one because if you dont have a collision, there is nothing to bond with
                /*else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Atomic_Bond")
                {
                    AtomicBond(collision, i);
                }*/
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Change_Kinematic")
                {
                    ChangeKinematic(i);
                }
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Hide_Unhide")
                {
                    HideUnhide(i);
                }
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Apply_Force")
                {
                    ApplyForce(i);
                }
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Particles")
                {
                    PlayParticle();
                }
            }
        }
    }
    #endregion

        #region Collision
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
                    //in case the tag is specifically atom, then it will also ask you what specific atom num you are looking for
                    if (collision.gameObject.tag == "Atom")
                    {
                        //if the specific atom being collided with has the particular bond number you were looking for, then continue
                        if (atomProperties.atomicEvents[i].specificAtom == collision.gameObject.GetComponent<AtomBehaviour>().atomProperties.bondNum)
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
                            else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Atomic_Bond")
                            {
                                AtomicBond(collision, i);
                            }
                            else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Change_Kinematic")
                            {
                                ChangeKinematic(i);
                            }
                            else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Hide_Unhide")
                            {
                                HideUnhide(i);
                            }
                            else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Apply_Force")
                            {
                                ApplyForce(i);
                            }
                            else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Particles")
                            {
                                PlayParticle();
                            }
                        }
                    }  
                    else
                    {
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
                        //I skip this one because you cannot form an atomic bond with a non atom object anyways
                        /*else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Atomic_Bond")
                        {
                            AtomicBond(collision, i);
                        }*/
                        else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Change_Kinematic")
                        {
                            ChangeKinematic(i);
                        }
                        else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Hide_Unhide")
                        {
                            HideUnhide(i);
                        }
                        else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Apply_Force")
                        {
                            ApplyForce(i);
                        }
                        else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Particles")
                        {
                            PlayParticle();
                        }
                    }
                }
                //this makes it so that if there is nothing written in that field, collide with anything
                else if (atomProperties.atomicEvents[i].collisionTag == "")
                {
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
                    else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Atomic_Bond")
                    {
                        AtomicBond(collision, i);
                    }
                    else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Change_Kinematic")
                    {
                        ChangeKinematic(i);
                    }
                    else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Hide_Unhide")
                    {
                        HideUnhide(i);
                    }
                    else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Apply_Force")
                    {
                        ApplyForce(i);
                    }
                    else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Particles")
                    {
                        PlayParticle();
                    }
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
    #endregion

        #region Becomes Child
    //had to make this void public because it is being accessed by the atom that has parented this obj
    public void OnBecomesChild()
    {
        //goes through each event present inside the scriptable obj of the atom being used
        for (int i = 0; i < atomProperties.atomicEvents.Length; i++)
        {
            //if it detects one of these events to be a start one, continue
            if (atomProperties.atomicEvents[i].triggerEvents.ToString() == "Becomes_Child")
            {
                //if it detects one of these event outputs to be a scale, continue. Then do the same to check each output type
                if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Change_Scale")
                {
                    ChangeScale(i);
                }
                //the speed here wont be visibly different until this obj becomes unparented, since while it is a child, it has no rigidbody
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Change_Speed")
                {
                    ChangeSpeed(i);
                }
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Change_Color")
                {
                    SetColor(i);
                }
                //I skipped this one because if you dont have a collision, there is nothing to bond with
                /*else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Atomic_Bond")
                {
                    AtomicBond(collision, i);
                }*/
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Change_Kinematic")
                {
                    ChangeKinematic(i);
                }
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Hide_Unhide")
                {
                    HideUnhide(i);
                }
                //not used because it does not have a rigidbody, hence cannot be used
                /*else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Apply_Force")
                {
                    ApplyForce(i);
                }*/
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Particles")
                {
                    PlayParticle();
                }
            }
        }
    }
    #endregion

        #region Becomes Parent
    private void BecomesParent()
    {
        //goes through each event present inside the scriptable obj of the atom being used
        for (int i = 0; i < atomProperties.atomicEvents.Length; i++)
        {
            //if it detects one of these events to be a start one, continue
            if (atomProperties.atomicEvents[i].triggerEvents.ToString() == "Becomes_Parent")
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
                //I skipped this one because becoming a parent means that you already collided with something, making this redundant
                /*else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Atomic_Bond")
                {
                    AtomicBond(collision, i);
                }*/
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Change_Kinematic")
                {
                    ChangeKinematic(i);
                }
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Hide_Unhide")
                {
                    HideUnhide(i);
                }
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Apply_Force")
                {
                    ApplyForce(i);
                }
                else if (atomProperties.atomicEvents[i].outputEvents.ToString() == "Particles")
                {
                    PlayParticle();
                }
            }
        }
    }
    #endregion

    #endregion

    #region Atom Outputs

    #region Set Color
    public void SetColor(int i)
    {
        if (atomProperties.atomicEvents[i].setColorBasedOnPosition == false)
        {
            atomProperties.atomColor = atomProperties.atomicEvents[i].fixedColor;
            propertyBlock.SetColor("_Color", atomProperties.atomColor * atomProperties.atomicEvents[i].colorIntensity);
        }
        else
        {
            float lerpR = Mathf.InverseLerp(atomProperties.atomicEvents[i].colorVector.x * -1, atomProperties.atomicEvents[i].colorVector.x, transform.position.x);
            float lerpG = Mathf.InverseLerp(atomProperties.atomicEvents[i].colorVector.y * -1, atomProperties.atomicEvents[i].colorVector.y, transform.position.y);
            float lerpB = Mathf.InverseLerp(atomProperties.atomicEvents[i].colorVector.z * -1, atomProperties.atomicEvents[i].colorVector.z, transform.position.z);

            atomProperties.atomColor = new Color(lerpR, lerpG, lerpB);
            propertyBlock.SetColor("_Color", new Color(lerpR, lerpG, lerpB) * atomProperties.atomicEvents[i].colorIntensity);
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

        #region Atomic Bond
    public void AtomicBond(Collision collision, int j)
    {
        //checks if the obj collided with is another atom
        if (collision.gameObject.tag == "Atom")
        {
            //goes through the bonding chart to see which atoms this is allowed to bond with
            for (int i = 0; i < atomProperties.bondingChart.Count; i++)
            {
                //if the other atom's atomicNum is within the list of bonds, then continue
                if (collision.gameObject.GetComponent<AtomBehaviour>().atomProperties.bondNum == atomProperties.bondingChart[i])
                {
                    //to determine who becomes a child of who, check to see which atom has the larger atomicNum
                    if (atomProperties.bondNum > collision.transform.GetComponent<AtomBehaviour>().atomProperties.bondNum)
                    {
                        OtherIntoChild(collision, j);
                    }
                    //in case they have the exact same atomicNum, then continue
                    else if (atomProperties.bondNum == collision.gameObject.GetComponent<AtomBehaviour>().atomProperties.bondNum)
                    {
                        //checks to see which of the two has more children, and decides which becomes a child based on who has more
                        if (transform.childCount > collision.transform.childCount)
                        {
                            OtherIntoChild(collision, j);
                        }
                        //in case they also happen to have the exact same number of children, continue
                        else if (transform.childCount == collision.transform.childCount)
                        {
                            //the final condition is to check which of the two atoms has a higher sibling index to determine who is dominant
                            if (transform.GetSiblingIndex() > collision.transform.GetSiblingIndex())
                            {
                                OtherIntoChild(collision, j);
                            }
                        }
                    }

                    break;
                }
            }
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

        #region Change Kinematic
    private void ChangeKinematic(int i)
    {
        if (atomProperties.atomicEvents[i].changeKinematic)
            GetComponent<Rigidbody>().isKinematic = true;
        else
            GetComponent<Rigidbody>().isKinematic = false;
    }
    #endregion

        #region Hide Unhide
    private void HideUnhide(int i)
    {
        if (!atomProperties.atomicEvents[i].hide_Unhide)
            GetComponent<MeshRenderer>().enabled = false;
        else
            GetComponent<MeshRenderer>().enabled = true;
    }
    #endregion

        #region Apply Force
    private void ApplyForce(int i)
    {
        if (GetComponent<Rigidbody>() != null)
        {
            var currentRigi = GetComponent<Rigidbody>();
            currentRigi.AddForce(new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f)) * atomProperties.atomicEvents[i].forceAmount, ForceMode.VelocityChange);
        }
    }
    #endregion

        #region Play Particle
    private void PlayParticle()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule ma = ps.main;
        ma.startColor = atomProperties.atomColor;
        ps.Play();
    }
    #endregion

        #region Other Into Child
    private void OtherIntoChild(Collision other, int i) //add new bool here for unparenting
    {
        if (atomProperties.atomicEvents[i].parent_Unparent)
        {
            if (isChild == false)
            {
                Destroy(other.gameObject.GetComponent<Rigidbody>());
                other.transform.parent = transform;
                other.gameObject.GetComponent<AtomBehaviour>().isChild = true;
                other.transform.SetSiblingIndex(transform.childCount);
                var line = GetComponent<LineRenderer>();
                line.positionCount++;
                line.SetPosition(transform.childCount, other.transform.localPosition);
                line.startColor = atomProperties.atomColor;
                line.endColor = atomProperties.atomColor;
                line.startWidth = ((transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3) / 2;
                line.endWidth = ((transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3) / 2;
                other.gameObject.GetComponent<AtomBehaviour>().OnBecomesChild();
                BecomesParent();
            }

            

            /*if (transform.childCount >= atomProperties.maxNumOfAtoms && atomProperties.maxNumOfAtoms != 0)
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }*/
        }
        else
        {
            for (int j = transform.childCount - 1; j > 0; j--)
            {
                GameObject currentChild = transform.GetChild(j).gameObject;
                var newRigid = currentChild.AddComponent<Rigidbody>();
                newRigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                currentChild.transform.parent = null;
                currentChild.GetComponent<AtomBehaviour>().isChild = false;
                GetComponent<LineRenderer>().positionCount = 1;
                newRigid.AddExplosionForce(atomProperties.atomicEvents[i].unparentForce_Radius.x, transform.position, atomProperties.atomicEvents[i].unparentForce_Radius.y, 0, ForceMode.VelocityChange);
            }
        }

    }
    #endregion

    #endregion
}