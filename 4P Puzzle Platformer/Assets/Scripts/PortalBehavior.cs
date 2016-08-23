using UnityEngine;
using System.Collections;

public class PortalBehavior : MonoBehaviour 
{

	private string name;
	private GameObject characterTarget;
	private bool portalOpen;
	private bool characterInPortal;
	private BoxCollider2D portalCollider;
	private GameObject portalSwitch;


	void Start ()
	{
		//characterTarget = "";
		name = gameObject.name;
		switch (name)
		{
			case "OrangePortal":
				characterTarget = GameObject.Find("DoubleJumper");
				break;
			case "YellowPortal":
				characterTarget = GameObject.Find("Slider");
				break;
			case "GreenPortal":
				characterTarget = GameObject.Find("Teleporter");
				break;
			//case "BluePortal":
				//characterTarget = "Thrower";
				//break;
			case "RedPortal":
				characterTarget = GameObject.Find("WallJumper");
				break;
		}

		portalOpen = false;
		characterInPortal = false;

		portalCollider = GetComponent<BoxCollider2D>();
		//portalCollider.isTrigger = false;
	}

	void Update ()
	{
		DebugPanel.Log ("Character Finished:  " + name + ": ", CharacterFinished());
		if (CharacterFinished() && !characterTarget.GetComponent<PlayerController>().characterStopped)
		{
			Debug.Log(characterTarget.name + " locked in.");
			if(GameManager.GetCurrentCharacter() == characterTarget.name) GameManager.NextCharacter();
			LockCharacter();
		}
	}
	
	//Portal physics layer can only interact with Player physics layer. No need for extra "if other is player" check here
	void OnTriggerEnter2D (Collider2D other)
	{
		//Debug.Log(other.name + " entered " + name);
		if (other.name == characterTarget.name) characterInPortal = true;
	}

	void OnTriggerExit2D (Collider2D other)
	{
		//Debug.Log(other.name + " exited " + name);
		if (other.name == characterTarget.name) characterInPortal = false;
	}

	
	public bool CharacterFinished ()
	{
		return (portalOpen && characterInPortal);
	}

	//Does toggling like this instead of explicitly setting both portalOpen and isTrigger allow for these two to possibly become unsynced?
	public void SwitchPortal ()
	{
		portalOpen = !portalOpen;
		//portalCollider.isTrigger = !portalCollider.isTrigger;
		//Debug.Log(name + " portalOpen is " + portalOpen);
	}

	void LockCharacter ()
	{
		characterTarget.GetComponent<PlayerController>().characterFinished = true;

		characterTarget.GetComponent<PlayerController>().ImmediateStop();
		characterTarget.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);

		Debug.Log("Called ImmediateStop");

		//GameManager.NextCharacter();
	}
}
