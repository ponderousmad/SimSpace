using UnityEngine;
using System.Collections;

public class Storm : MonoBehaviour {

	public float startDelay = 30.0f;
	public float rampTime = 25.0f;
	public float fogDensity = 0.1f;
	public float dim = 0.25f;
	public Light daylight;

	private bool mStarted = false;
	private float mElapsed = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (mStarted && mElapsed < rampTime) {
			mElapsed = Mathf.Min (mElapsed + Time.deltaTime, rampTime);
			RenderSettings.fogDensity = fogDensity * mElapsed / rampTime;
			if (daylight != null) {
				daylight.intensity = 1 - (dim * mElapsed / rampTime);
			}
		} else if (!mStarted) {
			mElapsed += Time.deltaTime;
			if (mElapsed > startDelay) {
				StartStorm ();
			}
		}
	}

	public void StartStorm() {
		mStarted = true;
		mElapsed = 0.0f;

		var particles = GetComponent<ParticleSystem>();
		if (particles != null) {
			particles.Play ();
		}
	}
}
