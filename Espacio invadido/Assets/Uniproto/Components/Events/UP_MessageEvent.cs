using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UP_AbstractMessageEvent : MonoBehaviour
{
    internal abstract void ThrowMessageEvent(string message);
    
}

public class UP_MessageEvent : UP_AbstractMessageEvent {

    [SerializeField] string message = null;
    [SerializeField] UP_NoArgsUnityEvent onMessageReceived = null;

    internal override void ThrowMessageEvent(string mensaje)
    {
        if (this.isActiveAndEnabled && this.message == mensaje)
        {
            onMessageReceived.Invoke();
        }
    }

}
