using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {


	GameObject go_score;
	
	Player playerRef;
	
	TextMesh meshTextScore;
	public Transform scorePosition;
	Color newColor;

	string phraseOnTextEnd = " ";
	public string negativeSighOrPositive = " ";
	
	public bool scoreIsActive = false;
	public bool scoreIsStatic = false;

	public int tempText;

	public float timerActive;

	// Use this for initialization
	void Start () {
		 
		go_score = gameObject;
		
		meshTextScore = gameObject.GetComponent<TextMesh>();
		
	}

	//Here we getting score setting from game objects	
	public int GettingNumber(int num, Color color, string endPhrase, string sigh, Transform newTransform, bool b_scoreStatic)
	{	
		if(num == 1 && endPhrase == " pts")
		{
            endPhrase = " pt";
			phraseOnTextEnd = endPhrase;
		}	
		else
		{ 
			phraseOnTextEnd = endPhrase;
		}
		negativeSighOrPositive = sigh;
		tempText = num;
		newColor = color;
		 // we get new position from refference object
		scoreIsStatic = b_scoreStatic; 	
		if(scoreIsStatic)
		{ 
			scorePosition = newTransform;
			scorePosition.position = new Vector3(scorePosition.position.x, scorePosition.position.y, 0);
		}
		if(!scoreIsStatic)
		{
			scorePosition = newTransform;
		} 
		

		return num;
	}

	public void DestroyOnGameReload()
	{
		this.gameObject.SetActive(true);
		Destroy(this.gameObject);
	}


	void FixedUpdate()
	{
		if(timerActive > 0)
		{
			timerActive = timerActive - 0.25f;
		}
		if(timerActive <= 0)
		{
			scoreIsActive = false;

			if(scoreIsActive == false)
			{
				go_score.transform.position = new Vector3(0,10,0);
				go_score.transform.localScale = new Vector3(1f,1f,1f);
				this.gameObject.SetActive(false);
				phraseOnTextEnd = "";
				this.name = "score_NoName";
				meshTextScore.color = Color.clear;

			}
		}
	}



	// Update is called once per frame
	void Update () {

		if(playerRef != null)
		{
			playerRef = GameObject.Find("player").GetComponent<Player>();
		}
		//////if the score is NOT static
		if(scoreIsActive && !scoreIsStatic)
		{	
			meshTextScore.color = newColor;
			meshTextScore.text = negativeSighOrPositive + tempText.ToString() + phraseOnTextEnd; 
			go_score.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			go_score.transform.position = new Vector3(go_score.transform.position.x, go_score.transform.position.y + 0.015f, 0);
		}
		///// if the  score is static
		if(scoreIsActive && scoreIsStatic)
		{
			meshTextScore.color = newColor;
			meshTextScore.text = negativeSighOrPositive + tempText.ToString() + phraseOnTextEnd;
			go_score.transform.localScale = new Vector3(1.15f,1.15f,1.15f);
			
				//If passing transform argument bacome a NULL we make this objcet not active	
				if(scorePosition != null)
				{
					go_score.transform.position = scorePosition.position;
				} 
				else
				{
					timerActive = 0;
				}
				
		}
		
		if(timerActive > 0)
		{
			scoreIsActive = true;
		}

		
	}
}
