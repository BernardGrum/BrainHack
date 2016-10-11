using UnityEngine;
using System.Collections;

public class AttentionWhoreController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetRandomPosition (float xBorder, float yBorder){
		float x = Random.Range (-xBorder, xBorder);
		float y = Random.Range (-yBorder, yBorder);

		transform.localPosition = new Vector2 (x, y);
	}
}
