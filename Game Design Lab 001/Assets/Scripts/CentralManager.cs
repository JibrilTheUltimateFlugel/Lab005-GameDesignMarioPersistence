using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this has methods callable by players
public class CentralManager : MonoBehaviour
{
	public GameObject gameManagerObject; //reference to game object with Game Manager attached to it
	private GameManager gameManager;
	public static CentralManager centralManagerInstance; //static variable to be accessed and refered by other scripts

	// add reference to PowerupManager
	public GameObject powerupManagerObject;
	private PowerUpManager powerUpManager;

	// add references to SpawnManager
	public GameObject spawnManagerObject;
	private SpawnManager spawnManager;

	void Awake()
	{
		centralManagerInstance = this;
	}
	// Start is called before the first frame update
	void Start()
	{
		gameManager = gameManagerObject.GetComponent<GameManager>();
		// instantiate in start
		powerUpManager = powerupManagerObject.GetComponent<PowerUpManager>();
		spawnManager = spawnManagerObject.GetComponent<SpawnManager>();
	}

	// Increase score method
	public void increaseScore()
	{
		gameManager.increaseScore();
	}

	// Damage player
	public void damagePlayer()
	{
		gameManager.damagePlayer();
	}

	public void consumePowerup(KeyCode k, GameObject g)
	{
		powerUpManager.consumePowerup(k, g);
	}

	public void addPowerup(Texture t, int i, ConsumableInterface c)
	{
		powerUpManager.addPowerup(t, i, c);
	}

	// Method to spawn new enemies
	public void newEnemy()
    {
		spawnManager.spawnNewEnemy(); //simply call spawnNewEnemy in the spawnManager
    }

	// Play Sound Effects
	public void CentralCoinSFX()
    {
		gameManager.CentralCoinSFX();
    }
}