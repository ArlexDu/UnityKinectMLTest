using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(HistogramTexture))]
public class HistogramTextureEditor : Editor {

	override public void OnInspectorGUI(){
		var myscript = target as HistogramTexture;
		myscript.iscanvas = EditorGUILayout.Toggle ("Is Canvas",myscript.iscanvas);
		if (!myscript.iscanvas) {
			EditorGUILayout.ObjectField("material",myscript.material,typeof(Material));
		}
		myscript.textureWidth = EditorGUILayout.IntField ("Texture Width",myscript.textureWidth);
		myscript.rectangleColor = EditorGUILayout.ColorField ("Rectangle Color",myscript.rectangleColor);
		myscript.Count = EditorGUILayout.IntField ("Count",myscript.Count);
	}
}
