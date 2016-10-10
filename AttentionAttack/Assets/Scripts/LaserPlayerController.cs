using UnityEngine;
using System.Collections;
using TETCSharpClient;
using TETCSharpClient.Data;

public class LaserPlayerController : MonoBehaviour, IGazeListener {
    float gazeX = 0f;
    float gazeY = 0f;

    public Transform GazeEyes;
    public Transform GazeCrosshair;
    public Camera GazeCamera;
    public float Tolerance;
    public float BlinkCooldown;

    private float CooldownTimer;
    private bool Blinked;
    private PlanetController LastPlanet;

    // Use this for initialization
    void Start()
    {
        GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push);

        GazeManager.Instance.AddGazeListener(this);
        CooldownTimer = 0f;
        Blinked = false;
    }

    // Update is called once per frame
    void Update()
    {
        float diff = GazeEyes.position.z - GazeCamera.transform.position.z;
        Vector3 target = GazeCamera.ScreenToWorldPoint(new Vector3(gazeX, gazeY, diff));
        GazeEyes.position = target;

        Ray ray = GazeCamera.ScreenPointToRay(new Vector3(gazeX, gazeY, 0f));
        int mask = LayerMask.GetMask(new string[] { "Planet" });
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 50, mask);
        if (hit != null && hit.collider != null)
        {
            target = hit.collider.transform.position;
            target.z = GazeCrosshair.position.z;
            PlanetController p = hit.collider.GetComponent<PlanetController>();
            if(LastPlanet != null && p != LastPlanet){
                LastPlanet.SetLooking(false);
            }
            LastPlanet = p;
            LastPlanet.SetLooking(true);
        }
        else
        {
            if (LastPlanet != null)
            {
                LastPlanet.SetLooking(false);
            }
        }

        GazeCrosshair.position = Vector3.Lerp(GazeCrosshair.position, target, 0.1f);

        if (CooldownTimer > 0f)
        {
            CooldownTimer -= Time.deltaTime;
        }

    }

    public void OnGazeUpdate(GazeData gazeData)
    {
        if (CooldownTimer > 0f)
            return;

        if ((gazeData.State & (GazeData.STATE_TRACKING_FAIL | GazeData.STATE_TRACKING_LOST)) > 0)
        {
            Blinked = true;
        }

        Point2D point = UnityGazeUtils.getGazeCoordsToUnityWindowCoords(gazeData.SmoothedCoordinates);
        if ((gazeData.State & GazeData.STATE_TRACKING_GAZE) > 0)
        {
            if (Blinked)
            {
                CooldownTimer = BlinkCooldown;
                Blinked = false;
            }
            else
            {
                gazeX = (float)point.X;
                gazeY = (float)point.Y;
            }
        }
    }
}
