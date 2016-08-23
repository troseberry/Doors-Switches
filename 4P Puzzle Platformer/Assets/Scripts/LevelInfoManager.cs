//Use this to determine what UI character buttons should be shown

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelInfoManager : MonoBehaviour
{
	public static List<LevelInfo> allLevelInfos = new List<LevelInfo>();

	public static LevelInfo Tutorial_DoubleJumper;
	public static LevelInfo Tutorial_WallJumper;
	public static LevelInfo Tutorial_Slider;
	public static LevelInfo Tutorial_Teleporter;

	public static LevelInfo TwoPlayer_01;
	public static LevelInfo TwoPlayer_02;
	public static LevelInfo TwoPlayer_03;
	public static LevelInfo TwoPlayer_04;
	public static LevelInfo TwoPlayer_05;
	public static LevelInfo TwoPlayer_06;

	public static LevelInfo FourPlayer_01;
	public static LevelInfo FourPlayer_newAssetTest;

	void Start ()
	{
		Tutorial_DoubleJumper = new LevelInfo 
		{
			levelName = "Tutorial_DoubleJumper",
			numberOfCharacters = 1,
			characters = new string[] {"DoubleJumper"},
			portals = new string[] {"OrangePortal"}
		};

		Tutorial_WallJumper = new LevelInfo 
		{
			levelName = "Tutorial_WallJumper",
			numberOfCharacters = 1,
			characters = new string[] {"WallJumper"},
			portals = new string[] {"RedPortal"}
		};

		Tutorial_Slider = new LevelInfo 
		{
			levelName = "Tutorial_Slider",
			numberOfCharacters = 1,
			characters = new string[] {"Slider"},
			portals = new string[] {"YellowPortal"}
		};

		Tutorial_Teleporter = new LevelInfo 
		{
			levelName = "Tutorial_Teleporter",
			numberOfCharacters = 1,
			characters = new string[] {"Teleporter"},
			portals = new string[] {"GreenPortal"}
		};




		TwoPlayer_01 = new LevelInfo 
		{
			levelName = "TwoPlayer_01",
			numberOfCharacters = 2,
			characters = new string[] {"WallJumper", "DoubleJumper"},
			portals = new string[] {"RedPortal", "OrangePortal"}
		};

		TwoPlayer_02 = new LevelInfo 
		{
			levelName = "TwoPlayer_02",
			numberOfCharacters = 2,
			characters = new string[] {"WallJumper", "Slider"},
			portals = new string[] {"RedPortal", "YellowPortal"}
		};

		TwoPlayer_03 = new LevelInfo 
		{
			levelName = "TwoPlayer_03",
			numberOfCharacters = 2,
			characters = new string[] {"WallJumper", "Teleporter"},
			portals = new string[] {"RedPortal", "GreenPortal"}
		};

		TwoPlayer_04 = new LevelInfo 
		{
			levelName = "TwoPlayer_04",
			numberOfCharacters = 2,
			characters = new string[] {"DoubleJumper", "Slider"},
			portals = new string[] {"OrangePortal", "YellowPortal"}
		};

		TwoPlayer_05 = new LevelInfo 
		{
			levelName = "TwoPlayer_05",
			numberOfCharacters = 2,
			characters = new string[] {"DoubleJumper", "Teleporter"},
			portals = new string[] {"OrangePortal", "GreenPortal"}
		};

		TwoPlayer_06 = new LevelInfo 
		{
			levelName = "TwoPlayer_06",
			numberOfCharacters = 2,
			characters = new string[] {"Slider", "Teleporter"},
			portals = new string[] {"YellowPortal", "GreenPortal"}
		};




		FourPlayer_01 = new LevelInfo 
		{
			levelName = "FourPlayer_01",
			numberOfCharacters = 4,
			characters = new string[] {"DoubleJumper", "WallJumper", "Slider", "Teleporter"},
			portals = new string[] {"OrangePortal", "RedPortal", "YellowPortal", "GreenPortal"}
		};

		FourPlayer_newAssetTest = new LevelInfo 
		{
			levelName = "FourPlayer_newAssetTest",
			numberOfCharacters = 4,
			characters = new string[] {"DoubleJumper", "WallJumper", "Slider", "Teleporter"},
			portals = new string[] {"OrangePortal", "RedPortal", "YellowPortal", "GreenPortal"}
		};


		allLevelInfos.Add(Tutorial_DoubleJumper);
		allLevelInfos.Add(Tutorial_DoubleJumper);
		allLevelInfos.Add(Tutorial_DoubleJumper);
		allLevelInfos.Add(Tutorial_DoubleJumper);

		allLevelInfos.Add(TwoPlayer_01);
		allLevelInfos.Add(TwoPlayer_02);
		allLevelInfos.Add(TwoPlayer_03);
		allLevelInfos.Add(TwoPlayer_04);
		allLevelInfos.Add(TwoPlayer_05);
		allLevelInfos.Add(TwoPlayer_06);

		allLevelInfos.Add(FourPlayer_01);
		allLevelInfos.Add(FourPlayer_newAssetTest);

	}

	public static LevelInfo GetLevelInfo (string levelName)
	{

		foreach (LevelInfo toFind in allLevelInfos)
		{
			if (toFind.levelName == levelName)
			{
				return toFind;
			}
		}

		Debug.Log("Level dne.");
		return new LevelInfo();
	}
}
