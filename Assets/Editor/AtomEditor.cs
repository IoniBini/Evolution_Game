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

        for (int i = 0; i < atom.atomicEvents.Length; i++)
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Trigger " + i + ":");
            GUILayout.Space(2);

            #region Trigger Overrides
            if (atom.atomicEvents[i].triggerEvents.ToString() == "Collision")
            {
                //should add tooltip here for if equals to nothing, collide with anything
                atom.atomicEvents[i].collisionTag = EditorGUILayout.TextField("Collision Tag", atom.atomicEvents[i].collisionTag);

                if (atom.atomicEvents[i].collisionTag == "Atom")
                {
                    atom.atomicEvents[i].specificAtom = EditorGUILayout.IntField("Specific Atom", atom.atomicEvents[i].specificAtom);
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
            #endregion

            GUILayout.Space(10);

            EditorGUILayout.LabelField("Output " + i + ":");
            GUILayout.Space(2);
            #region Output Overrides
            if (atom.atomicEvents[i].outputEvents.ToString() == "Change_Scale")
            {
                atom.atomicEvents[i].scaleAmount = EditorGUILayout.Vector3Field("Change Scale ", atom.atomicEvents[i].scaleAmount);
            }
            else if (atom.atomicEvents[i].outputEvents.ToString() == "Change_Speed")
            {
                atom.atomicEvents[i].speedAmount = EditorGUILayout.FloatField("Change Speed ", atom.atomicEvents[i].speedAmount);
            }
            else if (atom.atomicEvents[i].outputEvents.ToString() == "Change_Color")
            {
                atom.atomicEvents[i].colorIntensity = EditorGUILayout.FloatField("Change Color Intensity ", atom.atomicEvents[i].colorIntensity);
                atom.atomicEvents[i].setColorBasedOnPosition = EditorGUILayout.Toggle("Set Color Based On Position", atom.atomicEvents[i].setColorBasedOnPosition);

                if (atom.atomicEvents[i].setColorBasedOnPosition == false)
                {
                    atom.atomicEvents[i].fixedColor = EditorGUILayout.ColorField("Fixed Color ", atom.atomicEvents[i].fixedColor);
                }
                else
                {
                    atom.atomicEvents[i].colorVector = EditorGUILayout.Vector3Field("Color Based in Position ", atom.atomicEvents[i].colorVector);
                }
            }
            else if (atom.atomicEvents[i].outputEvents.ToString() == "Atomic_Bond")
            {
                //the reason I decided against making each event have its own values was because each atom should only have one unique value as opposed to a
                //value per event, which would mean that I would end up making multiple copies of the same value in the inspector, which would be pointless
                EditorGUILayout.LabelField("Refer back to the variables above: Bond Num, Max Num Of Atoms and Bonding Chart");
                EditorGUILayout.LabelField("DO NOT use Atomic Bond if you are not using a collision, it will NOT work");
                atom.atomicEvents[i].parent_Unparent = EditorGUILayout.Toggle("Parent / Unparent", atom.atomicEvents[i].parent_Unparent);
                if (atom.atomicEvents[i].parent_Unparent == false)
                {
                    atom.atomicEvents[i].unparentForce_Radius = EditorGUILayout.Vector2Field("Unparent Force and Radius", atom.atomicEvents[i].unparentForce_Radius);
                }
            }
            else if (atom.atomicEvents[i].outputEvents.ToString() == "Change_Kinematic")
            {
                atom.atomicEvents[i].changeKinematic = EditorGUILayout.Toggle("Change Kinematic", atom.atomicEvents[i].changeKinematic);
            }
            else if (atom.atomicEvents[i].outputEvents.ToString() == "Hide_Unhide")
            {
                atom.atomicEvents[i].hide_Unhide = EditorGUILayout.Toggle("Hide or Unhide", atom.atomicEvents[i].hide_Unhide);
            }
            else if (atom.atomicEvents[i].outputEvents.ToString() == "Apply_Force")
            {
                EditorGUILayout.LabelField("This will not work if the object in question is a child, because it will not have a rigidbody");
                atom.atomicEvents[i].forceAmount = EditorGUILayout.FloatField("Apply Force", atom.atomicEvents[i].forceAmount);
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
