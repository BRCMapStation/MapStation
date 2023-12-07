using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using Winterland.Common;

[CustomEditor(typeof(EndSequenceAction))]
public class EndSequenceActionEditor : Editor {
    public override void OnInspectorGUI() {
        EditorGUILayout.LabelField("This action simply ends the sequence.");
        DrawDefaultInspector();
    }
}
