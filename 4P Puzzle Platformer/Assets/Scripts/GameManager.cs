using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{

	LevelInfo currentLevelInfo;

	//public PortalBehavior orangePortal;
	//public PortalBehavior yellowPortal;
	//public PortalBehavior greenPortal;
	//public PortalBehavior redPortal;

	//private bool levelWon;
	private Canvas levelCompleteCanvas;

	//private static GameObject[] characters;
	public GameObject[] charactersCheck;
	private static List<GameObject> characters;
	private static string currentCharacter;
	private static PlayerController currentCharacterController;

	//private static GameObject[] portals;
	//public GameObject[] portalsCheck;
	//private static List<GameObject> portals;

	public GameObject[] doorsCheck;
	private static List<GameObject> doors;

	private bool holdingCharacterReset;
	private float characterResetTimer;
	private string characterToReset;

	private static Vector2 sliderStartPosition;
	private static Vector2 doubleJumperStartPosition;
	private static Vector2 wallJumperStartPosition;
	private static Vector2 teleporterStartPosition;

	//private bool doSwitchCharacters;

	private Canvas resetCanvas;
	private Slider loadBar;
	private Text resetText;
	private float loadTime = 0;


	
	void Start () 
	{
		Debug.Log(SceneManager.GetActiveScene().name);
		currentLevelInfo = LevelInfoManager.GetLevelInfo(SceneManager.GetActiveScene().name);


		levelCompleteCanvas = GameObject.Find("LevelCompleteCanvas").GetComponent<Canvas>();
		levelCompleteCanvas.enabled = false;


		characters = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
		charactersCheck = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject character in characters)
		{
			character.GetComponent<PlayerController>().enabled = false;
			

			EventTrigger characterEventTrigger = character.GetComponentInChildren<EventTrigger>();
			//Add Trigger Hold Timer Event Trigger
			EventTrigger.Entry triggerEntry = new EventTrigger.Entry();
			triggerEntry.eventID = EventTriggerType.PointerDown;
			triggerEntry.callback.AddListener((data) => {TriggerHoldTimer((BaseEventData) data); });
			characterEventTrigger.triggers.Add(triggerEntry);

			//Add Reset Hold Timer Event Trigger
			EventTrigger.Entry holdEntry = new EventTrigger.Entry();
			holdEntry.eventID = EventTriggerType.PointerUp;
			holdEntry.callback.AddListener((data) => {ResetHoldTimer(); });
			characterEventTrigger.triggers.Add(holdEntry);

			////Add Switch Character Event Trigger
			EventTrigger.Entry switchEntry = new EventTrigger.Entry();
			switchEntry.eventID = EventTriggerType.PointerClick;
			switchEntry.callback.AddListener((data) => {SwitchCharacters((BaseEventData) data); });
			characterEventTrigger.triggers.Add(switchEntry);
		}
		characters[0].GetComponent<PlayerController>().enabled = true;
		

		currentCharacter = characters[0].name;
		currentCharacterController = GameObject.Find(currentCharacter).GetComponent<PlayerController>();
		characterToReset = "";


		/*portals = new List<GameObject>(GameObject.FindGameObjectsWithTag("Portal"));
		portalsCheck = GameObject.FindGameObjectsWithTag("Portal");
		foreach (GameObject portal in portals)
		{
			Debug.Log(portal.name);
		}*/

		doors = new List<GameObject>(GameObject.FindGameObjectsWithTag("Door"));
		doorsCheck = GameObject.FindGameObjectsWithTag("Door");
		foreach (GameObject door in doors)
		{
			Debug.Log(door.name);
		}

		resetCanvas = GameObject.Find("ResetCanvas").GetComponent<Canvas>();
		resetCanvas.enabled = false;

		loadBar = GameObject.Find("ResetLoadBar").GetComponent<Slider>();
		loadBar.value = 0;

		resetText = GameObject.Find("ResetText").GetComponent<Text>();
	}

	void Update ()
	{
		//levelWon = (orangePortal.CharacterFinished() && yellowPortal.CharacterFinished() && greenPortal.CharacterFinished() && redPortal.CharacterFinished());
		//DebugPanel.Log("Level Won? ", LevelWon());
		

		if (doors != null && LevelWon())
		{
			levelCompleteCanvas.enabled = true;
			//pause game, disallow player movement
			//load next level
		}

		if(holdingCharacterReset) 
		{
			characterResetTimer += Time.deltaTime;

			if (loadTime < 1)
			{
				loadTime += Time.deltaTime * .75f;
				loadBar.value = Mathf.Lerp(0, 100, loadTime);
			}
			resetText.color = new Color(resetText.color.r, resetText.color.g, resetText.color.b, Mathf.PingPong(Time.time*1.5f, 1));

		}
		else
		{
			if (resetCanvas.enabled)
			{
				resetCanvas.enabled = false;
				loadTime = 0;
				loadBar.value = 0;
			}
		}
		
		/*if (characterResetTimer >= 0.5)
		{
			//Debug.Log("Button Being Held: " + characterToReset);
			if (loadTime < 1)
			{
				loadTime += Time.deltaTime * .75f;
				loadBar.value = Mathf.Lerp(0, 100, loadTime);
			}
		}*/

		if (characterResetTimer >= 1.5f)
		{
			ResetCharacter();
			ResetHoldTimer();
		}
		DebugPanel.Log("Reset Character Timer: ", characterResetTimer);	
	}

	public void TestTrigger ()
	{
		Debug.Log("Add Listener Worked");
	}

	bool LevelWon ()
	{
		foreach (GameObject door in doors)
		{
			if (!door.GetComponent<DoorBehavior>().CharacterFinished())
			{
				return false;
			}
		}
		return true;
	}

	public static string GetCurrentCharacter ()
	{
		return currentCharacter;
	}

	public static void SetCharacterStart (string characterName, Vector2 position)
	{
		switch (characterName)
		{
			case "Slider":
				sliderStartPosition = position;
				//Debug.Log("Slider Position: " + sliderStartPosition);
				break;
			case "DoubleJumper":
				doubleJumperStartPosition = position;
				break;
			case "WallJumper":
				wallJumperStartPosition = position;
				break;
			case "Teleporter":
				teleporterStartPosition = position;
				break;
		}
	}

	public void TriggerHoldTimer (BaseEventData hold)
	{
		PointerEventData pedHold = (PointerEventData)hold;
		characterToReset = pedHold.pointerEnter.name;
		characterToReset = characterToReset.Remove(characterToReset.Length - 6);

		if (!holdingCharacterReset && currentCharacter == characterToReset) 
		{
			//Debug.Log("Holding Character Reset");
			holdingCharacterReset = true;	
			//Invoke("HoldTimerDelay", 0.5f);
			resetCanvas.enabled = true;
		}
	}

	void HoldTimerDelay ()
	{
		//Debug.Log("Holding Character Reset");
		holdingCharacterReset = true;	
	}

	public void ResetHoldTimer ()
	{
		holdingCharacterReset = false;
		characterResetTimer = 0f;
		//Debug.Log("Character Reset Released");
	}

	//only allow reset if currentChar is characterToReset
	void ResetCharacter ()
	{
		if (currentCharacter == characterToReset) 
		{
			GameObject toResetFromList = characters.Find(obj => obj.name == characterToReset);
			switch (characterToReset)
			{
				case "Slider":
					toResetFromList.transform.position = sliderStartPosition;
					break;
				case "DoubleJumper":
					toResetFromList.transform.position = doubleJumperStartPosition;
					break;
				case "WallJumper":
					toResetFromList.transform.position = wallJumperStartPosition;
					break;
				case "Teleporter":
					toResetFromList.transform.position = teleporterStartPosition;
					break;
			}
			resetCanvas.enabled = false;
			loadBar.value = 0;
			loadTime = 0f;
		}
	}

	//only allow switch when currentChar is not character to switch to
	public void SwitchCharacters (BaseEventData tap)
	{
		PointerEventData pedTap = (PointerEventData)tap;

		if(pedTap.pointerEnter.name.Contains("Button"))
		{
			string switchCharacter = pedTap.pointerEnter.name;
			switchCharacter = switchCharacter.Remove(switchCharacter.Length - 6);

			Debug.Log("Switch Character Name: " + switchCharacter);

			bool canSwitchCharacter = !GameObject.Find(switchCharacter).GetComponent<PlayerController>().characterFinished;

			if(currentCharacter != switchCharacter && canSwitchCharacter) 
			{
				Debug.Log("Switching Character");
				//Because this freezes rigidbody constraints, they don't unfreeze when you switch back to that character
				//Should possibly just set velocity to zero?
				//GameObject.Find(currentCharacter).GetComponent<PlayerController>().ImmediateStop();

				foreach (GameObject character in characters)
				{
					character.GetComponent<PlayerController>().enabled = false;
				}

				characters.Find(obj => obj.name == switchCharacter).GetComponent<PlayerController>().enabled = true;
				currentCharacter = switchCharacter;
				currentCharacterController = GameObject.Find(currentCharacter).GetComponent<PlayerController>();
			}
			else 
			{
				//Debug.Log("Switch Character is same as Current Character");
			}
		}
	}

	public static void NextCharacter ()
	{
		foreach (GameObject character in characters)
		{
			character.GetComponent<PlayerController>().enabled = false;
		}

		foreach (GameObject character in characters)
		{
			if (!character.GetComponent<PlayerController>().characterFinished)
			{
				character.GetComponent<PlayerController>().enabled = true;
				currentCharacter = character.name;
				currentCharacterController = character.GetComponent<PlayerController>();
				return;
			}
		}
	}

	public void ProcessButtonHorizontalInput ()
	{
		
		if (EventSystem.current.currentSelectedGameObject.name == "ArrowLeft")
		{
			Debug.Log("Pressing Left");
			currentCharacterController.SetHorizontalInput(-1f);
			//horizontalInput = -1f;
		}
		else if (EventSystem.current.currentSelectedGameObject.name == "ArrowRight")
		{
			Debug.Log("Pressing Right");
			currentCharacterController.SetHorizontalInput(1f);
			//horizontalInput = 1f;
		}
	}

	public void ZeroHorizontalInput ()
	{
		Debug.Log("Pressing No Button");
		currentCharacterController.SetHorizontalInput(0);
		EventSystem.current.SetSelectedGameObject(null);
	}
}
