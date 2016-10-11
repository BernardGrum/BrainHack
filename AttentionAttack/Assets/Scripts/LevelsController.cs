using UnityEngine;
using System.Collections;

public class LevelsController : MonoBehaviour {
	public LevelController[] EasyLevels;
	public LevelController[] MediumLevels;
	public LevelController[] HardLevels;

	int CurrentLevelIndex;
	LevelController CurrentLevel;
	public LevelController[] LevelPool;

	// Use this for initialization
	void Start () {

	}

	public void BeginLevel(){
		InitLevelsPool ();
		PlayNextLevel ();
	}

	void InitLevelsPool(){
		CurrentLevelIndex = -1;
		switch (ADHDTestController.Instance.UserDifficulty) {
		case ADHDTestController.Difficulty.EASY:
			LevelPool = EasyLevels;
			break;
		case ADHDTestController.Difficulty.MEDIUM:
			LevelPool = MediumLevels;
			break;
		case ADHDTestController.Difficulty.HARD:
			LevelPool = HardLevels;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (CurrentLevel != null && CurrentLevel.IsFinished) {
			PlayNextLevel ();
		}
	}

	void PlayNextLevel(){
		/*
		int i;
		do {
			i = Random.Range(0, LevelPool.Length);
		} while (i == CurrentLevelIndex && LevelPool.Length > 1);
		*/


		CurrentLevelIndex++;

		if (CurrentLevelIndex >= LevelPool.Length) {
			ADHDTestController.Instance.IncreaseDifficulty ();
			InitLevelsPool ();
			CurrentLevelIndex = 0;
		}

		CurrentLevel = GameObject.Instantiate (LevelPool [CurrentLevelIndex], transform, false) as LevelController;
		CurrentLevel.Play ();
	}
}
