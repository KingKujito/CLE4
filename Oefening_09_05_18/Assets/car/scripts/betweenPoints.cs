using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class betweenPoints : MonoBehaviour {

	public Transform a, b;

	void Update() {
		transform.position = (a.position + b.position) / 2.0f;
	}
}
