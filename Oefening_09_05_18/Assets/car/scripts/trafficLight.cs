using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trafficLight : MonoBehaviour {

	public GameObject[] green, yellow, red;
	public float maxTimer;
	float Timer;
	
	// Update is called once per frame
	void Update () {
		Timer += Time.deltaTime;
		if (Timer > maxTimer) {
			Timer = 0;
		}

		if (Timer < maxTimer / 3) {
			activity (green, true);
			activity (yellow, false);
			activity (red, false);
		} else if (Timer < maxTimer / 2) {
			activity (green, false);
			activity (yellow, true);
			activity (red, false);
		} else {
			activity (green, false);
			activity (yellow, false);
			activity (red, true);
		}
	}

	void activity (GameObject [] x, bool y) {
		foreach(GameObject i in x) {
			i.SetActive (y);
		}
	}
}
