using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour 
{

	public static PlayerController Instance;

	private Rigidbody2D characterRigidbody;
	private float horizontalInput;
	private float accelerationSpeed;

	private bool grounded;
	private Transform leftGroundCheck;
	private Transform centerGroundCheck;
	private Transform rightGroundCheck;
	private bool leftGrounded;
	private bool centerGrounded;
	private bool rightGrounded;

	private int jumpCount = 0;

	private bool onWall;
	private bool leftWallCheck;
	private bool rightWallCheck;

	public bool doubleJumper;
	public bool wallJumper;
	public bool slider;
	public bool	thrower;
	public bool teleporter;
	
	private bool executeJump;
	private bool executeWallJump;
	private bool executeSlide;
	public bool executeTeleport;
	public bool canTeleport;
	public bool canFlipSwitch;
	
	private Button jumpButton;
	private Button abilityButton;
	private Button arrowLeft;
	private Button arrowRight;

	private bool leftMovement;
	private bool rightMovement;

	public bool characterFinished;
	public bool characterStopped;
	//private RigidbodyConstraints2D stationaryConstraints;
	//public GameObject sliderInterfaceObject;

	void Start () 
	{
		characterRigidbody = GetComponent<Rigidbody2D>();
		accelerationSpeed = 5.0f;

		leftGroundCheck =  transform.Find("LeftGroundCheck");
		centerGroundCheck =  transform.Find("CenterGroundCheck");
		rightGroundCheck =  transform.Find("RightGroundCheck");

		canTeleport = false;
		canFlipSwitch = true;

		jumpButton = GameObject.Find("JumpButton").GetComponent<Button>();
		jumpButton.onClick.AddListener(() => TriggerJump());

		abilityButton = GameObject.Find("AbilityButton").GetComponent<Button>();
		abilityButton.onClick.AddListener(() => TriggerAbility());

		//sliderInterfaceObject = GameObject.Find("SliderButton");
		//sliderInterfaceObject.GetComponent

		GameManager.SetCharacterStart(name, transform.position);
		characterFinished = false;
		characterStopped = false;
	}
	
	
	void Update () 
	{
		//horizontalInput = Input.GetAxisRaw("Horizontal");

		/*Vector3 accelerometor = Input.acceleration;
		if (accelerometor.x < -0.1f || accelerometor.x > 0.1f)
		{
			horizontalInput = accelerometor.x > 0 ? 1 : -1;
		}
		else 
		{
			horizontalInput = 0;
		}*/
		//DebugPanel.Log("Accel X: ", Input.acceleration.x);

		//For joystick prefab
		//horizontalInput = CrossPlatformInputManager.GetAxisRaw("Horizontal");


		//for left & right arrow buttons
		if (!leftMovement && !rightMovement)
		{
			horizontalInput = 0;
		}
		else 
		{
			horizontalInput = leftMovement ? -1 : 1;
		}
		
		
		leftGrounded = Physics2D.Raycast(leftGroundCheck.transform.position, Vector2.down, 0.01f, (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Players")));
		centerGrounded = Physics2D.Raycast(centerGroundCheck.transform.position, Vector2.down, 0.01f, (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Players")));
		rightGrounded = Physics2D.Raycast(rightGroundCheck.transform.position, Vector2.down , 0.01f, (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Players")));
		grounded = (leftGrounded || centerGrounded || rightGrounded);
		
		rightWallCheck = Physics2D.Raycast(transform.position, Vector2.right, 0.55f, 1 << LayerMask.NameToLayer("Wall"));
		leftWallCheck = Physics2D.Raycast(transform.position, Vector2.left, 0.55f, 1 << LayerMask.NameToLayer("Wall"));
		onWall = (leftWallCheck || rightWallCheck);



		//RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 10f, 1 << LayerMask.NameToLayer("Wall"));
		//Debug.DrawRay(transform.position, hit.transform.position - transform.position, Color.red);

		DebugPanel.Log("Can Teleport? ", canTeleport);

		if(grounded) jumpCount = 0;

		
		//ONLY NEED THESE IN UPDATE FOR MOUSE/KEYBOARD INPUT
		// Character Jumps
		if (doubleJumper)
		{
			if (Input.GetButtonDown("Jump") && jumpCount < 2)
			{
				executeJump = true;
				//jumpCount = (jumpCount + 1) % 3;
			}
		}
		//issue where after about 5 consecutive jumps, wall jumper is no longer registered as being onWall
		else if (wallJumper)
		{
			if (Input.GetButtonDown("Jump"))
			{
				if(grounded && jumpCount < 1) 
				{
					executeJump = true;
					//jumpCount = (jumpCount + 1) % 2;
				}
				else if(onWall) 
				{
					executeWallJump = true;
					//jumpCount++;
				}
			}
		}
		else if (slider)
		{
			if (Input.GetButtonDown("Jump") && jumpCount < 1)
			{
				executeJump = true;
				//jumpCount = (jumpCount + 1) % 2;
			}

			if ((Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift)) && horizontalInput != 0)
			{
				Debug.Log("Slide");
				executeSlide = true;
				//StartCoroutine("ScaleSlider");
			}
		} 
		else if (teleporter)
		{
			if (Input.GetButtonDown("Jump")  && jumpCount < 1)
			{
				executeJump = true;
				//jumpCount = (jumpCount + 1) % 2;
			}

			if ((Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift)) && horizontalInput != 0 && canTeleport)
			{
				Debug.Log("Teleport");
				executeTeleport = true;
				//StartCoroutine("BlinkTeleporter");
			}
		}


		DebugPanel.Log("Horizontal Input: ", horizontalInput);
		DebugPanel.Log("Grounded: ", grounded);
		DebugPanel.Log("Jump Count: " , jumpCount);
		DebugPanel.Log("On Wall: ", onWall);
		DebugPanel.Log("Left Wall: ", leftWallCheck);
		DebugPanel.Log("Right Wall: ", rightWallCheck);
		//DebugPanel.Log("Wall Jump Force: ", wallJumpForce);
	}

	void FixedUpdate ()
	{
		if(horizontalInput != 0)
		{
			//Stops character from slidding up walls and slows them if they hold a horizontal direction key
			if(onWall && !grounded) 
			{
				float verticalVelocity = wallJumper ? 0 : characterRigidbody.velocity.y;
				characterRigidbody.velocity = new Vector2(characterRigidbody.velocity.x, verticalVelocity);
			}
			else
			{
				Vector2 moveVector = new Vector2(horizontalInput*2, characterRigidbody.velocity.y);
				characterRigidbody.velocity = new Vector2(moveVector.x * accelerationSpeed, moveVector.y);
			}	
		}
		else 
		{
			characterRigidbody.velocity = new Vector2(0, characterRigidbody.velocity.y);

			//Use this for after slide movement
			//characterRigidbody.velocity =  Vector2.Lerp(characterRigidbody.velocity, new Vector2(0, characterRigidbody.velocity.y), Time.deltaTime * 3);
		}

		

		if (executeJump)
		{
			//Debug.Log("Adding force to character.");
			characterRigidbody.AddForce(new Vector2(0f, 10.0f), ForceMode2D.Impulse);

			//for touch button, ie. not updating jumpcount in update, must update here or else count will not update correctly.
			//jumpCount++;

			if (doubleJumper) 
			{
				jumpCount = (jumpCount + 1) % 3;
			}
			else if (wallJumper) 
			{
				jumpCount = onWall ? ((jumpCount + 1) % 2) : jumpCount++;
			}
			else
			{
				jumpCount = (jumpCount + 1) % 2;
			}

			executeJump = false;
		}
		else if (executeWallJump)
		{
			float upwardForce = (horizontalInput != 0) ? 20f : 0f;
			float wallJumpForce = leftWallCheck ? 50.0f : -50.0f;

			characterRigidbody.AddForce(new Vector2(wallJumpForce, upwardForce), ForceMode2D.Impulse);
			executeWallJump = false;
		}
		else if (executeSlide)
		{
			StartCoroutine("ScaleSlider");
			float slideForce = (horizontalInput > 0) ? 50f : -50f;	//force of 100f shoots cube thru wallls
			characterRigidbody.AddForce(new Vector2(slideForce, 0f), ForceMode2D.Impulse);
			executeSlide = false;
		}
		else if (executeTeleport)
		{
			//StartCoroutine("BlinkTeleporter");
			//float teleportMovement = (horizontalInput > 0) ? 40f : -40f;
			//characterRigidbody.AddForce(new Vector2(teleportMovement, 0f), ForceMode2D.Impulse);
			executeTeleport = false;
		}
	}

	public void SetHorizontalInput (float input)
	{
		if (input < 0) 
		{
			leftMovement = true;
			rightMovement = false;
		}
		else if (input > 0)
		{
			leftMovement = false;
			rightMovement = true;
		}
		else
		{
			leftMovement = false;
			rightMovement = false;
		}
	}

	public void ImmediateStop ()
	{
		Debug.Log("Executed ImmediateStop");
		characterRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
		characterStopped = true;
		
	}

	IEnumerator ScaleSlider ()
	{
		transform.localScale = new Vector3(1f, Mathf.Lerp(0.5f, 1f, Time.deltaTime / 5), 1f);
		yield return new WaitForSeconds(0.15f);
		transform.localScale = new Vector3(1f, Mathf.Lerp(1f, 0.5f, Time.deltaTime * 10), 1f);

		transform.localScale = new Vector3(1f, 1f, 1f);
	}

	IEnumerator BlinkTeleporter () 
	{
		//For Teleporter instead of Slider
		//The following makes it look like the cube transport to the other side of a wall if walls are positioned on the ground
		//Player must stand next to wall, push in the direction they wish to go, and hit shift for this to work
		//Execute slide code lets the cube move thru walls. Need to limit this to non-outermost walls so player doesn't die
		//Or limit it to teleporter specific walls so players can't cheat

		transform.localScale = new Vector3(Mathf.Lerp(0.25f, 1f, Time.deltaTime / 5), Mathf.Lerp(0.25f, 1f, Time.deltaTime / 5), 1f);
		yield return new WaitForSeconds(0.15f);
		transform.localScale = new Vector3(Mathf.Lerp(1f, 0.25f, Time.deltaTime * 10), Mathf.Lerp(1f, 0.25f, Time.deltaTime * 10), 1f);
		

		transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void TriggerJump ()
	{
		if (doubleJumper)
		{
			if (jumpCount < 2) executeJump = true;
		}
		//issue where after about 5 consecutive jumps, wall jumper is no longer registered as being onWall
		else if (wallJumper)
		{
			if(grounded && jumpCount < 1) 
			{
				executeJump = true;
			}
			else if(onWall) 
			{
				executeWallJump = true;
			}
		}
		else
		{
			if (jumpCount < 1) executeJump = true;
		}

	}

	public void TriggerAbility ()
	{
		if (slider)
		{
			if (horizontalInput != 0)
			{
				Debug.Log("Slide");
				executeSlide = true;
			}
		} 
		else if (teleporter)
		{
			if (/*horizontalInput != 0 && */canTeleport)
			{
				Debug.Log("Teleport");
				executeTeleport = true;
			}
		}
	}
}



















