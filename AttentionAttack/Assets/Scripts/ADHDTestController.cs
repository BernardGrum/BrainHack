using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ADHDTestController : MonoBehaviour {
	public Canvas results;
	public Text numberOfGazeOffs;
	public Text timeOffPlanet; 
	public Canvas rules;
	public static ADHDTestController Instance;
	public enum Difficulty {
		EASY,
		MEDIUM,
		HARD
	}

	public BoxCollider2D Border;
	public AttentionWhoreController AttentionWhore;
	public EyeTrackerController EyeTrackerController;
	public LevelsController LevelsController;

	public Difficulty UserDifficulty;

	// Use this for initialization
	void Start () {

	}

	public void BeginLevel(){
		rules = rules.GetComponent<Canvas> ();
		rules.enabled = false;
		results = results.GetComponent<Canvas> ();
		results.enabled = false;
		numberOfGazeOffs = numberOfGazeOffs.GetComponent<Text> ();
		timeOffPlanet = timeOffPlanet.GetComponent<Text> ();

		Instance = this;
		UserDifficulty = Difficulty.EASY;
		AttentionWhore.SetRandomPosition (Border.bounds.extents.x, Border.bounds.extents.y);

		LevelsController.BeginLevel ();
        EyeTrackerController.StartLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void IncreaseDifficulty(){
		if (UserDifficulty == Difficulty.EASY) {
			UserDifficulty = Difficulty.MEDIUM;
		} else if (UserDifficulty == Difficulty.MEDIUM) {
			UserDifficulty = Difficulty.HARD;
		} else {
			results.enabled = true;
			numberOfGazeOffs.text = "Looked away: "+ EyeTrackerController.lookAways + "x";
			timeOffPlanet.text = "Look away time: " + EyeTrackerController.timeOff + " sec";
		}
	}
}
