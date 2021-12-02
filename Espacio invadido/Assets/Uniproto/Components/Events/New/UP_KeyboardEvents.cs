using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


public class UP_KeyboardEvents : MonoBehaviour {

    [System.Serializable]
    struct Event
    {
        public string name;
        public bool isExpanded;        
        public float timeBetweenEvents;
        public UP_NoArgsUnityEvent onEvent;
        [HideInInspector] public float time;

        public Event(string name)
        {
            this.name = name;
            isExpanded = false;
            timeBetweenEvents = 0;
            time = 0;
            onEvent = new UP_NoArgsUnityEvent();
        }
    }
      

    [SerializeField] string key = "space";
    [SerializeField] Event onKeyDown = new Event("On Key Down");
    [SerializeField] Event onKey = new Event("On Key");
    [SerializeField] Event onKeyUp = new Event("On Key Up");

    void Update ()
    {
        onKeyDown.time = Mathf.Max(0, onKeyDown.time - Time.deltaTime);
        onKeyUp.time = Mathf.Max(0, onKeyUp.time - Time.deltaTime);

        if (onKeyDown.time == 0 && Input.GetKeyDown(key))
        {
            onKeyDown.time = onKeyDown.timeBetweenEvents;
            onKeyDown.onEvent.Invoke();
        }
        if (onKeyUp.time == 0 && Input.GetKeyUp(key))
        {
            onKeyUp.time = onKeyUp.timeBetweenEvents;
            onKeyUp.onEvent.Invoke();
        }        
	}

    private void FixedUpdate()
    {
        onKey.time = Mathf.Max(0, onKey.time - Time.deltaTime);

        if (onKey.time == 0 && Input.GetKey(key))
        {
            onKey.time = onKey.timeBetweenEvents;
            onKey.onEvent.Invoke();
        }
    }



#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UP_KeyboardEvents))]
    public class CustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UP_KeyboardEvents component = target as UP_KeyboardEvents;

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("key"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("onKeyDown"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onKey"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onKeyUp"));

            serializedObject.ApplyModifiedProperties();

        }
    }

    [UnityEditor.CustomPropertyDrawer(typeof(Event))]
    public class MessageEventCustomEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var name = property.FindPropertyRelative("name");
            var isExpanded = property.FindPropertyRelative("isExpanded");
            EditorGUI.indentLevel = 0;
            isExpanded.boolValue = EditorGUILayout.Foldout(isExpanded.boolValue, name.stringValue, true, EditorStyles.foldoutHeader);
            if (isExpanded.boolValue)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(property.FindPropertyRelative("timeBetweenEvents"));
                EditorGUILayout.PropertyField(property.FindPropertyRelative("onEvent"), new GUIContent(name.stringValue.Replace(" ", string.Empty)));
            }


        }
    }

#endif

}
