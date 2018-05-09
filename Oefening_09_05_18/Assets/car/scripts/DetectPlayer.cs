using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectPlayer : MonoBehaviour {

	public playerCar p, p2;
	public Text t;
	public AudioListener[] a;
	int x;

	void Start () {
		t.text = "";
		//StartCoroutine ("s");
	}
	
	// Update is called once per frame
	void Update () {
		
		/*
		foreach(AudioListener y in a) {
			y.enabled = false;
		}
		a [x].enabled = true;*/

		if(p.dead) {
			t.text = "Player 2 wins";
			t.color = new Color (0,0,255);
			Destroy (this);
			a [0].enabled = false;
			a [1].enabled = true;
		} else if(p2.dead) {
			t.text = "Player 1 wins";
			t.color = new Color (255,0,0);
			Destroy (this);
			a [1].enabled = false;
			a [0].enabled = true;
		}
	}

	IEnumerator s () {
		yield return new WaitForSeconds (1);
		if (x == 0) {
			x = 1;
		} else {
			x = 0;
		}
		StartCoroutine ("s");
	}
}
