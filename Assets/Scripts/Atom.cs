using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Atom", menuName = "Atom")]
public class Atom : ScriptableObject
{
    //eventually, substitute each of the variables below for scriptable objs, bc this way then I can have a list that the user
    //can specify what particular atributes they want this atom to control, ignoring the other ones, which makes for a less clogged visual
    //of the atom scriptable obj

    #region Atom Variables
    [Header("Atom Variables")]

    public bool stuckPrevention = false;
    public float atomSize = 1;
    public float atomSpeed = 1;
    public bool drawAtom = true;
    public Color atomColor;
    [Min(0)] public int bondNum;
    public List<int> bondingChart;

    #endregion

    [Space]

    #region Cell Variables
    [Header("Cell Variables")]

    [Header("Vision")]
    public float visionAngle;
    public float visionRadius;

    [Space]

    [Header("Health")]
    public float health;
    public float hunger;

    [Space]

    [Header("Movement")]
    public float cellSpeed;
    public float actionSpeed;

    #endregion
}
