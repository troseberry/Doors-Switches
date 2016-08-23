using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorSwitch : MonoBehaviour {

	private DoorBehavior correspondingDoor;
	private string doorTarget;
	private bool canInteract;

	private GameObject offSwitch;
	private GameObject onSwitch;

	private Button abilityButton;

	private PlayerController characterAtSwitch;

	
	void Start () 
	{
		offSwitch = transform.Find("Off").gameObject;
		offSwitch.SetActive(true);

		onSwitch = transform.Find("On").gameObject;
		onSwitch.SetActive(false);


		switch (gameObject.name)
		{
			case "OrangeSwitch":
				doorTarget = "OrangeDoor";
				break;
			case "YellowSwitch":
				doorTarget = "YellowDoor";
				break;
			case "GreenSwitch":
				doorTarget = "GreenDoor";
				break;
			case "BlueSwitch":
				doorTarget = "BlueDoor";
				break;
		}
		correspondingDoor = GameObject.Find(doorTarget).GetComponent<DoorBehavior>();

		abilityButton = GameObject.Find("AbilityButton").GetComponent<Button>();
		abilityButton.onClick.AddListener(() => FlipSwitch());
	}
	
	
	void Update () 
	{
	
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		//Debug.Log(other.gameObject.name);
		if (other.gameObject.layer == 11)
		{
			characterAtSwitch = other.gameObject.GetComponent<PlayerController>();
			canInteract = true;
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		//Debug.Log(other.gameObject.name);
		if (other.gameObject.layer == 11)
		{
			characterAtSwitch = null;
			canInteract = false;
		}
	}

	void FlipSwitch()
	{
		if(canInteract && characterAtSwitch.canFlipSwitch)
		{
			offSwitch.SetActive(!offSwitch.active);
			onSwitch.SetActive(!onSwitch.active);

			correspondingDoor.SwitchDoor();
			characterAtSwitch.canFlipSwitch = false;
		}
	}
}
