using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class UP_Counter : MonoBehaviour {

    [System.Serializable]
    struct RandomRange
    {
        public RandomRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public int min;
        public int max;
    }

    public enum Comparison
    {
        LessOrEqual,
        Equal,
        GreaterOrEqual        
    }

    [SerializeField] int counter;
    [SerializeField] int multiplier = 1;
    [SerializeField] bool useMaximum = false;
    [SerializeField] int maximum = 10;
    [SerializeField] bool useMinimum = false;
    [SerializeField] int minimum = 0;
    [SerializeField] bool alertIfReachesValue = false;
    [SerializeField] bool randomizeTargetValue = false;
    [SerializeField] int targetValue = 0;
    [SerializeField] RandomRange randomTargetValue = new RandomRange(0, 10);
    [SerializeField] Comparison comparison = Comparison.Equal;
    [SerializeField] UP_NoArgsUnityEvent onValueReached = null;

    public int Maximum
    {
        get { return maximum; }
    }

    public int Minimum
    {
        get { return minimum; }
    }

    public int Counter
    {
        get { return counter; }
    }

    private void Start()
    {
        UP_RandomizeTargetValue();
    }

    public void UP_SetMultiplier(int value)
    {
        this.multiplier = value;
    }

    public void UP_IncreaseMultiplier(int increase)
    {
        multiplier += increase;
    }

    public void UP_SetCounter(int value)
    {
        this.counter = value;
        if (useMinimum) { counter = Mathf.Max(counter, minimum); }
        if (useMaximum) { counter = Mathf.Min(counter, maximum); }
        CheckAlert();
    }

    public void UP_IncreaseCounter(int increase)
    {
        counter += increase * multiplier;
        if (useMinimum) { counter = Mathf.Max(counter, minimum); }
        if (useMaximum) { counter = Mathf.Min(counter, maximum); }
        CheckAlert();
    }
    
    public void UP_SetTargetValue(int targetValue)
    {
        this.targetValue = targetValue;        
        CheckAlert();
    }

    public void UP_RandomizeTargetValue()
    {
        if(randomizeTargetValue)
        {
            targetValue = Random.Range(randomTargetValue.min, randomTargetValue.max + 1);
        }
        
    }

    private void CheckAlert()
    {
        if (alertIfReachesValue)
        {
            switch (comparison)
            {
                case Comparison.Equal:
                    if (counter == targetValue) { SendEvent(); }
                    break;
                case Comparison.GreaterOrEqual:
                    if (counter >= targetValue) { SendEvent(); }
                    break;
                case Comparison.LessOrEqual:
                    if (counter <= targetValue) { SendEvent(); }
                    break;
            }
        }
    }

    void SendEvent()
    {
        if (onValueReached != null) { onValueReached.Invoke(); }
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UP_Counter))]
    public class CustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UP_Counter cont = target as UP_Counter;

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("counter"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("multiplier"));

            EditorGUILayout.LabelField("Limits", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useMaximum"));
            if(cont.useMaximum)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("maximum"));
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useMinimum"));
            if (cont.useMinimum)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("minimum"));
            }
            EditorGUILayout.LabelField("Alert", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("alertIfReachesValue"));
            if (cont.alertIfReachesValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("randomizeTargetValue"));
                if(cont.randomizeTargetValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("randomTargetValue"));
                }
                else
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("targetValue"));
                }
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty("comparison"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onValueReached"));                
            }

            serializedObject.ApplyModifiedProperties();

        }
    }

#endif

}
