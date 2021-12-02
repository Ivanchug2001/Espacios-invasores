using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


public class UP_MouseEvents : MonoBehaviour {

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


    public enum MouseButton
    {
        LeftClick,
        RightClick,
        MiddleClick
    }

    [SerializeField] MouseButton button = MouseButton.LeftClick;
    [SerializeField] Event onMouseButtonDown = new Event("On Mouse Button Down");
    [SerializeField] Event onMouseButton = new Event("On Mouse Button");
    [SerializeField] Event onMouseButtonUp = new Event("On Mouse Button Up");

    void Update ()
    {
        onMouseButtonDown.time = Mathf.Max(0, onMouseButtonDown.time - Time.deltaTime);
        onMouseButtonUp.time = Mathf.Max(0, onMouseButtonUp.time - Time.deltaTime);

        if (onMouseButtonDown.time == 0 && Input.GetMouseButtonDown((int)button))
        {
            onMouseButtonDown.time = onMouseButtonDown.timeBetweenEvents;
            onMouseButtonDown.onEvent.Invoke();
        }
        if (onMouseButtonUp.time == 0 && Input.GetMouseButtonUp((int)button))
        {
            onMouseButtonUp.time = onMouseButtonUp.timeBetweenEvents;
            onMouseButtonUp.onEvent.Invoke();
        }        
	}

    private void FixedUpdate()
    {
        onMouseButton.time = Mathf.Max(0, onMouseButton.time - Time.deltaTime);

        if (onMouseButton.time == 0 && Input.GetMouseButton((int)button))
        {
            onMouseButton.time = onMouseButton.timeBetweenEvents;
            onMouseButton.onEvent.Invoke();
        }
    }



#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UP_MouseEvents))]
    public class CustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UP_MouseEvents component = target as UP_MouseEvents;

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("button"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("onMouseButtonDown"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onMouseButton"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onMouseButtonUp"));

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
