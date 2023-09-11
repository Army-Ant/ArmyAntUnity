namespace ArmyAnt.Manager {
    /// <summary>
    /// 让面板属性只读
    /// </summary>
    public class InspectorReadOnly : UnityEngine.PropertyAttribute {

    }
#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer(typeof(InspectorReadOnly))]
    public class ReadOnlyDrawer : UnityEditor.PropertyDrawer {
        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            return UnityEditor.EditorGUI.GetPropertyHeight(property, label, true);
        }
        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            UnityEngine.GUI.enabled = false;
            UnityEditor.EditorGUI.PropertyField(position, property, label, true);
            UnityEngine.GUI.enabled = true;
        }
    }
#endif
}