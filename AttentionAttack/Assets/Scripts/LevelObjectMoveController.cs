using UnityEngine;
using System.Collections;

public class LevelObjectMoveController : LevelObjectController {
	[SerializeField]public Vector3 Speed;
	bool IsPlaying = false;

	public override void Play(){
		IsPlaying = true;
	}

	void Update(){
		if (IsPlaying) {
			Vector3 pos = transform.localPosition;
			pos += Speed * Time.deltaTime;
			transform.localPosition = pos;
		}
	}
}
