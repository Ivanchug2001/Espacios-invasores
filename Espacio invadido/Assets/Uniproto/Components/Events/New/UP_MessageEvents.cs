using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UP_MessageEvents : UP_AbstractMessageEvent
{

    [System.Serializable]
    public class Event
    {
        public string message;
        public bool disabled = false;        
        public UP_NoArgsUnityEvent onMessageReceived;
    }

    [SerializeField] Event[] messageEvents;

    public void UP_EnableMessage(string message)
    {
        for (int i = 0; i < messageEvents.Length; i++)
        {
            var m = messageEvents[i];
            if (!string.IsNullOrEmpty(m.message) && m.message == message)
            {
                m.disabled = false;
            }
        }
    }

    public void UP_DisableMessage(string message)
    {
        for (int i = 0; i < messageEvents.Length; i++)
        {
            var m = messageEvents[i];
            if (!string.IsNullOrEmpty(m.message) && m.message == message)
            {
                m.disabled = true;
            }
        }
    }

    internal override void ThrowMessageEvent(string message)
    {
        if(this.isActiveAndEnabled)
        {
            for (int i = 0; i < messageEvents.Length; i++)
            {
                var m = messageEvents[i];
                if (!string.IsNullOrEmpty(m.message) && m.message == message && !m.disabled)
                {
                    if(m.onMessageReceived != null)
                    {
                        m.onMessageReceived.Invoke();
                    }
                }

            }
        }
    }
    
}
