using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(Atom))]
public class AtomEditor : Editor
{
    private bool foldout = false;

#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Atom atom = (Atom)target;

        for (int i = 0; i < atom.atomicEvents.Length; i++)
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Trigger " + i);
            GUILayout.Space(2);

            #region Trigger Overrides
            if (atom.atomicEvents[i].triggerEvents.ToString() == "Collision")
            {
                //should add tooltip here for if equals to nothing, collide with anything
                atom.atomicEvents[i].collisionTag = EditorGUILayout.TextField("Collision " + " Tag", atom.atomicEvents[i].collisionTag);
            }
            #endregion

            GUILayout.Space(10);

            EditorGUILayout.LabelField("Output " + i);
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
            else if(atom.atomicEvents[i].outputEvents.ToString() == "Change_Color")
            {
                //this needs to be a slider from 1 to 2
                atom.atomicEvents[i].setColorBasedOnPosition = EditorGUILayout.IntSlider("Color Mode ", atom.atomicEvents[i].setColorBasedOnPosition, 1, 2);

                if (atom.atomicEvents[i].setColorBasedOnPosition == 1)
                {
                    atom.atomicEvents[i].fixedColor = EditorGUILayout.ColorField("Fixed Color ", atom.atomicEvents[i].fixedColor);
                }
                else
                {
                    atom.atomicEvents[i].colorVector = EditorGUILayout.Vector3Field("Color Based in Position ", atom.atomicEvents[i].colorVector);
                }
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
