using UnityEngine;
using System.Collections;

public class TeleportPad : MonoBehaviour 
{
	public Renderer rend;

	public Transform destination;
	private Transform characterTarget;
	private PlayerController characterController;

	private Color lowBrightness;
	private Color highBrightness;

	void Start ()
	{
		lowBrightness = new Color(3.0f, 3.0f, 3.0f);//new Color(1.25f, 1.25f, 1.25f);
		highBrightness = new Color(3.75f, 3.75f, 3.75f);//new Color(2.0f, 2.0f, 2.0f);

		characterTarget = null;
		characterController = null;
	}

	void Update ()
	{
		rend.material.SetColor("_EmissionColor", Color.Lerp(lowBrightness, highBrightness, Mathf.PingPong(Time.time/1.5f, 1)));

		if (characterTarget && characterController.executeTeleport)
		{
			characterTarget.position = new Vector3(destination.position.x, destination.position.y, characterTarget.position.z);
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		
		if(other.name == "Teleporter")
		{
			Debug.Log("Hit trigger");
			other.gameObject.GetComponent<PlayerController>().canTeleport = true;
			characterTarget = other.gameObject.transform;
			characterController = characterTarget.gameObject.GetComponent<PlayerController>();
			//Debug.Log("Can Teleport");
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if(other.name == "Teleporter")
		{
			other.gameObject.GetComponent<PlayerController>().canTeleport = false;
			characterTarget = null;
			characterController = null;
			//Debug.Log("Can't Teleport");
		}
	}
}









