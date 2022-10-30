using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomBehaviour : MonoBehaviour
{
    //it is worth noting that this script contains lots of repetition which can probably be optimized, but due to
    //time constraints, I will refrain from trying to do that. 
    //In saying that, I will also add that I won't explain some things in comments over and over because they
    //are the exact same over and over again

    //this field stays empty and public so that the game controller script can access and insert a scriptable obj
    [HideInInspector]
    public Atom atomProperties;

    private Vector3 atomDir;
    private Vector3 oldPos;
    private int dirResetCounter = 0;
    private bool isChild = false;

    private float resetSpeed;

    private Renderer target;
    private MaterialPropertyBlock propertyBlock;
    [HideInInspector] public bool preventColorUpdate = false;

    #region Atom Triggers

        #region Start
    void Start()
    {
        //capture the initial speed value so it can be reset in the end, and it won't affect the scriptable obj's value
        resetSpeed = atomProperties.atomSpeed;

        //chooses a direction to start moving in
        RandomizeDirection();

        //so that I don't have to keep making new material property block references, I do it only once at the start
        target = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

        //as I said before, there is LOTS of boring repetition in this script. Below I will explain how the loop works every step
        //of the way, but every subsequent one I won't because they are the same. I will of course add notes whenever somehting changes

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
        //the atom will always be moving, so I never reveal the AtomMovement void as an option, it just always gets called
        //though it only ever gets called if this is not a child, because children will not have rigidbodies
        if (isChild == false)
            if (GetComponent<Rigidbody>().isKinematic == false) AtomMovement();

        //each frame, check to see if the number of children this obj has is sufficient to trigger this trigger type
        HasChildAmount();

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
                    //in case it is not colliding with an atom at all, then there is no need to look at what type of atom the collision is with
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

        #region Has Child Amount
    private void HasChildAmount()
    {
        //goes through each event present inside the scriptable obj of the atom being used
        for (int i = 0; i < atomProperties.atomicEvents.Length; i++)
        {
            //if it detects one of these events to be a start one, continue
            if (atomProperties.atomicEvents[i].triggerEvents.ToString() == "Has_Children_Amount")
            {
                //any zero or negative value is disregarded, and children obj are also disregarded
                if (atomProperties.atomicEvents[i].childrenRequired > 0 && isChild == false)
                {
                    //only trigger if the num of children is equal or bigger than what is specified
                    if (transform.childCount >= atomProperties.atomicEvents[i].childrenRequired)
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
                            AtomicBondException(i);
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
        }
    }
    #endregion

    #endregion

    #region Atom Outputs

        #region Set Color
    public void SetColor(int i)
    {
        //if the event received by the trigger specifies that it does NOT want to set color based on position, continue
        if (atomProperties.atomicEvents[i].setColorBasedOnPosition == false)
        {
            //gives the player a specific color to change the atom to
            atomProperties.atomColor = atomProperties.atomicEvents[i].fixedColor;
            //then sets the atom to the color specified
            propertyBlock.SetColor("_Color", atomProperties.atomColor * atomProperties.atomicEvents[i].colorIntensity);
            propertyBlock.SetColor("_EmissionColor", atomProperties.atomColor * atomProperties.atomicEvents[i].colorIntensity);
        }
        //if the event received by the trigger specifies that it wants to set color based on position, continue
        else
        {
            //based on the area that the player can specify (also visually represented with a gizmo), the atom will define a color 
            //the R = X position, G = Y and Z = B
            float lerpR = Mathf.InverseLerp(atomProperties.atomicEvents[i].colorVector.x * -1, atomProperties.atomicEvents[i].colorVector.x, transform.position.x);
            float lerpG = Mathf.InverseLerp(atomProperties.atomicEvents[i].colorVector.y * -1, atomProperties.atomicEvents[i].colorVector.y, transform.position.y);
            float lerpB = Mathf.InverseLerp(atomProperties.atomicEvents[i].colorVector.z * -1, atomProperties.atomicEvents[i].colorVector.z, transform.position.z);

            //then set that color
            atomProperties.atomColor = new Color(lerpR, lerpG, lerpB);
            propertyBlock.SetColor("_Color", new Color(lerpR, lerpG, lerpB) * atomProperties.atomicEvents[i].colorIntensity);
            propertyBlock.SetColor("_EmissionColor", new Color(lerpR, lerpG, lerpB) * atomProperties.atomicEvents[i].colorIntensity);
        }

        target.SetPropertyBlock(propertyBlock);
    }
    #endregion

        #region Reflection
    public void Reflection(Vector3 dir, Vector3 vel, Vector3 currentPos)
    {
        //https://answers.unity.com/questions/377616/how-to-detect-which-side-of-a-box-collider-was-hit.html

        //I used the link above to understand how to create a reflection effect on rigidbodies colliding.
        //All it does is flip the value of force being applied on the obj from positive to negative (and vice versa)
        //depending on what direction the collision came from 

        if (dir.x == -1) { atomDir.x = Mathf.Abs(atomDir.x) * -1; GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Abs(vel.x) * -1, vel.y, vel.z); }
        if (dir.x == 1) { atomDir.x = Mathf.Abs(atomDir.x); GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Abs(vel.x), vel.y, vel.z); }
        if (dir.y == -1) { atomDir.y = Mathf.Abs(atomDir.y) * -1; GetComponent<Rigidbody>().velocity = new Vector3(vel.x, Mathf.Abs(vel.y) * -1, vel.z); }
        if (dir.y == 1) { atomDir.y = Mathf.Abs(atomDir.y); GetComponent<Rigidbody>().velocity = new Vector3(vel.x, Mathf.Abs(vel.y), vel.z); }
        if (dir.z == -1) { atomDir.z = Mathf.Abs(atomDir.z) * -1; GetComponent<Rigidbody>().velocity = new Vector3(vel.x, vel.y, Mathf.Abs(vel.z) * -1); }
        if (dir.z == 1) { atomDir.z = Mathf.Abs(atomDir.z); GetComponent<Rigidbody>().velocity = new Vector3(vel.x, vel.y, Mathf.Abs(vel.z)); }

        //I have observed that on non flat surfaces, this script runs into issues, so I made this failsafe.
        //it checks to see if after a collision, there was no change in position, then randomize the direction and try again
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
        //randomize each axis of force being applied
        atomDir.x = Random.Range(-0.01f, 0.01f);
        atomDir.y = Random.Range(-0.01f, 0.01f);
        atomDir.z = Random.Range(-0.01f, 0.01f);

        //reset the counter in case the atom got stuck
        dirResetCounter = 0;
    }
    #endregion

        #region Atom Movement
    public void AtomMovement()
    {
        //every frame, apply force to the rigidbody depending on its defined direction and speed
        GetComponent<Rigidbody>().AddForce((atomDir) * atomProperties.atomSpeed, ForceMode.VelocityChange);
    }
    #endregion

        #region Atomic Bond
    public void AtomicBond(Collision collision, int j)
    {
        //check for null in case nothing is being collided with, as a failsafe
        if (collision != null)
        {
            //checks if the obj collided with is another atom, as this can only ever occur between atoms
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
    }
    #endregion

        #region Change Scale
    private void ChangeScale(int i)
    {
        //the reason I have to add each individual axis one at a time as opposed to simply adding a vector 3 to the local scale
        //is because in case the player inadvertendly ends up making something too small that it becomes negative, it will break
        //to account for that, I made a hard limit of 0,01 per axis, meaning you can't go any lower than that

        float scaleX = transform.localScale.x + atomProperties.atomicEvents[i].scaleAmount.x;
        float scaleY = transform.localScale.y + atomProperties.atomicEvents[i].scaleAmount.y;
        float scaleZ = transform.localScale.z + atomProperties.atomicEvents[i].scaleAmount.z;

        if (scaleX >= 0.01f)
        {
            transform.localScale += new Vector3(atomProperties.atomicEvents[i].scaleAmount.x, 0, 0);
        }
        else
        {
            transform.localScale = new Vector3(0.01f, transform.localScale.y, transform.localScale.z);
            Debug.Log("Scaling obj " + gameObject.name + " further down in the X axis would cause its values to be negative. Setting size to 0.01 instead");
        }

        if (scaleY >= 0.01f)
        {
            transform.localScale += new Vector3(0, atomProperties.atomicEvents[i].scaleAmount.y, 0);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, 0.01f, transform.localScale.z);
            Debug.Log("Scaling obj " + gameObject.name + " further down in the Y axis would cause its values to be negative. Setting size to 0.01 instead");
        }

        if (scaleZ >= 0.01f)
        {
            transform.localScale += new Vector3(0, 0, atomProperties.atomicEvents[i].scaleAmount.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0.01f);
            Debug.Log("Scaling obj " + gameObject.name + " further down in the Z axis would cause its values to be negative. Setting size to 0.01 instead");
        }
    }
    #endregion

        #region Change Speed
    private void ChangeSpeed(int i)
    {
        //simply add the specified amount to the objs scriptable obj speed. This can be positive or negative
        atomProperties.atomSpeed += atomProperties.atomicEvents[i].speedAmount;
    }
    #endregion

        #region Change Kinematic
    private void ChangeKinematic(int i)
    {
        // a simple switch, if the player ticks the bool to true, make it kinematic, otherwise, the opposite
        if (atomProperties.atomicEvents[i].changeKinematic)
            GetComponent<Rigidbody>().isKinematic = true;
        else
            GetComponent<Rigidbody>().isKinematic = false;
    }
    #endregion

        #region Hide Unhide
    private void HideUnhide(int i)
    {
        //same exact thing as the change kinematic event, but with the renderer of the obj
        if (!atomProperties.atomicEvents[i].hide_Unhide)
            GetComponent<MeshRenderer>().enabled = false;
        else
            GetComponent<MeshRenderer>().enabled = true;
    }
    #endregion

        #region Apply Force
    private void ApplyForce(int i)
    {
        //in case the obj has a rigidbody, continue
        if (GetComponent<Rigidbody>() != null)
        {
            //add force to the obj in a random direction, with a magnitude that the player specifies
            var currentRigi = GetComponent<Rigidbody>();
            currentRigi.AddForce(new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f)) * atomProperties.atomicEvents[i].forceAmount, ForceMode.VelocityChange);
        }
    }
    #endregion

        #region Play Particle
    private void PlayParticle()
    {
        //simple, there is no override, when this is called, it plays the particle inside the atom and gives its color to what is currently used
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule ma = ps.main;
        ma.startColor = atomProperties.atomColor;
        ps.Play();
    }
    #endregion

        #region Other Into Child
    private void OtherIntoChild(Collision other, int i) 
    {
        //unlike the other outputs, this one is not selectable in the enums, it just gets called by other outputs

        //checks to see if the type of event is a parenting event, then continue
        if (atomProperties.atomicEvents[i].parent_Unparent)
        {
            //makes sure that this is not a child, because it would have no rigidbody
            if (isChild == false)
            {
                //destroy's the collision obj's rigidbody
                Destroy(other.gameObject.GetComponent<Rigidbody>());
                //makes that obj into a child of this
                other.transform.parent = transform;
                //lets the other obj know it is now a child
                other.gameObject.GetComponent<AtomBehaviour>().isChild = true;
                //sets the new obj as the last child in the list
                other.transform.SetSiblingIndex(transform.childCount);
                //grabs a reference to the line renderer inside the atom
                var line = GetComponent<LineRenderer>();
                //creates a new vector within the line renderer
                line.positionCount++;
                //sets the newly created vector's position to the position of the child
                line.SetPosition(transform.childCount, other.transform.localPosition);
                //sets the color of the line to this obj's color
                line.startColor = atomProperties.atomColor;
                line.endColor = atomProperties.atomColor;
                //sets the line's width to half the size of this obj
                line.startWidth = ((transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3) / 2;
                line.endWidth = ((transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3) / 2;
                //lets the other obj know that it is supposed to fire off its OnBecomesChild() trigger event;
                other.gameObject.GetComponent<AtomBehaviour>().OnBecomesChild();
                //fire off the becomes parent event
                BecomesParent();
            }
        }
        //checks to see if the type of event is an UNparenting event, then continue
        else if (!atomProperties.atomicEvents[i].parent_Unparent)
        {
            //gets the number of children this obj has
            for (int j = transform.childCount - 1; j > 0; j--)
            {
                //grabs a reference to the current child
                GameObject currentChild = transform.GetChild(j).gameObject;
                //grabs a reference to the current child's newly added rigidbody
                var newRigid = currentChild.AddComponent<Rigidbody>();
                //tells the new rigidbody what collision mode to use (this is crucial. If not set, then the atoms would fly through the arena's walls)
                newRigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                //makes the atom being detached fly away from the parent with a specified force amount
                newRigid.AddExplosionForce(atomProperties.atomicEvents[i].unparentForce_Radius.x, transform.position, atomProperties.atomicEvents[i].unparentForce_Radius.y, 0, ForceMode.VelocityChange);
                //unparents the child
                currentChild.transform.parent = null;
                //tells the child it no longer is a child
                currentChild.GetComponent<AtomBehaviour>().isChild = false;
                //resets the number of lines this obj has
                GetComponent<LineRenderer>().positionCount = 1;
            }
        }

    }
    #endregion

        #region Atomic Bond Exception
    private void AtomicBondException(int i)
    {
        //this function is the exact same thing as other into child in unparenting mode, but this is separate
        //the reason is because other into child takes an arguement of collision, which does NOT necessarily happen
        //in a situation, such as, when the "has number of children" event triggers, meaning it would be null, and it would break
        //I acknoledge I should have made the parenting and unparenting separate to avoid this issue, but oh well, life

        if (!atomProperties.atomicEvents[i].parent_Unparent)
        {
            for (int j = transform.childCount - 1; j >= 0; j--)
            {
                GameObject currentChild = transform.GetChild(j).gameObject;
                currentChild.transform.parent = null;
                currentChild.GetComponent<AtomBehaviour>().isChild = false;
                GetComponent<LineRenderer>().positionCount = 1;
                var newRigid = currentChild.AddComponent<Rigidbody>();
                newRigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                newRigid.AddExplosionForce(atomProperties.atomicEvents[i].unparentForce_Radius.x, transform.position, atomProperties.atomicEvents[i].unparentForce_Radius.y, 0, ForceMode.VelocityChange);
            }
        }
    }
    #endregion

    #endregion

    private void OnApplicationQuit()
    {
        //reset the speed back to its initial state
        atomProperties.atomSpeed = resetSpeed;
    }
}