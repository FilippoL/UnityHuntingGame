using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeHandler : MonoBehaviour {

	private float time = 60f;

	private Text time_text;

	// Use this for initialization
	void Start () {
		time_text = GetComponent<Text> ();
		DontDestroyOnLoad (this);
	}

	// Update is called once per frame
	void Update () {

		time -= Time.deltaTime;

		time_text.text = "Time: " +  Mathf.RoundToInt(time);

		if (time < 0) {

			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		}
	}
		
}
