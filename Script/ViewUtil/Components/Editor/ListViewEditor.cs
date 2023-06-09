﻿using UnityEditor;
using ArmyAnt.ViewUtil.Components;

namespace ArmyAnt.ViewUtil.Components.Editor {

    [CustomEditor(typeof(ListView), true)]
    public class ListViewEditor : UnityEditor.UI.ScrollRectEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            ListView listview = target as ListView;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            listview.DefaultElement = EditorGUILayout.ObjectField("Default element", listview.defaultElement, typeof(UnityEngine.RectTransform), true) as UnityEngine.RectTransform;
            Undo.RecordObject(listview, "listview change");
            EditorGUILayout.EndVertical();
        }
    }

}