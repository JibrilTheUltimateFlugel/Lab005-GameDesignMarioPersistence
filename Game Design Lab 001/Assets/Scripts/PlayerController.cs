using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for UI stuff
using UnityEngine.SceneManagement; // to transition to Game Over Scene

public class PlayerController : MonoBehaviour
{
    // Variables
    public float speed;
    private Rigidbody2D marioBody;
    private BoxCollider2D marioCollider;
    public float maxSpeed = 10;
    public float upSpeed;

    // Boolean to check if mario is on the Ground
    private bool onGroundState = true;

    // For Flipping Mario Sprite
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    // Scoring Variables
    public Transform enemyLocation;
    public Text scoreText;

    // Sound Effects
    private AudioSource marioAudioSource;

    // Animation
    private Animator marioAnimator;

    // Particle System
    private ParticleSystem dustCloud;

    // Game Over and Restart
    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScene"); // go to the next scene which is the Game Over Scene
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        // Obtain the Mario sprite by getting the SpriteRenderer component from mario object
        marioSprite = GetComponent<SpriteRenderer>();
        // jump sound effect
        marioAudioSource = GetComponent<AudioSource>();
        marioAnimator = GetComponent<Animator>();
        marioCollider = GetComponent<BoxCollider2D>();
        dustCloud = GetComponentInChildren<ParticleSystem>();//get dustCloud particle system from child

        GameManager.OnPlayerDeath += PlayerDiesSequence; //subscribe to event OnPlayerDeath
    }

    void FixedUpdate()
    {
        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal"); // get input value from A and D keys
        if (Mathf.Abs(moveHorizontal) > 0) // check if there is movement in horizontal direction
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed) // check if magnitude of the velocity of the mario rigid body has not exceeded maxSpeed
                marioBody.AddForce(movement * speed);
        }

        if ((Input.GetKeyUp("a") || Input.GetKeyUp("d")) && onGroundState)// check if key is released
        {
            // stop
            marioBody.velocity = new Vector2(0,marioBody.velocity.y);
        }

        // Jumping Logic
        if (Input.GetKeyDown("space") && onGroundState && Mathf.Abs(marioBody.velocity.y) < 0.01f)
        {
            //marioJumpSFX.Play();
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false; // Mario is now in the air until he lands back on the ground
            // for jumping animation, set onGround to false to transition to the jump animation
            marioAnimator.SetBool("onGround", onGroundState);
            PlayJumpSound(); //call and play the jumping SFX
        }
    }

    // Update is called once per frame, does not involve Physics
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
        // Flipping Logic - does not require physics
        if (Input.GetKeyDown("a") && faceRightState) //If A key is pressed and Mario is facing right, change the sprite into facing left by enabling flipX, set faceRightState to false
        {
            faceRightState = false;
            marioSprite.flipX = true;

            // add skidding logic
            if (Mathf.Abs(marioBody.velocity.x) > 1.0)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        if (Input.GetKeyDown("d") && !faceRightState) //IF D key is pressed and Mario is facing left (not facing right), change the sprite into facing right and disable the flipX and set the faceRightstate to True
        {
            faceRightState = true;
            marioSprite.flipX = false;

            // add skidding logic
            if (Mathf.Abs(marioBody.velocity.x) > 1.0)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        // z button input to use first powerup slot 0
        if (Input.GetKeyDown("z"))
        {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z, this.gameObject); //it will call consumePowerup in CentralManager, and pass the reference of itself. This will eventually reach PowerupManager�fs consumePowerup
        }

        // x button input to use second powerup slot 1
        if (Input.GetKeyDown("x"))
        {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.X, this.gameObject);
        }

        // set the xSpeed of the animator to be the abs value of mario's rigidbody velocity
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
    }

    // Detect collision with the Ground
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            onGroundState = true; // back on ground
            // change from jumping state to idle or running state when mario is back on the ground
            marioAnimator.SetBool("onGround", onGroundState);
            dustCloud.Play(); //plays dust cloud particle system
        }

        // For bricks and misc obstacles
        if (col.gameObject.CompareTag("Obstacles") && Mathf.Abs(marioBody.velocity.y) < 0.01f)  //check if Y vertical velocity of the mario rigid body is 0
        {//apply the same logic as landing on ground
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
            dustCloud.Play(); //plays dust cloud particle system
        }
    }

    // OnTriggerEnter, triggered when Mario collider collides with the Goomba collider which is a Trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (other.gameObject.CompareTag("Enemy")) // if mario collides with an "Enemy"
        {
            Debug.Log("Collided with Gomba!"); // print
            GameOver();
        }
        */

        if (other.gameObject.CompareTag("Fire")) // if mario collides with an "Enemy"
        {
            Debug.Log("Collided with Fire!"); // print
            CentralManager.centralManagerInstance.damagePlayer();
        }
    }

    // Function to play jumping sound
    void PlayJumpSound()
    {
        marioAudioSource.PlayOneShot(marioAudioSource.clip);
    }

    // Method when player dies
    void PlayerDiesSequence()
    {
        // Mario dies
        Debug.Log("Mario dies");
        marioAnimator.SetBool("isAlive", false); //set the isAlive property in mario animator to false
        marioBody.constraints = RigidbodyConstraints2D.FreezePositionX; //freeze motion along x axis
        marioBody.AddForce(new Vector2(0, marioBody.mass * 50), ForceMode2D.Impulse); //add impulse force upwards
        marioCollider.enabled = false; //disable the mario box collider 2D to make him fall into oblivion
    }
}
