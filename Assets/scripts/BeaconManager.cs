using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeaconManager : MonoBehaviour {

	public List<AudioClip> tones = new List<AudioClip>();
	public float period = 5.0f;
	public AudioSource sourcePrefab;

	private float mTimer = 0;
	private List<AudioSource> mSources = new List<AudioSource>();
	private int mBeaconIndex = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		mTimer += Time.deltaTime;
		if (mTimer > period) {
			mTimer -= period;

			foreach (var source in mSources) {
				source.Play ();
			}
		}
	}

	void CreateBeacon(Transform at) {
		var source = Instantiate (sourcePrefab, at.position, at.rotation) as AudioSource;
		source.clip = tones [mBeaconIndex];
		mSources.Add (source);
		mBeaconIndex = (mBeaconIndex + 1) % tones.Count;
	}
}
