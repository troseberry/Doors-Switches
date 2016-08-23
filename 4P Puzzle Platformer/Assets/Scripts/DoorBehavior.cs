using UnityEngine;
using System.Collections;

public class DoorBehavior : MonoBehaviour 
{

	private string name;
	private GameObject characterTarget;
	private bool doorOpen;
	private bool characterInDoor;
	private BoxCollider2D doorCollider;
	private GameObject doorSwitch;
	
	void Start () 
	{
		name = gameObject.name;
		switch (name)
		{
			case "OrangeDoor":
				characterTarget = GameObject.Find("DoubleJumper");
				break;
			case "YellowDoor":
				characterTarget = GameObject.Find("Slider");
				break;
			case "GreenDoor":
				characterTarget = GameObject.Find("Teleporter");
				break;
			case "BlueDoor":
				characterTarget = GameObject.Find("WallJumper");
				break;
		}

		doorOpen = false;
		characterInDoor = false;

		doorCollider = GetComponent<BoxCollider2D>();
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

	void OnTriggerEnter2D (Collider2D other)
	{
		//Debug.Log(other.name + " entered " + name);
		if (other.name == characterTarget.name) characterInDoor = true;
	}

	void OnTriggerExit2D (Collider2D other)
	{
		//Debug.Log(other.name + " exited " + name);
		if (other.name == characterTarget.name) characterInDoor = false;
	}

	public bool CharacterFinished ()
	{
		return (doorOpen && characterInDoor);
	}

	//Does toggling like this instead of explicitly setting both doorOpen and isTrigger allow for these two to possibly become unsynced?
	public void SwitchDoor ()
	{
		doorOpen = !doorOpen;
		//doorCollider.isTrigger = !doorCollider.isTrigger;
		//Debug.Log(name + " doorOpen is " + doorOpen);
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
