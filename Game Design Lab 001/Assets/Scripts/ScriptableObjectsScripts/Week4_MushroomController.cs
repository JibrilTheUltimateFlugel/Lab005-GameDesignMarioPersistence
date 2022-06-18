using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for Mushroom
public class Week4_MushroomController : MonoBehaviour
{
    // Variables
    private Rigidbody2D mushroomBody; //mushrooms rigid body
    private int currentDirection; //current direction of the mushroom, similar idea to Enemy Goomba
    private float mushSpeed = 4; //mushroom's speeds
    private bool collected = false; // boolean to indicate mushroom has been collected by mario or not

    void Start()
    {
        mushroomBody = GetComponent<Rigidbody2D>();
        // Randomly set the currentDirection to be left or right with equal probability, source: https://gamedevbeginner.com/how-to-use-random-values-in-unity-with-examples/
        int leftOrRight = Random.Range(0, 2); //generates either 0 or 1
        if (leftOrRight == 0)
        {
            currentDirection = -1; // direction = left
        } else if (leftOrRight == 1)
        {
            currentDirection = 1; // direction = right
        }
        // Add initial impulse force upwards for "pop up from Box effect"
        mushroomBody.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (!collected)
        { //only if mushroom has not touched/collide and collected with mario
          // Movement Left and Right
            mushroomBody.velocity = new Vector2(currentDirection * mushSpeed, mushroomBody.velocity.y);
        }
    }

    // Collision Callback function
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacles Vertical")) // if the mushroom collides with an object with tag Obstacles Vertical (hint: pipe body or wall)
        {
            // Flip movement direction by multiplying it with -1
            currentDirection *= -1;
        }

        // Colliding with mario
        if (other.gameObject.CompareTag("Player")) //if the mushroom collides with mario
        {
            mushroomBody.velocity = Vector2.zero; //stop movement by setting velocity of the mushroom rigid body to 0
            collected = true; // once mushroom collides with mario, set collected to True, mushroom will no longer move
            Debug.Log("Mario collected the Mushroom");
        }
    }
}
