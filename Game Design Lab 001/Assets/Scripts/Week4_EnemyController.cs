using System.Collections;
using UnityEngine;

public class Week4_EnemyController : MonoBehaviour
{
	public GameConstants gameConstants;
	private int moveRight;
	private float originalX;
	private Vector2 velocity;
	private Rigidbody2D enemyBody;
	private SpriteRenderer enemySprite; // for flipping enemy sprite
	private bool marioAlive = true; //mario is alive at the beginning
	private bool enemyAlive = true; //enemy is alive at the beginning/when activated

	private void OnEnable() //when enemy object is activated
    {
		enemyAlive = true;
		originalX = transform.position.x; //change the original X to the new spawned position x, otherwise it will use the Original X position and Keep Flipping!
	}

    void Start()
	{
		enemyBody = GetComponent<Rigidbody2D>();
		enemySprite = GetComponent<SpriteRenderer>(); //get sprite renderer component

		// get the starting position
		originalX = transform.position.x;

		// randomise initial direction
		moveRight = Random.Range(0, 2) == 0 ? -1 : 1;

		// compute initial velocity
		ComputeVelocity();

		// subscribe to player event
		GameManager.OnPlayerDeath += EnemyRejoice;
	}

	void ComputeVelocity()
	{
		velocity = new Vector2((moveRight) * gameConstants.maxOffset / gameConstants.enemyPatroltime, 0); //velocity in the x direction is the direction (moveRight) multiplied by supposed travel distance/patrol time
	}

	void MoveEnemy()
	{
		enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
	}

	void changeSprite() //function to flip enemy sprite
    {
		if (moveRight == 1)
		{
			enemySprite.flipX = false;
		}
		else
		{
			enemySprite.flipX = true;
		}
	}

	void Update()
	{
		if (marioAlive) //if mario is still alive
		{
			if (Mathf.Abs(enemyBody.position.x - originalX) < gameConstants.maxOffset)
			{// move gomba
				changeSprite();
				MoveEnemy();
			}
			else
			{
				// change direction
				moveRight *= -1;
				changeSprite();
				ComputeVelocity();
				MoveEnemy();
			}
        }
        else
        {
			// change sprite flipping direction
			moveRight *= -1;
			changeSprite();
		}
	}

	// When mario collides with the enemy, a Trigger callback will happen
	void OnTriggerEnter2D(Collider2D other)
	{
		// check if it collides with Mario with the Player tag
		if (other.gameObject.tag == "Player")
		{
			// check if collides on top
			float yoffset = (other.transform.position.y - this.transform.position.y); //The idea is to check if the playerfs y location is higher than the enemyfs, If yes, then we the enemy is killed
			if (yoffset > 0.75f && enemyAlive)
			{
				enemyAlive = false; //enemy is now dead
				KillSelf(); //enemy is killed
				CentralManager.centralManagerInstance.newEnemy(); //spawn new enemy
			}
			else if (yoffset < 0.75f && enemyAlive)
			{
				// hurt player, call damagePlayer in Game Manager which will cast delegate OnPlayerDeath which will call EnemyRejoice and PlayerDiesSequence
				CentralManager.centralManagerInstance.damagePlayer();
			}
		}
	}

	// function to kill enemy self
	void KillSelf()
	{
		// enemy dies
		CentralManager.centralManagerInstance.increaseScore(); //add the score since Mario kills an enemy
		StartCoroutine(flatten());
		Debug.Log("Kill sequence ends");
	}

	// Coroutine for flatten, gradually animate the enemy being flattened onto the ground
	IEnumerator flatten()
	{
		Debug.Log("Flatten starts");
		int steps = 5;
		float stepper = 1.0f / (float)steps;

		for (int i = 0; i < steps; i++)
		{
			this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y - stepper, this.transform.localScale.z); //change the y scale of the enemy to "flatten" enemy

			// make sure enemy is still above ground
			this.transform.position = new Vector3(this.transform.position.x, 0 + GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield return null;
		}
		Debug.Log("Flatten ends");
		this.gameObject.SetActive(false); //enemy is disabled, and returned to pool 
		Debug.Log("Enemy returned to pool");
		yield break;
	}

	// Rejoice animation when player is dead
	void EnemyRejoice()
	{
		Debug.Log("Enemy killed Mario");
		// do whatever you want here, animate etc
		// Alternate between the sprite's flipX true and false
		marioAlive = false; //mario is now dead and the enemy update will instead flip, creating a rejoice dance
	}
}