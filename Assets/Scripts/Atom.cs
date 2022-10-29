using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Atom", menuName = "Atom")]
public class Atom : ScriptableObject
{
    #region Updated Properties In Real Time
    [Header("Updated Properties In Real Time")]

    //the values of the atom get updated in real time in the area below, so you can tweak it as you go if you wanna change it
    //you will also be able to see the variables changing in real time as they update as well
    //I need to make a bool which allows the user to reset the values of the scriptable obj back to default upon stop playing

    [Space]

    public bool stuckPrevention = false;
    public float atomSize = 1;
    public float atomSpeed = 1;
    public bool drawAtom = true;
    public Color atomColor;

    [Min(0)] public int bondNum;
    [Tooltip("if you leave this at 0, there is no max limit of atom bonds")] [Min(0)]
    public int maxNumOfAtoms = 0;
    public List<int> bondingChart;
    #endregion

    [Space]

    public AtomEvents[] atomicEvents;

    [System.Serializable]
    public struct AtomEvents
    {
        public enum trigger { Start, Collision, Trigger_Area, Always_Update, Becomes_Child, Becomes_Parent, Has_Children }
        public trigger triggerEvents;
        [HideInInspector] public string collisionTag;
        [HideInInspector] public int specificAtom;

        public enum output { Change_Scale, Change_Speed, Change_Color, Atomic_Bond, Make_Kinematic, Apply_Force, Particles, Hide_Unhide }
        public output outputEvents;
        [HideInInspector] public Vector3 scaleAmount;
        [HideInInspector] public float speedAmount;
        [HideInInspector] public bool setColorBasedOnPosition;
        [HideInInspector] public Vector3 colorVector;
        [HideInInspector] public Color fixedColor;
        [HideInInspector] public bool parent_Unparent;
        [HideInInspector] public Vector2 unparentForce_Radius;
    }
}