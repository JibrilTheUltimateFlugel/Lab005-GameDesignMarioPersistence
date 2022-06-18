using System.Collections;
using UnityEngine;

public class GreenMushroom : MonoBehaviour, ConsumableInterface //implements consumable interface
{
	public Texture t;
	public void consumedBy(GameObject player)
	{
		// give player speed boost 2x max speed
		player.GetComponent<PlayerController>().speed *= 2;
		player.GetComponent<PlayerController>().maxSpeed *= 2;
		StartCoroutine(removeEffect(player));
	}

	// Coroutine
	IEnumerator removeEffect(GameObject player)
	{
		yield return new WaitForSeconds(5.0f); //5 seconds until yield
		player.GetComponent<PlayerController>().speed /= 2;
		player.GetComponent<PlayerController>().maxSpeed /= 2;
	}

	// when green mushroom collides with mario and collected
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			// update UI
			CentralManager.centralManagerInstance.addPowerup(t, 1, this); //add powerup by calling the Central Manager, pass reference of texture t, to slot with index 1
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
