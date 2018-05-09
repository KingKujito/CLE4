using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	public playerCar p;
	public bool shooter;
	public GameObject gun;
	public GameObject bulletObj;
	GameObject bull;

	void Start () {
		if (!shooter) {
			gun.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton (p.jump) && !shoot) {
			StartCoroutine ("Shoot");
		}
	}

	bool shoot;
	IEnumerator Shoot () {
		shoot = true;
		yield return new WaitForSeconds (0.75f);
		if (p.canControl) {
			bull = Instantiate (bulletObj, transform.position, transform.rotation);
			bull.GetComponent<Rigidbody> ().velocity = bull.transform.forward * (100+p.momentum);
		}
		shoot = false;
	}
		
}
