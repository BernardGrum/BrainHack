using UnityEngine;
using System.Collections;

public class PlanetController : MonoBehaviour {
	public ParticleSystem Explosion;
	public bool BadPlanet;
	public bool Spawned { get; set; }
    public Animator Animator;
    public float Duration;

    private float TimeOn;

	// Use this for initialization
	void Start () {
        Spawned = true;
	}

	// Update is called once per frame
	void Update () {
        TimeOn += Time.deltaTime;
        if (TimeOn > Duration)
        {
            Animator.SetBool("End", true);
        }
	}

	public void PlayExplosion() {
		Explosion.Play ();
	}

	public void Despawn(){
	    Spawned = false;
	}

    public void SetLooking(bool looking)
    {
        Animator.SetBool("Looking", looking);
    }
}
