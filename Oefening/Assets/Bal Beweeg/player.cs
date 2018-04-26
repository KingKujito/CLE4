using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

	//-----------------VARIABLES--------------//
	public float speed;
	public float jumpPower;
	public float rayHeight;
	public float gravity;
	Rigidbody myRigidbody;


	//-----------------STANDAARD FUNCTIES--------------//
	void Start () {
		myRigidbody = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

		control ();

	}


	//-----------------BEWEEG FUNCTIES--------------//
	void control () {
		if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
			myRigidbody.MovePosition(transform.position + transform.right * speed * Input.GetAxis("Horizontal") * Time.deltaTime + transform.forward * speed * Input.GetAxis("Vertical") * Time.deltaTime);
		}

		if(Input.GetButtonDown("Jump") && detectGround()) {
			myRigidbody.velocity = new Vector3(myRigidbody.velocity.x , jumpPower , myRigidbody.velocity.z);
		}

		if(myRigidbody.velocity.y <= 0.5f) {
			myRigidbody.velocity = new Vector3(myRigidbody.velocity.x , -gravity * Time.deltaTime + myRigidbody.velocity.y , myRigidbody.velocity.z);
		}
	}

	//-----------------DETECTEER GROND--------------//
	bool detectGround () {
		Ray ray = new Ray (transform.position, Vector3.down);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, rayHeight)) {
			return true;
		} else {
			return false;
		}
	}

}
