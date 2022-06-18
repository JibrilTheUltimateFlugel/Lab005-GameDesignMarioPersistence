using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
	public List<GameObject> powerupIcons; //list of powerUp icons
	private List<ConsumableInterface> powerups; //list of powerups consumable interface

	// For Audio
	private AudioSource mushroomSound;

	// Start is called before the first frame update
	void Start()
	{
		mushroomSound = GetComponent<AudioSource>();
		powerups = new List<ConsumableInterface>();
		for (int i = 0; i < powerupIcons.Count; i++)
		{
			powerupIcons[i].SetActive(false);
			powerups.Add(null);
		}
	}

	// Add powerup to the list upon collection
	public void addPowerup(Texture texture, int index, ConsumableInterface i)
	{
		mushroomSound.PlayOneShot(mushroomSound.clip);
		Debug.Log("adding powerup");
		if (index < powerupIcons.Count)
		{
			powerupIcons[index].GetComponent<RawImage>().texture = texture; 
			powerupIcons[index].SetActive(true); //enable active on powerup icon
			powerups[index] = i;
		}
	}

	// Remove powerup from list when used
	public void removePowerup(int index)
	{
		if (index < powerupIcons.Count)
		{
			powerupIcons[index].SetActive(false); //disable the powerup icon
			powerups[index] = null;
		}
	}

	// Cast the powerup depending on the given integer index and remove that powerup from the list
	void cast(int i, GameObject p)
	{
		if (powerups[i] != null)
		{
			powerups[i].consumedBy(p); // interface method, implemented by any class having this ConsumableInterface
			removePowerup(i); //remove powerup with index i from list
		}
	}

	// Consume the powerup method based on key press
	public void consumePowerup(KeyCode k, GameObject player)
	{
		switch (k)
		{
			case KeyCode.Z: //if the Z key is pressed, use powerup index 0
				cast(0, player);
				break;
			case KeyCode.X: //if the X key is pressed, use powerup index 1
				cast(1, player);
				break;
			default:
				break;
		}
	}
}