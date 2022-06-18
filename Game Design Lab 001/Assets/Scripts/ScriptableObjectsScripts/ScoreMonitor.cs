using UnityEngine.UI;
using UnityEngine;

public class ScoreMonitor : MonoBehaviour
{
    public IntVariable marioScore; //scriptable object
    public Text text; //the score text UI

    public void Start()
    {
        UpdateScore(); //update score at the start of every scene
    }
    public void UpdateScore()
    {
        text.text = "Score: " + marioScore.Value.ToString();
    }
}