using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManagerEV : MonoBehaviour
{
    public UnityEvent onApplicationExit;
    void OnApplicationQuit()
    {
        onApplicationExit.Invoke(); //invoke onApplicationExit Event when application is quit
    }
}