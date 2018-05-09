using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnColl : MonoBehaviour {

	void OnCollisionEnter (Collision coll) {
		gameObject.GetComponent<AudioSource> ().Stop ();
		gameObject.GetComponent<AudioSource> ().Play ();
	}
}
