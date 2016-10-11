using UnityEngine;
using System.Collections;

public class RotationController : MonoBehaviour {
	public Vector3 RotationSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 rot = transform.eulerAngles;
		rot += RotationSpeed * Time.deltaTime;
		transform.eulerAngles = rot;
	}
}
