using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public Text score;
	private int playerScore = 0;

	public Transform maincamera;
	public delegate void gameEvent(); // declare a delegate as as reference pointer to a method
	public static event gameEvent OnPlayerDeath; // create instance of delegate with "event" keyword, to allow other scrips to subscribe to it

	//death sound effect
	private AudioSource deathSFX; //for marios death sound

	public GameObject coinSound; // object containing coin sound effect
	private AudioSource coinSFX; // Audio source of object containing coin sound effect

	private void Start()
    {
		deathSFX = GetComponent<AudioSource>();
		coinSFX = coinSound.GetComponent<AudioSource>();
    }
    public void increaseScore() //method to increase score, declared in Game Manager
	{
		playerScore += 1;
		score.text = "SCORE: " + playerScore.ToString();
	}

	public void damagePlayer() // when damagePlayer() is called in GameManager.cs, this will cast the delegate OnPlayerDeath(), which in turn will call EnemyRejoice() and PlayerDiesSequence() both since they are subscribed to the event
	{
		maincamera.GetComponent<AudioSource>().Stop();
		deathSFX.PlayOneShot(deathSFX.clip);
		OnPlayerDeath(); //it is an event, of which all of its subscribers will cast the subscribed method whenever GameManager calls OnPlayerDeath().
	}

	// Function to play Coin Sound Effects
	public void CentralCoinSFX()
    {
		coinSFX.PlayOneShot(coinSFX.clip);
    }
}
