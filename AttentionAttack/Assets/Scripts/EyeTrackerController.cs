using UnityEngine;
using System.Collections;
using System;
using TETCSharpClient;
using TETCSharpClient.Data;

public class EyeTrackerController : MonoBehaviour, IGazeListener {
	float gazeX = 0f;
	float gazeY = 0f;
	public int lookAways = 0;
	public float timeOff = 0f;
    public float curTimeOff = 0f;
    private bool lookedAway = false;

	public Transform GazeTarget;
	public Transform GazePlanet;
	public Camera GazeCamera;
	public float Tolerance;
	public float BlinkCooldown;

	private float CooldownTimer;
	private bool Blinked;

    // Use this for initialization
    void Start () {
        GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push);
        
        GazeManager.Instance.AddGazeListener(this);
		CooldownTimer = 0f;
		Blinked = false;
    }

    public void StartLevel() {	
        lookAways = 0;
        timeOff = 0f;
        curTimeOff = 0f;
        lookedAway = false;
    }

    // Update is called once per frame
    void Update(){
		float diff = GazeTarget.position.z - GazeCamera.transform.position.z;
		Vector3 target = GazeCamera.ScreenToWorldPoint(new Vector3(gazeX, gazeY , diff));
		if (Vector2.Distance (target, GazePlanet.position) < Tolerance) {
			target.x = GazePlanet.position.x;
			target.y = GazePlanet.position.y;
            lookedAway = false;
		} else {
            if (!lookedAway)
            {
                lookedAway = true;
                curTimeOff = 0f;
            }
            else
            {
                curTimeOff += Time.deltaTime;
                if (curTimeOff > 0.2f && curTimeOff < 10f) {
                    lookAways++;
                    curTimeOff = 15f;
                }
            }
			timeOff += Time.deltaTime;
		}

		GazeTarget.position = Vector3.Lerp(GazeTarget.position, target, 0.1f);

		if (CooldownTimer > 0f) {
			CooldownTimer -= Time.deltaTime;
		}

    }

    public void OnGazeUpdate(GazeData gazeData){
		if (CooldownTimer > 0f)
			return;
		
		if ((gazeData.State & (GazeData.STATE_TRACKING_FAIL | GazeData.STATE_TRACKING_LOST)) > 0) {
			Blinked = true;
		}

		Point2D point = UnityGazeUtils.getGazeCoordsToUnityWindowCoords (gazeData.SmoothedCoordinates);
		if ((gazeData.State & GazeData.STATE_TRACKING_GAZE) > 0) {
			if (Blinked) {
				CooldownTimer = BlinkCooldown;
				Blinked = false;
			} else {
				gazeX = (float)point.X;
				gazeY = (float)point.Y;
			}
		}
    }
}
