using System.Collections;
using UnityEngine;

public class RedMushroom : MonoBehaviour, ConsumableInterface //this class implements the ConsumableInterface
{
	public Texture t;
	public void consumedBy(GameObject player)
	{
		// give player jump boost, increase the upSpeed
		player.GetComponent<PlayerController>().upSpeed += 20;
		StartCoroutine(removeEffect(player));
	}

	// Coroutine
	IEnumerator removeEffect(GameObject player)
	{
		yield return new WaitForSeconds(5.0f); //wait for 5 seconds before yielding
		player.GetComponent<PlayerController>().upSpeed -= 20; //return speed back to normal
	}

	// when the red mushroom powerup collides with Mario (Player)
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			// update UI
			CentralManager.centralManagerInstance.addPowerup(t, 0, this); //add powerup by calling the Central Manager, pass reference of texture t, to slot index 0
			GetComponent<Collider2D>().enabled = false;
			StartCoroutine(eaten()); //start eaten coroutine
		}
	}


	//coroutine for mushroom getting eaten
	IEnumerator eaten()
	{
		Debug.Log("Eaten starts");
		int steps = 5;
		float stepper = 1.0f / (float)steps;

		for (int i = 0; i < steps; i++)
		{
			this.transform.localScale = new Vector3(this.transform.localScale.x - stepper, this.transform.localScale.y - stepper, this.transform.localScale.z); //scale down the x and y scale
			yield return null;
		}
		Debug.Log("Eaten ends");
		yield break;
	}
}