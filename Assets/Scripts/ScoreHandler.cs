using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreHandler : MonoBehaviour {

	private int score = 0;
	private int highscore;

	private Text score_text;

	string highScoreKey = "HighScore";
	string currentScore = "Score";


	// Use this for initialization
	void Start () {
		score_text = GetComponent<Text> ();
		DontDestroyOnLoad (this);

		highscore = PlayerPrefs.GetInt (highScoreKey, 0);
	}
	
	// Update is called once per frame
	void Update () {

		score_text.text = "Score: " + score;
	}

	public void IncreaseScore()
	{
		score++;
	}

	void OnDisable()
	{


		PlayerPrefs.SetInt (currentScore, score);
		PlayerPrefs.Save ();
	}
}
