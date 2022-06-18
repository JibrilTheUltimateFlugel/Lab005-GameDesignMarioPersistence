using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBrick : MonoBehaviour
{
    private bool brickBroken = false; //boolean to indicate if brick has been broken, starts false
    public GameObject brickDebris; // reference to the debris prefab
    // Sound Effects
    private AudioSource brickAudioSource;
    public GameObject coinGold; // reference to coin prefab

    private void Start()
    {
        // brick sound effect
        brickAudioSource = GetComponent<AudioSource>();
        Debug.Log(brickAudioSource);
    }

    void OnTriggerEnter2D(Collider2D col) // OnTriggerCallback
    {
        if (col.gameObject.CompareTag("Player") && !brickBroken) //if breakable brick collides with "Player" and 
        {
            brickAudioSource.PlayOneShot(brickAudioSource.clip);
            brickBroken = true; //the brick is now broken
            // Spawn coin prefab on top of box by 2.0 float points
            Instantiate(coinGold, new Vector3(this.transform.position.x, this.transform.position.y + 2.0f, this.transform.position.z), Quaternion.identity);
            // assume we have 5 debris per box
            for (int x = 0; x < 5; x++)
            {
                Instantiate(brickDebris, transform.position, Quaternion.identity); //instantiate the brickDebris prefab at the brick's position
            }
            gameObject.transform.parent.GetComponent<SpriteRenderer>().enabled = false; //disable the sprite renderer of the parent which is the one with Breakable Brick sprite renderer
            gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled = false; //disable box collider 2D of breakable brick
            GetComponent<EdgeCollider2D>().enabled = false; //disable own edge collider
        }
    }
}
