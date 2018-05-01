using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGamePauseMenu : MonoBehaviour {

	void Start () {

		GetComponent<Canvas> ().enabled = false;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			Time.timeScale = 0;
			GetComponent<Canvas> ().enabled = !GetComponent<Canvas> ().enabled;
		}

		if (!GetComponent<Canvas> ().enabled) {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<SmoothMouseLook> ().enabled = true;
			Cursor.lockState = CursorLockMode.Locked;
			Time.timeScale = 1;
		} else {
			Cursor.lockState = CursorLockMode.None;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<SmoothMouseLook> ().enabled = false;
		}
	}

	public void BackToPlay(){
		GameObject.FindGameObjectWithTag ("Player").GetComponent<SmoothMouseLook> ().enabled = true;
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1;	
	}

	public void GoToMainMenu(){
		Time.timeScale = 1;	
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);
	}
		
	public void QuitGame(){
		Application.Quit ();
	}
}
