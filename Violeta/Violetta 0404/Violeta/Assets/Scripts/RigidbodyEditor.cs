﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Rigidbody))]
public class RigidbodyEditor : Editor {


	void OnSceneGUI () {
        Rigidbody rb = target as Rigidbody;
        Handles.color = Color.red;
        Handles.SphereCap(1, rb.transform.TransformPoint(rb.centerOfMass), rb.rotation, 0.0000002f);
	
	}
	
    public override void OnInspectorGUI () {
        GUI.skin =
            EditorGUIUtility.GetBuiltinSkin(UnityEditor.EditorSkin.Inspector);
        DrawDefaultInspector();
	
	}
}
