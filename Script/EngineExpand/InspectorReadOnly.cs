using UnityEditor;
using UnityEngine;

namespace ArmyAnt.Manager {
    /// <summary>
    /// ��������ֻ��
    /// </summary>
    public class InspectorReadOnly : PropertyAttribute {

    }

    [CustomPropertyDrawer(typeof(InspectorReadOnly))]
    public class ReadOnlyDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}