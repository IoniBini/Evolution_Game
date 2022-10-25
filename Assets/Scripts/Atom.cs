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

    public bool alwaysUpdateColor = false;
    public bool forceFixateColorChild = false;
    public Color atomColor;
    public Vector3 colorVector = Vector3.one;

    [Min(0)] public int bondNum;
    [Tooltip("if you leave this at 0, there is no max limit of atom bonds")]
    [Min(0)]
    public int maxNumOfAtoms = 0;
    public List<int> bondingChart;
    #endregion
}
