using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnOnColl : MonoBehaviour {

	public GameObject expl;

	void OnCollisionEnter (Collision coll) {
		foreach (ContactPoint contact in coll.contacts) {
			Instantiate (expl, contact.point, Quaternion.Euler(0,0,0));
		}
	}
}
