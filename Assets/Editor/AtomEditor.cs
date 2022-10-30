using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(Atom))]
public class AtomEditor : Editor
{
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Atom atom = (Atom)target;

        //in essence, the way I made this custom menu work is:
        //the atom scriptable objs have loads of hidden info, and whenever the player specifies that an event trigger or output
        //will need a piece of info which is currently hidden, then this script serializes that field so it can be modified
        //each event in the struct has its own variables, meaning that events' variables won't intertwine with each other and cause trouble

        //checks to see if there is at least one event
        if (atom.atomicEvents.Length != 0)
        //goes through each event in the struct
        for (int i = 0; i < atom.atomicEvents.Length; i++)
        {
            EditorGUILayout.BeginVertical();

            //label the first area of serializable fields "triggers", and give it a number which corresponds to what event it references
            EditorGUILayout.LabelField("Trigger " + i + ":");
            GUILayout.Space(2);

            //essentually, each of the ifs below do the exact same thing, both in triggers and in outputs:
            //they go through each event in the current scriptable obj, and if it finds a reference to a specific variable that is present
            //in the enums, then serialize it so the player can modify it

            #region Trigger Overrides
            if (atom.atomicEvents[i].triggerEvents.ToString() == "Collision")
            {
                GUIContent collisionTag = new GUIContent("Collision Tag", "The tag of the object that needs to be collided with. Leave empty to make this collide with anything");
                atom.atomicEvents[i].collisionTag = EditorGUILayout.TextField(collisionTag, atom.atomicEvents[i].collisionTag);

                if (atom.atomicEvents[i].collisionTag == "Atom")
                {
                    GUIContent specificAtom = new GUIContent("Specific Atom", "Specify the bond number of the atom you wish to collide with");
                    atom.atomicEvents[i].specificAtom = EditorGUILayout.IntField(specificAtom, atom.atomicEvents[i].specificAtom);
                }
            }
            else if (atom.atomicEvents[i].triggerEvents.ToString() == "Start")
            {
                EditorGUILayout.LabelField("No overrides");
            }
            else if (atom.atomicEvents[i].triggerEvents.ToString() == "Always_Update")
            {
                EditorGUILayout.LabelField("No overrides");
            }
            else if (atom.atomicEvents[i].triggerEvents.ToString() == "Becomes_Child")
            {
                EditorGUILayout.LabelField("No overrides");
            }
            else if (atom.atomicEvents[i].triggerEvents.ToString() == "Becomes_Parent")
            {
                EditorGUILayout.LabelField("No overrides");
            }
            else if (atom.atomicEvents[i].triggerEvents.ToString() == "Has_Children_Amount")
            {
                GUIContent childrenRequired = new GUIContent("Required Number of Children", "If at least the declared number of children is reached, this will trigger every time a new child is added");
                atom.atomicEvents[i].childrenRequired = EditorGUILayout.IntField(childrenRequired, atom.atomicEvents[i].childrenRequired);
            }
            #endregion

            GUILayout.Space(10);

            //label the second area of serializable fields "outputs", and give it a number which corresponds to what event it references
            EditorGUILayout.LabelField("Output " + i + ":");
            GUILayout.Space(2);

            #region Output Overrides
            if (atom.atomicEvents[i].outputEvents.ToString() == "Change_Scale")
            {
                GUIContent content = new GUIContent("Change Scale", "The amount you specify will be added onto the current size. This accepts negative values too");
                atom.atomicEvents[i].scaleAmount = EditorGUILayout.Vector3Field(content, atom.atomicEvents[i].scaleAmount);
            }
            else if (atom.atomicEvents[i].outputEvents.ToString() == "Change_Speed")
            {
                GUIContent content = new GUIContent("Change Speed", "The amount you specify will be added onto the current speed. This accepts negative values too");
                atom.atomicEvents[i].speedAmount = EditorGUILayout.FloatField(content, atom.atomicEvents[i].speedAmount);
            }
            else if (atom.atomicEvents[i].outputEvents.ToString() == "Change_Color")
            {
                GUIContent intensity = new GUIContent("Change Color Intensity", "The amount you specify will be multiplied by the color you specify. Use only positive, non zero numbers");
                atom.atomicEvents[i].colorIntensity = EditorGUILayout.FloatField(intensity, atom.atomicEvents[i].colorIntensity);
                atom.atomicEvents[i].setColorBasedOnPosition = EditorGUILayout.Toggle("Set Color Based On Position", atom.atomicEvents[i].setColorBasedOnPosition);

                if (atom.atomicEvents[i].setColorBasedOnPosition == false)
                {
                    GUIContent col = new GUIContent("Fixed Color", "Sets the color of the atom to the specified color");
                    atom.atomicEvents[i].fixedColor = EditorGUILayout.ColorField(col, atom.atomicEvents[i].fixedColor);
                }
                else
                {
                    GUIContent gizmo = new GUIContent("Gizmo Color", "The color of the area specified in Color Based on Position. This is purely aesthetic and will not affect the atom itself.");
                    atom.atomicEvents[i].colorVectorGizmo = EditorGUILayout.ColorField(gizmo, atom.atomicEvents[i].colorVectorGizmo);
                    GUIContent area = new GUIContent("Color Based on Position", "Sets the color of the atom based on its position within the specified values, where R = X, G = Y, B = Z");
                    atom.atomicEvents[i].colorVector = EditorGUILayout.Vector3Field(area, atom.atomicEvents[i].colorVector);
                }
            }
            else if (atom.atomicEvents[i].outputEvents.ToString() == "Atomic_Bond")
            {
                //the reason I decided against making each event have its own values was because each atom should only have one unique value as opposed to a
                //value per event, which would mean that I would end up making multiple copies of the same value in the inspector, which would be pointless
                EditorGUILayout.LabelField("Refer back to the variables above: Bond Num, Max Num Of Atoms and Bonding Chart");
                EditorGUILayout.LabelField("DO NOT use Atomic Bond if you are not using a collision, it will NOT work");

                GUIContent toggle = new GUIContent("Parent / Unparent", "When true, it will parent the other object. When false, it will unparent all the objects currently as children of this object.");
                atom.atomicEvents[i].parent_Unparent = EditorGUILayout.Toggle(toggle, atom.atomicEvents[i].parent_Unparent);
                if (atom.atomicEvents[i].parent_Unparent == false)
                {
                        GUIContent force = new GUIContent("Unparent Force and Radius", "The first variable is the amount of force which the children atoms fly off from the parent. The second is the radius of the force being applied, starting from this atom's position");
                        atom.atomicEvents[i].unparentForce_Radius = EditorGUILayout.Vector2Field(force, atom.atomicEvents[i].unparentForce_Radius);
                }
            }
            else if (atom.atomicEvents[i].outputEvents.ToString() == "Change_Kinematic")
            {
                GUIContent toggle = new GUIContent("Change Kinematic", "When true, becomes kinematic. When false, it stops being kinematic");
                atom.atomicEvents[i].changeKinematic = EditorGUILayout.Toggle(toggle, atom.atomicEvents[i].changeKinematic);
            }
            else if (atom.atomicEvents[i].outputEvents.ToString() == "Hide_Unhide")
            {
                GUIContent toggle = new GUIContent("Hide / Unhide", "When true, becomes visible. When false, becomes invisible");
                atom.atomicEvents[i].hide_Unhide = EditorGUILayout.Toggle(toggle, atom.atomicEvents[i].hide_Unhide);
            }
            else if (atom.atomicEvents[i].outputEvents.ToString() == "Apply_Force")
            {
                EditorGUILayout.LabelField("This will not work if the object in question is a child, because it will not have a rigidbody");
                GUIContent force = new GUIContent("Apply Force", "Will choose a random direction, then apply force to this atom in that direction, with the intensity that is specified here");
                atom.atomicEvents[i].forceAmount = EditorGUILayout.FloatField(force, atom.atomicEvents[i].forceAmount);
            }
            else if (atom.atomicEvents[i].outputEvents.ToString() == "Particles")
            {
                //I decided to stop adding features so I just didn't add any particle overrides, although its simple to do so if I want to
                EditorGUILayout.LabelField("No output overrides");
            }


            EditorGUILayout.EndVertical();

            //adds a bar to make it clear the division between events
            if (i + 1 < atom.atomicEvents.Length)
            {
                GUILayout.Space(10);
                GUILayout.Label("----- /// -----");
                GUILayout.Space(10);
            }

            #endregion
        }


    }
    
#endif
}
