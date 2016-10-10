using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {
	public float Duration;
	public float ClearTime;
	public LevelObjectController[] LevelObjects;

	public bool IsFinished { get; private set; }

	float TimePlaying;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		TimePlaying += Time.deltaTime;
		if (TimePlaying > Duration) {
			IsFinished = true;
		}
		if (TimePlaying > ClearTime) {
			GameObject.Destroy (this.gameObject);
		}
	}

	public void Play(){
		TimePlaying = 0f;
		for (int i = 0; i < LevelObjects.Length; i++) {
			LevelObjects [i].Play ();
		}
	}


}
