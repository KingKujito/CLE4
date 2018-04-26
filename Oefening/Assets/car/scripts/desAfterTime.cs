using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class desAfterTime : MonoBehaviour {

	public float timer;

	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0) {
			Destroy (gameObject);
		}
	}
}
