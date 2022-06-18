using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class CustomPowerupEvent : UnityEvent<Powerup>
{
}

public class PowerupEventListener : MonoBehaviour //to subsscribe to Powerup Events Scriptable Object which has one parameter to pass
{
    public PowerupEvent Event;
    public CustomPowerupEvent Response;
    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Powerup p)
    {
        Response.Invoke(p);
    }
}