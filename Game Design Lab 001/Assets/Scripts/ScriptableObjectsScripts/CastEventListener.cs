using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class CustomCastEvent : UnityEvent<KeyCode>
{
}

public class CastEventListener : MonoBehaviour //to subsscribe to Cast Events Scriptable Object which has one parameter to pass
{
    public CastEvent Event;
    public CustomCastEvent Response;
    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(KeyCode k)
    {
        Response.Invoke(k);
    }
}