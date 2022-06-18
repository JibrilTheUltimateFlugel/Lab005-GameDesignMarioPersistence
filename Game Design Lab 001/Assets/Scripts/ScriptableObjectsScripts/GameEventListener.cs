using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this); //register the object this script is attached to as a subscriber to the Event
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this); //unregister the object this script is attached to as subscriber to the event
    }

    public void OnEventRaised()
    {
        Response.Invoke(); //invoke the methods in the Response Box
    }
}