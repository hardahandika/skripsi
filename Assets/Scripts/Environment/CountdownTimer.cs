using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour {
	public Text timeText;

	public bool gameisStarted;
	float totalTime = 120f; //2 minutes

	public StageManager stageManager;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		totalTime -= Time.deltaTime;
		UpdateLevelTimer(totalTime);

		if(totalTime <= 0){
			stageManager.isOutOfTime = true;
			stageManager.GameOver();
		}
	}

	public void UpdateLevelTimer(float totalSeconds)
		{
			int minutes = Mathf.FloorToInt(totalSeconds / 60f);
			int seconds = Mathf.RoundToInt(totalSeconds % 60f);

			string formatedSeconds = seconds.ToString();

			if (seconds == 60)
			{
				seconds = 0;
				minutes += 1;
			}

			timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
		}
}
