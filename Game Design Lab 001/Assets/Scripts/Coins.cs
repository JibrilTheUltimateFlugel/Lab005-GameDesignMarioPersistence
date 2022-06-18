using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    // When coin is collected/collided with mario
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            CentralManager.centralManagerInstance.CentralCoinSFX(); // Function to play coin SFX by calling method coinSFX() in Central Manager which will call coinSFX() in Game Manager
            // Call increaseScore in the Central Manager
            CentralManager.centralManagerInstance.increaseScore();
            // Spawn 1 new enemy upon collection of coin
            CentralManager.centralManagerInstance.newEnemy();
            gameObject.SetActive(false); //disable the Coin game object
        }
    }
}
