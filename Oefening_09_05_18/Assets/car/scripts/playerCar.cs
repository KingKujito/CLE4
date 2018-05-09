using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerCar : MonoBehaviour {

	//-------------------------VARIABLES-----------------------------//
	public float money, speed, gravity, backwardSpeed, rayHeight, acceleration, accelBack, drag, drift;
	public Transform[] wheels; // 0=fr 1=fl 2=br 3=bl
	public bool isPlayer, canControl;
	public Vector3 m_EulerAngleVelocity;
	public Transform[] rays; //0-1 = ground, 2-4 = walls
	public float momentum;
	Vector3 myVelocity;
	Rigidbody myRigidbody;
	public Transform myCam, camCore, attention, rotForce;
	public float camSpeed;
	public string hor = "Horizontal", vert = "Vertical", jump = "Jump";
	bool grnd, near;
	public ParticleSystem driftL, driftR, drive;
	public GameObject dust, coinExpl;
	Camera mainCam;
	public AudioSource engine, tires;
	Quaternion lastRot;
	public bool dead;
	//-------------------------STANDAARD FUNCTIES-----------------------------//
	void Start () {
		myRigidbody = gameObject.GetComponent<Rigidbody> ();
		mainCam = myCam.GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		//grnd = detectGround (); near = nearGround ();
		/*momentum = Mathf.Lerp (momentum, 0, Time.deltaTime * drag/momentum);

		myVelocity = transform.InverseTransformDirection(myRigidbody.velocity);

		if(!detectGround() && nearGround() && !res) {
			StartCoroutine ("resetPos");
		}

		if (isPlayer && canControl) {
			control ();
		}*/
		wallDetect ();
		particleControl ();
		wheelsControl ();
		effects ();
		soundControl ();
	}

	void FixedUpdate () {
		momentum = Mathf.Lerp (momentum, 0, Time.deltaTime * drag/ Mathf.Abs(momentum));

		myVelocity = transform.InverseTransformDirection(myRigidbody.velocity);

		if(!detectGround() && nearGround() && !res) {
			StartCoroutine ("resetPos");
		}

		if (isPlayer && canControl) {
			control ();
		}

		camFollow ();
	}

	//-------------------------MOVEMENT & PHYSICS-----------------------------//
	void control () {
		Ray ray = new Ray (rays[1].position, -transform.up);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, rayHeight)) {
			rotForce.rotation = Quaternion.Lerp (rotForce.rotation, Quaternion.FromToRotation (rotForce.up, hit.normal) * rotForce.rotation, 0.2f);
			rotForce.localRotation = Quaternion.Euler(rotForce.localRotation.eulerAngles.x,0,0);
			lastRot = rotForce.rotation;
		}
		//transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation, 0.2f);
		if (!detectGround ()) {
			myRigidbody.MovePosition (transform.position - Vector3.up * gravity * Time.deltaTime + rotForce.forward * momentum * Time.deltaTime);
		} else {
			myRigidbody.MovePosition (transform.position + rotForce.forward * momentum * Time.deltaTime);
		}

		//myRigidbody.MovePosition (transform.position + transform.forward * momentum * Time.deltaTime);
		if(Input.GetAxis(vert) > 0 && detectGround() && momentum < speed/2) {
			momentum += acceleration * Time.deltaTime;
		} else if(Input.GetAxis(vert) > 0 && detectGround() && momentum < speed) {
			momentum += acceleration * Time.deltaTime/2;
		} else if(Input.GetAxis(vert) < 0 && momentum > -backwardSpeed) {
			momentum -= accelBack * Time.deltaTime;
		}

		if(Input.GetAxis(hor) != 0 && detectGround() && Mathf.Abs(myVelocity.z) > 0) {

			float x = Mathf.Clamp( Mathf.Abs((momentum)/2), -1, 1 );

			if (Input.GetButton (jump) && momentum > 0) {
				Quaternion deltaRotation = Quaternion.Euler (m_EulerAngleVelocity * Time.deltaTime * Input.GetAxis (hor) * drift * x);
				myRigidbody.MoveRotation (transform.rotation * deltaRotation);
				myRigidbody.MovePosition (transform.position - transform.right * drift*2 * Input.GetAxis(hor) * Time.deltaTime * x + transform.forward * momentum * Time.deltaTime);
			} else {
				Quaternion deltaRotation = Quaternion.Euler (m_EulerAngleVelocity * Time.deltaTime * Input.GetAxis (hor) * x);
				myRigidbody.MoveRotation (transform.rotation * deltaRotation);
			}
		}
	}

	//-------------------------CAM CONTROLS-----------------------------//

	void camFollow () {

		Quaternion x = Quaternion.Euler (myCam.rotation.eulerAngles.x, camCore.rotation.eulerAngles.y, myCam.rotation.eulerAngles.z);
		float y = Mathf.Clamp(camSpeed * Mathf.Abs(momentum) / (Vector3.Distance(myCam.position,camCore.position)+0.0001f), 5, 100);
		Vector3 z = new Vector3 (camCore.position.x,  transform.position.y + camCore.localPosition.y, camCore.position.z);

		//myCam.position = new Vector3 (camCore.position.x,  transform.position.y + camCore.localPosition.y, camCore.position.z);
		myCam.position = Vector3.Lerp (myCam.position, z , Mathf.Clamp( y * Time.fixedDeltaTime, 0, 1));
		myCam.LookAt(attention);
		myCam.rotation = Quaternion.Euler (20, myCam.rotation.eulerAngles.y, 0);
	}

	//-------------------------RAYCASTS-----------------------------//
	bool detectGround () {
		Ray ray = new Ray (rays[0].position, -transform.up);
		Ray ray2 = new Ray (rays[1].position, -transform.up);
		RaycastHit hit;

		if (!Physics.Raycast (ray, out hit, rayHeight) && Physics.Raycast (ray2, out hit, rayHeight)) {
			Vector3 x = new Vector3 (transform.position.x, transform.position.y+1, transform.position.z+1);
			myRigidbody.AddForceAtPosition ( -transform.up * Time.deltaTime * 2, x);
		}

		if (Physics.Raycast (ray, out hit, rayHeight) || Physics.Raycast (ray2, out hit, rayHeight)) {
			return true;
		} else {
			//rotForce.localRotation = Quaternion.Euler(0,0,0);
			rotForce.rotation = lastRot;
			Quaternion x = Quaternion.Euler (0, transform.rotation.eulerAngles.y ,0);
			transform.rotation = Quaternion.Lerp (transform.rotation, x, Time.deltaTime/2.5f);
			return false;
		}
	}
		
	bool nr;
	bool nearGround () {
		Ray ray = new Ray (rays[0].position, -Vector3.up);
		Ray ray2 = new Ray (rays[1].position, -Vector3.up);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 1.7f) || Physics.Raycast (ray2, out hit, 1.7f)) {

			if (!nr) {
				Instantiate (dust, transform.position, transform.rotation);
			}

			nr = true;
			return true;
		} else {
			nr = false;
			return false;
		}
	}

	//-------------------------RESET POSITION-----------------------------//
	bool res;
	public IEnumerator resetPos () {
		res = true;
		yield return new WaitForSeconds (7);
		if (!detectGround () && nearGround ()) {
			myRigidbody.MovePosition (new Vector3 (transform.position.x, transform.position.y + Time.deltaTime, transform.position.z));
			transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);
			myRigidbody.velocity = new Vector3 (0, 0, 0);
			momentum = 0;
		}
		res = false;
	}


	//-------------------------PARTICLE CONTROL-----------------------------//
	void particleControl () {

		if (Input.GetButton (jump) && momentum > 0 && Input.GetAxis (hor) != 0 && detectGround ()) {
			if (Input.GetAxis (hor) > 0) {
				if (!driftL.isPlaying) {
					driftL.Play ();
				}
				if (driftR.isPlaying) {
					driftR.Stop ();
				}
			} else {
				if (driftL.isPlaying) {
					driftL.Stop ();
				}
				if (!driftR.isPlaying) {
					driftR.Play ();
				}
			}
		} else {
			if (driftL.isPlaying) {
				driftL.Stop ();
			}
			if (driftR.isPlaying) {
				driftR.Stop ();
			}
		}

		if (momentum > 0 && detectGround ()) {
			var main = drive.main;
			main.startLifetime = 0.5f-momentum*0.006f;
			if (!drive.isPlaying) {
				drive.Play ();
			}
		} else {
			if (drive.isPlaying) {
				drive.Stop ();
			}
		}
	}


	//-------------------------WHEEL CONTROL-----------------------------//
	void wheelsControl () {
		foreach(Transform i in wheels) {
			i.Rotate (-Vector3.up * momentum * Time.deltaTime * 4);
		}

		Quaternion x = Quaternion.Euler (wheels[0].localRotation.eulerAngles.x, Input.GetAxis(hor)*30 ,90);
		wheels [0].localRotation = Quaternion.Lerp (wheels[0].localRotation, x, Time.deltaTime*3);
		x = Quaternion.Euler (wheels[1].localRotation.eulerAngles.x, Input.GetAxis(hor)*30 ,90);
		wheels [1].localRotation = Quaternion.Lerp (wheels[1].localRotation, x, Time.deltaTime*3);
	}

	//-------------------------VISUAL EFFECT CONTROL-----------------------------//
	void effects () {
		if (momentum > 30) {
			mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, 60 + momentum / 5, Time.deltaTime*3);
		} else {
			mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, 60, Time.deltaTime*3);
		}
	}

	//-------------------------SOUND CONTROL-----------------------------//
	void soundControl () {
		if (Input.GetAxis (vert) > 0) {
			engine.pitch = momentum / 30;
			engine.volume = Mathf.Lerp (engine.volume, 0.7f, Time.deltaTime * 2);
		} else {
			engine.volume = Mathf.Lerp (engine.volume, 0, Time.deltaTime*0.2f);
		}

		if (detectGround () && momentum > 0 && Input.GetAxis (vert) < 0) {
			tires.volume = Mathf.Lerp (tires.volume, momentum / 30 + 0.3f, Time.deltaTime * 2);
			tires.pitch = 1.3f;
		} else {
			tires.pitch = 1;
			if (Input.GetButtonDown (jump)) {
				tires.Stop ();
				tires.Play ();
			}

			if (Input.GetButton (jump) && Input.GetAxis (hor) != 0 && momentum > 0 && detectGround ()) {
				tires.volume = Mathf.Lerp (tires.volume, Mathf.Abs (Input.GetAxis (hor)), Time.deltaTime * 2);
			} else {
				tires.volume = Mathf.Lerp (tires.volume, 0, Time.deltaTime * 2);
			}
		}
	}

	//-------------------------WALL DETECTION-----------------------------//
	void wallDetect () {
		Ray ray = new Ray (rays[2].position, transform.forward);
		Ray ray1 = new Ray (rays[3].position, transform.forward);
		Ray ray2 = new Ray (rays[4].position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 2f) || Physics.Raycast (ray1, out hit, 2f) || Physics.Raycast (ray2, out hit, 2f)) {
			if (momentum > 0 && canControl && detectGround ()) {
				print ("hit a wall");
				momentum = 0;
				StartCoroutine ("hitWall");
			}
		}
	}

	IEnumerator hitWall () {
		canControl = false;
		yield return new WaitForSeconds (0.6f);
		canControl = true;
	}


	void OnTriggerEnter (Collider coll) {
		if (coll.tag == "Explosion") {
			dead = true;
			canControl = false;
			myRigidbody.velocity = Vector3.up * 20;
			StartCoroutine ("restartLevel");
		} else if (coll.tag == "Coin") {
			Instantiate (coinExpl, coll.transform.position, coll.transform.rotation);
			Destroy (coll.gameObject);
			money++;
		}
	}

	IEnumerator restartLevel () {
		yield return new WaitForSeconds (7);
		SceneManager.LoadScene(0);
	}
}
