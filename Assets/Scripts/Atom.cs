using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Atom", menuName = "Atom")]
public class Atom : ScriptableObject
{
    //this scriptable obj is responsible for containing all the informations that the atom prefabs will use whilst running the game
    //each property present here was designed to work MOSTLY independently so that you can proceduraly pick and choose what
    //atributes you would like to change at a time
    //the reason there are many hidden properties is because there is a custom menu script which reveals them only when they are going
    //to be used, so that the player doesn't have to deal with information they are not using

    #region Updated Properties In Real Time
    [Tooltip("In case atoms start getting stuck on walls, tick this on")]
    public bool stuckPrevention = false;
    [HideInInspector] public float atomSpeed = 1;
    [HideInInspector] public Color atomColor;

    [Tooltip("This number represents the unique atomic number that this atom will use in bonding with other atoms. Make sure not to specify two atom scriptable objects with the same bond number")]
    [Min(0)] public int bondNum;
    [Tooltip("The numbers present in this list are the atom's bond numbers that are allowed to bond with this atom. If an atom outside this list's number attempts to bond, it will NOT work")]
    public List<int> bondingChart;
    #endregion

    [Space]

    [Tooltip("Use the events to dynamically change the values of this atom in real time!")]
    public AtomEvents[] atomicEvents;

    //in a nutshell, the way I made this work is like this:
    //the events tab of each atom is a struct that takes two arguments, both enums. The first type of enum is a "trigger"
    //by which I mean, it tells the atom when to activate a certain event
    //the second enum is the "output", in other words, what the trigger should be activating when called
    //in combination, they form events that are in essence a visually representated version of if statements but in inspector. Example:
    //if "collision with an obj tagged arena", then "increase size of this atom by 2"
    //this way, there are endless possibilites of what the player can define, and all with a visually friendly inspector that makes sure
    //to occlude unecessary info when not needed, and show things that are important when they are being used

    [System.Serializable]
    public struct AtomEvents
    {
        public enum trigger { Start, Collision, Always_Update, Becomes_Child, Becomes_Parent, Has_Children_Amount }
        public trigger triggerEvents;
        [HideInInspector] public string collisionTag;
        [HideInInspector] public int specificAtom;

        public enum output { Change_Scale, Change_Speed, Change_Color, Atomic_Bond, Change_Kinematic, Hide_Unhide, Apply_Force, Particles }
        public output outputEvents;
        [HideInInspector] public Vector3 scaleAmount;
        [HideInInspector] public float speedAmount;
        [HideInInspector] public bool setColorBasedOnPosition;
        [HideInInspector] public Vector3 colorVector;
        [HideInInspector] public Color colorVectorGizmo;
        [HideInInspector] public float colorIntensity;
        [HideInInspector] public Color fixedColor;
        [HideInInspector] public bool parent_Unparent;
        [HideInInspector] public Vector2 unparentForce_Radius;
        [HideInInspector] public bool changeKinematic;
        [HideInInspector] public bool hide_Unhide;
        [HideInInspector] public float forceAmount;
        [HideInInspector] public int childrenRequired;
    }
}