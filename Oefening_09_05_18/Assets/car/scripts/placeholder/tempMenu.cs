using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tempMenu : MonoBehaviour {

	public GameObject[] playerObjs;

	void Update () {
		if(Input.GetKeyDown("r")) {
			SceneManager.LoadScene(0);
		}
	}

	public void playMode (int x) {
		playerObjs [0].SetActive (false);
		playerObjs [1].SetActive (false);
		playerObjs [2].SetActive (false);
		playerObjs [x].SetActive (true);
	}
}
