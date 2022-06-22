using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerupIndex //enumeration type to assign integer indexes to the different item types
{
    REDMUSHROOM = 0,
    GREENMUSHROOM = 1
}
public class PowerupManagerEV : MonoBehaviour
{
    // reference of all player stats affected
    public IntVariable marioJumpSpeed;
    public IntVariable marioMaxSpeed;
    public PowerupInventory powerupInventory;
    public List<GameObject> powerupIcons;

    void Start()
    {
        if (!powerupInventory.gameStarted) //check if the game is first time started or not, if it is the first time then reset the powerup icons, otherwise re-render from previous scene
        {
            powerupInventory.gameStarted = true;
            powerupInventory.Setup(powerupIcons.Count);
            resetPowerup();
        }
        else
        {
            // re-render the contents of the powerup from the previous time
            for (int i = 0; i < powerupInventory.Items.Count; i++)
            {
                Powerup p = powerupInventory.Get(i);
                if (p != null) //if powerup has been collected, add it to the UI otherwise it remains empty
                {
                    AddPowerupUI(i, p.powerupTexture);
                }
                else
                {
                    RemovePowerupUI(i);
                }
            }
        }
    }

    public void resetPowerup()
    {
        for (int i = 0; i < powerupIcons.Count; i++)
        {
            powerupIcons[i].SetActive(false);
        }
    }

    void AddPowerupUI(int index, Texture t)
    {
        powerupIcons[index].GetComponent<RawImage>().texture = t;
        powerupIcons[index].SetActive(true);
    }

    // method to remove powerup from UI
    void RemovePowerupUI(int index)
    {
        powerupIcons[index].SetActive(false); //set powerup icon at given index to be inactive
    }

    public void AddPowerup(Powerup p)
    {
        powerupInventory.Add(p, (int)p.index);
        AddPowerupUI((int)p.index, p.powerupTexture);
    }
    public void OnApplicationQuit()
    {
        ResetValues();
    }

    // Reset values function
    public void ResetValues()
    {
        powerupInventory.Clear(); //clear the Powerup Inventory and start with no powerups again
    }

    void castPowerup(int index)
    {
        if(powerupInventory.Get(index) != null) //if there exists a powerup with given index in the Powerup Inventory (either 0 or 1)
        {
            Debug.Log("Mario casted powerup");
            consumePowerup(powerupInventory.Get(index)); //consume powerup with given index
            powerupInventory.Remove(index); //remove powerup from powerup inventory
            RemovePowerupUI(index); //remove powerup from UI
        }
    }

    // Consume Powerup
    void consumePowerup(Powerup p)
    {
        Debug.Log("Effect starts now!");
        marioJumpSpeed.ApplyChange(p.absoluteJumpBooster); //apply + change to IntVariable marioJumpSpeed
        marioMaxSpeed.ApplyChange(p.aboluteSpeedBooster); //apply + change to the IntVariable marioMaxSpeed 
        StartCoroutine(removeEffect(p)); //start coroutine remove effect
    }

    // Coroutine to remove effect
    IEnumerator removeEffect(Powerup p)
    {
        yield return new WaitForSeconds(p.duration);
        removePowerupEffect(p); //call function to remove/undo powerup effect
        
    }

    // Function to remove effect
    void removePowerupEffect(Powerup p)
    {
        Debug.Log("Effect is now removed!");
        marioJumpSpeed.ApplyChange(-p.absoluteJumpBooster); //apply - change to IntVariable marioJumpSpeed
        marioMaxSpeed.ApplyChange(-p.aboluteSpeedBooster); //apply - change to the IntVariable marioMaxSpeed 
    }

    // Attempt to cast powerup
    public void AttemptConsumePowerup(KeyCode k)
    {
        switch (k)
        {
            case KeyCode.Z:
                castPowerup(0); //cast powerup at the first slot
                break;
            case KeyCode.X:
                castPowerup(1); //cast powerup at the second slot
                break;
            default:
                break;
        }
    }
}