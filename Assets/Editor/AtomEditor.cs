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
            if (atom.atomicEvents[i].triggerEvents.ToString() == "Collision")
            {
                //should add tooltip here for if equals to 0, collide with anything
                atom.atomicEvents[i].collisionTag = EditorGUILayout.TextField("Collision " + i + " Tag", atom.atomicEvents[i].collisionTag);
            }

            if (atom.atomicEvents[i].outputEvents.ToString() == "Change_Scale")
            {
                atom.atomicEvents[i].scaleAmount = EditorGUILayout.Vector3Field("Change Scale " + i, atom.atomicEvents[i].scaleAmount);
            }
        }
    }
    
#endif
}
