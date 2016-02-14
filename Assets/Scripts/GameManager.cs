﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

	// make game manager public static so can access this from other scripts
	public static GameManager gm;

	// public variables
	public int score=0;

	public bool canBeatLevel = false;
	public int beatLevelScore=0;

	public float startTime=5.0f;
	
	public Text mainScoreDisplay;
	public Text mainTimerDisplay;

	public GameObject gameOverScoreOutline;

	public AudioSource musicAudioSource;

	public bool gameIsOver = false;

	public GameObject playAgainButtons;
	public string playAgainLevelToLoad;

	public GameObject nextLevelButtons;
	public string nextLevelToLoad;

	private float currentTime;

	//==============================
	//LEVEL 2
	public enum PickupEnum {AddTime, SubstractTime, AddScore, SubstractScore, Multiplier, SlowerShot, FasterTarget}
	public GameObject specialTargetCanvas;
	public Text specialTargetDisplay;
	private int multiplier = 1;

	// setup the game
	void Start () {

		// set the current time to the startTime specified
		currentTime = startTime;

		// get a reference to the GameManager component for use by other scripts
		if (gm == null) 
			gm = this.gameObject.GetComponent<GameManager>();

		// init scoreboard to 0
		mainScoreDisplay.text = "0";
			
		// inactivate the gameOverScoreOutline gameObject, if it is set
		if (gameOverScoreOutline)
			gameOverScoreOutline.SetActive (false);

		// inactivate the playAgainButtons gameObject, if it is set
		if (playAgainButtons)
			playAgainButtons.SetActive (false);

		// inactivate the nextLevelButtons gameObject, if it is set
		if (nextLevelButtons)
			nextLevelButtons.SetActive (false);
	}

	// this is the main game event loop
	void Update () {
		if (!gameIsOver) {
			if (canBeatLevel && (score >= beatLevelScore)) {  // check to see if beat game
				BeatLevel ();
			} else if (currentTime < 0) { // check to see if timer has run out
				EndGame ();
			} else { // game playing state, so update the timer
				currentTime -= Time.deltaTime;
				mainTimerDisplay.text = currentTime.ToString ("0.00");
			}
		}
	}

	void EndGame() {
		// game is over
		gameIsOver = true;

		// repurpose the timer to display a message to the player
		mainTimerDisplay.text = "GAME OVER";

		// activate the gameOverScoreOutline gameObject, if it is set 
		if (gameOverScoreOutline)
			gameOverScoreOutline.SetActive (true);
	
		// activate the playAgainButtons gameObject, if it is set 
		if (playAgainButtons)
			playAgainButtons.SetActive (true);

		// reduce the pitch of the background music, if it is set 
		if (musicAudioSource)
			musicAudioSource.pitch = 0.5f; // slow down the music
	}
	
	void BeatLevel() {
		// game is over
		gameIsOver = true;

		// repurpose the timer to display a message to the player
		mainTimerDisplay.text = "LEVEL COMPLETE";

		// activate the gameOverScoreOutline gameObject, if it is set 
		if (gameOverScoreOutline)
			gameOverScoreOutline.SetActive (true);

		// activate the nextLevelButtons gameObject, if it is set 
		if (nextLevelButtons)
			nextLevelButtons.SetActive (true);
		
		// reduce the pitch of the background music, if it is set 
		if (musicAudioSource)
			musicAudioSource.pitch = 0.5f; // slow down the music
	}

	// public function that can be called to update the score or time
	public void targetHit (int scoreAmount, float timeAmount)
	{
		// increase the score by the scoreAmount and update the text UI
		score += scoreAmount;
		mainScoreDisplay.text = score.ToString ();
		
		// increase the time by the timeAmount
		currentTime += timeAmount;
		
		// don't let it go negative
		if (currentTime < 0)
			currentTime = 0.0f;

		// update the text UI
		mainTimerDisplay.text = currentTime.ToString ("0.00");
	}

	public void specialTargetHit() {
		PickupEnum pickupEnum = (PickupEnum)Random.Range (0, 7);
		string text = "";
		switch (pickupEnum) {
		case PickupEnum.AddScore:
			score += (int)Random.Range (0.1f, 2.0f) * 10 * multiplier;
			text = "MORE POINTS";
			break;
		case PickupEnum.SubstractScore:
			score -= (int)Random.Range (0.1f, 2.0f) * 10 * multiplier;
			text = "LESS POINTS";
			break;
		case PickupEnum.AddTime:
			currentTime += Random.Range (1, 10);
			text = "MORE TIME";
			break;
		case PickupEnum.SubstractTime:
			currentTime -= Random.Range (1, 10);
			text = "LESS TIME";
			break;
		case PickupEnum.Multiplier:
			multiplier = multiplier * 2;
			text = "MULTIPLIER X2";
			break;
		case PickupEnum.SlowerShot:
			Shooter.shooter.power = Shooter.shooter.power * 0.4f;
			text = "SLOWER SHOTS";
			break;
		case PickupEnum.FasterTarget:
			SpawnGameObjects.sgm.targetSpeed = SpawnGameObjects.sgm.targetSpeed * 2;
			SpawnGameObjects.sgm.motionMagnitude = SpawnGameObjects.sgm.motionMagnitude * 3f;
			text = "FASTER TARGETS";
			break;
		}
		if (currentTime < 0) {
			currentTime = 0.0f;
		}
		showSpecialPickupText (text);
		mainScoreDisplay.text = score.ToString ();
		mainTimerDisplay.text = currentTime.ToString ("0.00");
	}

	private void showSpecialPickupText(string pickupText) {
		specialTargetDisplay.text = pickupText;
	}
		

	// public function that can be called to restart the game
	public void RestartGame ()
	{
		// we are just loading a scene (or reloading this scene)
		// which is an easy way to restart the level
		Application.LoadLevel (playAgainLevelToLoad);
	}

	// public function that can be called to go to the next level of the game
	public void NextLevel ()
	{
		// we are just loading the specified next level (scene)
		Application.LoadLevel (nextLevelToLoad);
	}
	

}
