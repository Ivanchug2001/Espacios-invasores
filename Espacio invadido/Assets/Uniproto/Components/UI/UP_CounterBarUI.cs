using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UP_CounterBarUI : MonoBehaviour
{

    [SerializeField] UP_Counter counter = null;
    [SerializeField] Image barImage = null;

    void Update()
    {
        float value = counter.Counter - counter.Minimum;
        barImage.fillAmount = Mathf.Clamp01(value / counter.Maximum);
    }
}
