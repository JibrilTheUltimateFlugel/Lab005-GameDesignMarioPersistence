using System.Collections;
using UnityEngine;


public class PlayerControllerEV : MonoBehaviour
{
    private float force;
    public IntVariable marioUpSpeed; //references to IntVariable (scriptable object)
    public IntVariable marioMaxSpeed;
    public GameConstants gameConstants;

    // other components and internal state
    private Animator marioAnimator;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    // Sound Effects
    private AudioSource marioAudioSource;
    // Particle System
    private ParticleSystem dustCloud;

    public CustomCastEvent onPlayerCastPowerup;

    // boolean variables
    private bool isDead; //whether mario is dead or alive
    private bool isADKeyUp; //check if A or D key is pressed or not
    private bool isSpacebarUp; //check if spacebar is pressed or not
    private bool onGroundState; //whether mario is on the ground or not
    private bool faceRightState; //whether mario is facing right or not

    void Start()
    {
        //get components
        marioAudioSource = GetComponent<AudioSource>();
        marioAnimator = GetComponent<Animator>();
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        dustCloud = GetComponentInChildren<ParticleSystem>();//get dustCloud particle system from child

        //initial values for boleans
        isDead = false;
        isADKeyUp = true;
        isSpacebarUp = true;
        onGroundState = true;
        faceRightState = true;

        // set values based on game constants
        marioUpSpeed.SetValue(gameConstants.playerMaxJumpSpeed);
        marioMaxSpeed.SetValue(gameConstants.playerMaxSpeed);
        force = gameConstants.playerDefaultForce;
    }

    void Update()
    {
        //AD Key Up is true only if neither of A key or D key is pressed/held down
        isADKeyUp = (!Input.GetKey("a") && !Input.GetKey("d"));
        //Spacebar Up is true if spacebar is not pressed/held down
        isSpacebarUp = !Input.GetKey("space");

        // Flipping Logic - does not require physics
        if (Input.GetKeyDown("a") && faceRightState) //If A key is pressed and Mario is facing right, change the sprite into facing left by enabling flipX, set faceRightState to false
        {
            // add skidding logic
            if (Mathf.Abs(marioBody.velocity.x) > 1.0)
            {
                marioAnimator.SetTrigger("onSkid");
            }
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRightState) //IF D key is pressed and Mario is facing left (not facing right), change the sprite into facing right and disable the flipX and set the faceRightstate to True
        {
            // add skidding logic
            if (Mathf.Abs(marioBody.velocity.x) > 1.0)
            {
                marioAnimator.SetTrigger("onSkid");
            }
            faceRightState = true;
            marioSprite.flipX = false;
        }

        // Casting powerup
        if (Input.GetKeyDown("z"))
        {
            onPlayerCastPowerup.Invoke(KeyCode.Z);
        }

        if (Input.GetKeyDown("x"))
        {
            onPlayerCastPowerup.Invoke(KeyCode.X);
        }

        // set the xSpeed of the animator to be the abs value of mario's rigidbody velocity
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
    }

    // Fixed Update
    void FixedUpdate()
    {
        if (!isDead)
        {
            //check if a or d is pressed currently
            if (!isADKeyUp)
            {
                float direction = faceRightState ? 1.0f : -1.0f;
                Vector2 movement = new Vector2(force * direction, 0);
                if (Mathf.Abs(marioBody.velocity.x) < marioMaxSpeed.Value)
                    marioBody.AddForce(movement);
            }

            if (!isSpacebarUp && onGroundState)
            {
                marioBody.AddForce(Vector2.up * marioUpSpeed.Value, ForceMode2D.Impulse);
                onGroundState = false;
                // part 2
                marioAnimator.SetBool("onGround", onGroundState);
            }
        }
    }

    void PlayJumpSound()
    {
        marioAudioSource.PlayOneShot(marioAudioSource.clip);
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

    // Response to be called when Raise is called
    public void PlayerDiesSequence()
    {
        isDead = true;
        marioAnimator.SetBool("isAlive", false); //mario is now DEAD
        GetComponent<Collider2D>().enabled = false;
        marioBody.constraints = RigidbodyConstraints2D.FreezePositionX; //freeze motion along x axis
        marioBody.AddForce(new Vector2(0, marioBody.mass * 50), ForceMode2D.Impulse); //add impulse force upwards
        marioBody.gravityScale = 30;
        StartCoroutine(dead());
    }

    IEnumerator dead()
    {
        yield return new WaitForSeconds(1.0f);
        marioBody.bodyType = RigidbodyType2D.Static;
    }
}
