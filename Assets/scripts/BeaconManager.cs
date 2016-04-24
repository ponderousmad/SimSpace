using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeaconManager : MonoBehaviour {

	public List<AudioClip> tones = new List<AudioClip>();
	public float period = 5.0f;
	public float collectDistance = 2.0f;
	public float keepDistance = 5.0f;
	public GameObject beaconPrefab;

	private float mTimer = 0;
	private List<AudioSource> mSources = new List<AudioSource>();
	private int mBeaconIndex = 0;
	private bool mCanRemove = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		AudioSource toRemove = null;

		foreach (var source in mSources) {
			if (Vector4.Distance (source.transform.position, transform.position) < collectDistance) {
				toRemove = source;
			}
		}

		if (mSources.Count > 0) {
			if (Vector4.Distance(mSources[mSources.Count - 1].transform.position, transform.position) > keepDistance) {
				mCanRemove = true;
			}
			if (mCanRemove && toRemove != null) {
				mSources.Remove (toRemove);
			}
		}

		mTimer += Time.deltaTime;
		if (mTimer > period) {
			mTimer -= period;

			foreach (var source in mSources) {
				source.Play ();
			}
		}

		if (Input.GetKeyDown ("space")) {
			CreateBeacon (this.transform);
		}
	}

	void CreateBeacon(Transform at) {
		var beacon = Instantiate (beaconPrefab, at.position, at.rotation) as GameObject;
		var source = beacon.GetComponentInChildren<AudioSource> ();
		source.clip = tones [mBeaconIndex];
		mSources.Add (source);
		mBeaconIndex = (mBeaconIndex + 1) % tones.Count;
		mCanRemove = false;
	}
}
