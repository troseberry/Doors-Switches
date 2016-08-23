using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menus : MonoBehaviour 
{

	public Canvas debugCanvas;

	void Start () 
	{
		if (SceneManager.GetActiveScene().buildIndex > 0) debugCanvas.enabled = false;
	}

	public void NextLevel ()
	{
		int nextLevelNumber = SceneManager.GetActiveScene().buildIndex + 1;
		Debug.Log("Next Level: " + nextLevelNumber);

		Debug.Log("Total Scene Count: " + SceneManager.sceneCount);

		if(nextLevelNumber < SceneManager.sceneCountInBuildSettings)
		{
			SceneManager.LoadScene(nextLevelNumber);
		}
		else 
		{
			SceneManager.LoadScene(0);
		}
	}

	public void PreviousLevel ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	public void ResetLevel ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

	public void MainMenu ()
	{
		SceneManager.LoadScene(0);
	}


	public void ToggleDebugMenu ()
	{
		debugCanvas.enabled = !debugCanvas.enabled;
	}
}
