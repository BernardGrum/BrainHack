using UnityEngine;
using System.Collections;

public class ScaleController : MonoBehaviour {
	public float Speed;
	public float MaxOffset;
	private Vector3 StartScale;

	// Use this for initialization
	void Start () {
		StartScale = transform.localScale;

	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = StartScale + Vector3.one * Mathf.Sin (Time.time * Speed) * MaxOffset; 
	}
}
