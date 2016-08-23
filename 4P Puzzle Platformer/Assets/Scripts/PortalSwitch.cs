using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PortalSwitch : MonoBehaviour {

	private PortalBehavior correspondingPortal;
	private string portalTarget;
	private bool canInteract;

	private Transform lever;

	private Button abilityButton;

	private PlayerController characterAtSwitch;

	void Start () 
	{
		switch (gameObject.name)
		{
			case "OrangeSwitch":
				portalTarget = "OrangePortal";
				break;
			case "YellowSwitch":
				portalTarget = "YellowPortal";
				break;
			case "GreenSwitch":
				portalTarget = "GreenPortal";
				break;
			case "RedSwitch":
				portalTarget = "RedPortal";
				break;
		}
		correspondingPortal = GameObject.Find(portalTarget).GetComponent<PortalBehavior>();

		lever = transform.Find("Lever");

		abilityButton = GameObject.Find("AbilityButton").GetComponent<Button>();
		abilityButton.onClick.AddListener(() => FlipLever());
	}
	
	void Update () 
	{
		 /*if (canInteract && Input.GetButtonDown("Interact"))
		 {
		 	FlipLever();
		 }*/
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

	public void FlipLever ()
	{
		if (canInteract && characterAtSwitch.canFlipSwitch)
		{
			//Negate Z angle
			lever.eulerAngles = new Vector3(lever.eulerAngles.x, lever.eulerAngles.y, -lever.eulerAngles.z);
			correspondingPortal.SwitchPortal();
			characterAtSwitch.canFlipSwitch = false;
		}
	}
}
