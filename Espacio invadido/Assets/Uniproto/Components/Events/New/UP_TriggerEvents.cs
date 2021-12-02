using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class UP_TriggerEvents : MonoBehaviour
{
    
    [System.Serializable]
    struct Event
    {
        public string name;
        public bool isExpanded;
        public bool sendMessage;
        public string message;
        public float timeBetweenEvents;
        public UP_NoArgsUnityEvent onEvent;
        [HideInInspector] public float time;

        public Event(string name)
        {
            this.name = name;
            isExpanded = false;
            sendMessage = false;
            message = "";
            timeBetweenEvents = 0;
            time = 0;
            onEvent = new UP_NoArgsUnityEvent();
        }
    }

    [SerializeField] Event onTriggerEnter = new Event("On Trigger Enter");
    [SerializeField] Event onTriggerStay = new Event("On Trigger Stay");
    [SerializeField] Event onTriggerExit = new Event("On Trigger Exit");

    [SerializeField] bool filterByTag = false;
    [TagSelector, SerializeField] string filterTag = null;


    private void FixedUpdate()
    {
        onTriggerEnter.time = Mathf.Max(0, onTriggerEnter.time - Time.deltaTime);
        onTriggerStay.time = Mathf.Max(0, onTriggerStay.time - Time.deltaTime);
        onTriggerExit.time = Mathf.Max(0, onTriggerExit.time - Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ManageTriggerEvent(onTriggerEnter, other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        ManageTriggerEvent(onTriggerStay, other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        ManageTriggerEvent(onTriggerExit, other);
    }

    void ManageTriggerEvent(Event triggerEvent, Collider2D other)
    {
        if (triggerEvent.time == 0 && (!filterByTag || other.gameObject.CompareTag(filterTag)))
        {
            triggerEvent.time = triggerEvent.timeBetweenEvents;
            if (triggerEvent.onEvent != null) { triggerEvent.onEvent.Invoke(); }
            if (triggerEvent.sendMessage) { SendMessage(triggerEvent.message, other); }
        }
    }

    void SendMessage(string message, Collider2D collider)
    {
        Component target = collider.attachedRigidbody;
        if (target == null) { target = collider; }

        UP_AbstractMessageEvent[] messageEvents = target.GetComponentsInChildren<UP_AbstractMessageEvent>();
        for (int i = 0; i < messageEvents.Length; ++i)
        {
            messageEvents[i].ThrowMessageEvent(message);
        }
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UP_TriggerEvents))]
    public class CustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UP_TriggerEvents component = target as UP_TriggerEvents;

            serializedObject.Update();

            EditorGUILayout.LabelField("Filter", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("filterByTag"));
            if (component.filterByTag) { EditorGUILayout.PropertyField(serializedObject.FindProperty("filterTag")); }
                        
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onTriggerEnter"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onTriggerStay"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onTriggerExit"));

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
            if(isExpanded.boolValue)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(property.FindPropertyRelative("timeBetweenEvents"));
                SerializedProperty sendMessageProp = property.FindPropertyRelative("sendMessage");
                EditorGUILayout.PropertyField(sendMessageProp);
                if (sendMessageProp.boolValue) { EditorGUILayout.PropertyField(property.FindPropertyRelative("message")); }                
                
                EditorGUILayout.PropertyField(property.FindPropertyRelative("onEvent"), new GUIContent(name.stringValue.Replace(" ",string.Empty)));
            }

            
        }
    }

#endif

}
