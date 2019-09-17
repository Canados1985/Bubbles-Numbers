using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour {

    public static Canvas canvas;
	GameStateManager gameStateManagerRef;
	public GameManager gameManagerRef;
	public Player playerRef;
	
	////////// GAME LOGO TEXT ///////////
	public Text gameLogoText;
	int counter = 0;
	string[] letters = new string [] {"B", "u" , "b", "b", "l", "e", "s", " & ", "N", "u", "m", "b", "e", "r", "s"};

	[SerializeField]
	Text[] lettersOnText = new Text[15];
	Color gameLogoColor;
	
	[SerializeField] private Gradient colorOverTime; 
    [SerializeField] private float timeMultiplier = 0.5f; 
  
    [SerializeField] private bool useThisText = false; 
  
    [SerializeField] private bool changeColor = false; 
    [SerializeField] private bool goBack = false; 
    private float currentTimeStep; 


	/////// LOGO BUBBLES ///////

	public GameObject bubble_01_Logo;
	public GameObject bubble_02_Logo;
	public GameObject bubble_03_Logo;
	public GameObject bubble_04_Logo;
	public GameObject bubble_05_Logo;
	public GameObject bubble_06_Logo;
	public GameObject bubble_07_Logo;
	float randomRotBubble_1_Logo;
	float randomRotBubble_2_Logo;
	float randomRotBubble_3_Logo;
	float randomRotBubble_4_Logo;
	float randomRotBubble_5_Logo;
	float randomRotBubble_6_Logo;
	float randomRotBubble_7_Logo;

	////////////////////////////


	////////////////////////////////////

	public Text playerScoreOnCanvas; 
	public Text playerLifesOnCanvas;
	public Text playerBubbleOnCanvas;
	public Text playerKillsOnCanvas; 

	public Text shieldDurability;

	public Text frozenDurability;


	//// PLAYERS SHIELD IMAGES ON CANVAS ////

	public GameObject shield_01;
	public GameObject frozen_01;

	/////////////////////////////////////////

	public GameObject imageMuteOFF;
	public GameObject imageMuteON;
	public GameObject panelFAQ;

	public GameObject panelCredits;

	//// RECORD TEXT THAT WE CAN SEE WHILE GAMEPLAY////

	public Text playerBiggestScoreGAMEPLAY;
	public Text playerBiggestBubbleGAMEPLAY;
	public Text playerBiggestKillGAMEPLAY;


	//// RECORD TEXT THAT WE CAN SEE WHILE MAIN MENU ////
	public Text playerHighScoreMENU;
	public Text playerBiggestBubbleSizeMENU;
	public Text playerBuggestKillsMENU;
	
	////////////////////////////////////////////////////

	//// RECORD TEXT THAT WE CAN SEE WHILE GAME OVER MENU ////
	public Text playerHighScoreGAMEOVER;
	public Text playerBiggestBubbleSizeGAMEOVER;
	public Text playerBuggestKillsGAMEOVER;


	public Text playerResultScoreGAMEOVER;
	public Text playerResulttBubbleSizeGAMEOVER;
	public Text playerResultKillsGAMEOVER;


	public Text playerResultScore_NEWRECORD_GAMEOVER;
	public Text playerResulttBubbleSize_NEWRECORD_GAMEOVER;
	public Text playerResultKills_NEWRECORD_GAMEOVER;
	
	////////////////////////////////////////////////////

	public Image blackCanvasGameOver;
	public Image blackCanvasGameStart;
	float newAlphaGameOver = 0.0f;	
	float newAlphaGameStart = 0.0f;	

	int score;
	int life;
	int bubbleSize;
	int playerBubbleSizeRecord;
	int playerKills;	

	public int record;

	public bool panelFAQIsActive = false;

	// Use this for initialization
	void Start () {
		
		canvas = this;
		gameStateManagerRef = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
		gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();

/* 		for (int i = 0; i < lettersOnText.Length; i++)
		{
			float randomR = Random.Range(0f,255f);
			float randomB = Random.Range(0f,255f);
			float randomG = Random.Range(0f,255f);
			Color newColor = new Color(randomR/255, randomB/255, randomG/255, 1.0f);
			lettersOnText[i].color = newColor;
			lettersOnText[i].text = letters[i];
			gameLogoText.text += lettersOnText[i].text;
		} */


 		for(int i=0; i<letters.Length; i++)
		{
			float randomR = Random.Range(0f,255f);
			float randomB = Random.Range(0f,255f);
			float randomG = Random.Range(0f,255f);
			Color newColor = new Color(randomR/255, randomB/255, randomG/255, 1.0f);
			gameLogoText.color = newColor;
			gameLogoText.text += letters[i];
		}


		randomRotBubble_1_Logo = Random.Range(0, 360);
		randomRotBubble_2_Logo = Random.Range(0, 360);
		randomRotBubble_3_Logo = Random.Range(0, 360);
		randomRotBubble_4_Logo = Random.Range(0, 360);
		randomRotBubble_5_Logo = Random.Range(0, 360);
		randomRotBubble_6_Logo = Random.Range(0, 360);
		randomRotBubble_7_Logo = Random.Range(0, 360);
		
		bubble_01_Logo.transform.Rotate(new Vector3(0,0, randomRotBubble_1_Logo));
		bubble_02_Logo.transform.Rotate(new Vector3(0,0, randomRotBubble_2_Logo));
		bubble_03_Logo.transform.Rotate(new Vector3(0,0, randomRotBubble_3_Logo));
		bubble_04_Logo.transform.Rotate(new Vector3(0,0, randomRotBubble_4_Logo));
		bubble_05_Logo.transform.Rotate(new Vector3(0,0, randomRotBubble_5_Logo));
		bubble_06_Logo.transform.Rotate(new Vector3(0,0, randomRotBubble_6_Logo));
		//bubble_07_Logo.transform.Rotate(new Vector3(0,0, randomRotBubble_7_Logo));

		panelFAQIsActive = false;	

    }

	public void SetPlayerRef(Player player)
	{
		playerRef = player;
	}


	// Update is called once per frame
	void Update () {
		
		if(!gameManagerRef.faqActive)
		{
			panelFAQIsActive = false;
			panelFAQ.SetActive(false);
			
		}
		else
		{
			panelFAQIsActive = true;
			panelFAQ.SetActive(true);
			
		}
		if(!gameManagerRef.creditsActive)
		{
			panelCredits.SetActive(false);
			
		}
		else
		{
			panelCredits.SetActive(true);
			
		}

		if(!gameManagerRef.mute)
		{
			imageMuteOFF.SetActive(true);
			imageMuteON.SetActive(false);
		}
		else
		{
			imageMuteOFF.SetActive(false);
			imageMuteON.SetActive(true);			
		}


		if(gameStateManagerRef.b_MenuMode)
		{
			playerHighScoreMENU.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
			playerBiggestBubbleSizeMENU.text = "Biggest Size: " + PlayerPrefs.GetInt("HighBubble").ToString();
			playerBuggestKillsMENU.text = "Most Kills: " + PlayerPrefs.GetInt("HighKills").ToString();

		}
		if(gameStateManagerRef.b_IsGameMode)
		{

			playerBiggestScoreGAMEPLAY.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
			playerBiggestBubbleGAMEPLAY.text = "Biggest Size: " + PlayerPrefs.GetInt("HighBubble").ToString();
			playerBiggestKillGAMEPLAY.text = "Most Kills: " + PlayerPrefs.GetInt("HighKills").ToString();
			
		}
		if(gameStateManagerRef.b_GameOverMode)
		{
			playerHighScoreGAMEOVER.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
			playerBiggestBubbleSizeGAMEOVER.text = "Biggest Size: " + PlayerPrefs.GetInt("HighBubble").ToString();
			playerBuggestKillsGAMEOVER.text = "Most Kills: " + PlayerPrefs.GetInt("HighKills").ToString();

			playerResultScoreGAMEOVER.text = "Your Score: " + score.ToString();
			playerResulttBubbleSizeGAMEOVER.text = "Your Size " + playerRef.playerBubbleSizeRecord.ToString();
			playerResultKillsGAMEOVER.text = "Your Kills " + playerKills.ToString();

			if(score >= PlayerPrefs.GetInt("HighScore"))
			{ playerResultScore_NEWRECORD_GAMEOVER.text = "NEW RECORD!!!"; }
			else { playerResultScore_NEWRECORD_GAMEOVER.text = " "; }

			if(playerRef.playerBubbleSizeRecord >= PlayerPrefs.GetInt("HighBubble"))
			{ playerResulttBubbleSize_NEWRECORD_GAMEOVER.text = "NEW RECORD!!!"; }
			else { playerResulttBubbleSize_NEWRECORD_GAMEOVER.text = " "; }
			if(playerKills >= PlayerPrefs.GetInt("HighKills"))
			{ playerResultKills_NEWRECORD_GAMEOVER.text = "NEW RECORD!!!"; }
			else { playerResultKills_NEWRECORD_GAMEOVER.text = " "; }
		}

		if(playerRef != null)
		{	
			
			score = playerRef.playerScore;
			playerScoreOnCanvas.text = "Score: " + score.ToString();

			life = playerRef.playerBubbleLife;
			playerLifesOnCanvas.text = "Lifes: " + life.ToString();


			bubbleSize = playerRef.playerBubbleSize;
			playerBubbleOnCanvas.text = "Size: " + bubbleSize.ToString();

			playerKills = playerRef.playerKills;
			playerKillsOnCanvas.text = "Kills: " + playerKills.ToString();





			if(playerRef.shieldDurability > 0)
			{
				shield_01.SetActive(true);
				shieldDurability.text = "x " + playerRef.shieldDurability.ToString();
			}
			if(playerRef.shieldDurability <= 0)
			{
				shield_01.SetActive(false);
			}

			if(playerRef.frozenDurability > 0)
			{
				frozen_01.SetActive(true);
				frozenDurability.text = "x " + playerRef.frozenDurability.ToString();
			}
			if(playerRef.frozenDurability <= 0)
			{
				frozen_01.SetActive(false);
			}

		}



		//If GAME OVER we make screen black and transparent again	
		if(gameManagerRef.GameIsOver && newAlphaGameOver != 1f)
		{

			newAlphaGameOver += 0.005f;
			blackCanvasGameOver.color = new Color (0.0f, 0.0f, 0.0f, newAlphaGameOver);
			if(newAlphaGameOver >= 1f && gameManagerRef.GameIsOver)
			{
				newAlphaGameOver = 0.0f;
				blackCanvasGameOver.color = new Color (0.0f, 0.0f, 0.0f, newAlphaGameOver);
				gameManagerRef.GameIsOver = false;
				gameStateManagerRef.GameOver();
			}
		}
		if(gameStateManagerRef.b_GameOverMode)
		{
			blackCanvasGameOver.color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
			newAlphaGameOver = 0.0f;
		}



		//If GAME STARTS we make screen black and transparent again	
		if(gameStateManagerRef.b_startButtonPressed && newAlphaGameStart != 1f)
		{

			newAlphaGameStart += 0.015f;
			blackCanvasGameStart.color = new Color (0.0f, 0.0f, 0.0f, newAlphaGameStart);
			if(newAlphaGameStart >= 1f && gameStateManagerRef.b_startButtonPressed)
			{
				newAlphaGameStart = 0.0f;
				blackCanvasGameStart.color = new Color (0.0f, 0.0f, 0.0f, newAlphaGameStart);
				gameStateManagerRef.b_startButtonPressed = false;
				gameStateManagerRef.InstStartGame();
			}
		}
		if(!gameStateManagerRef.b_startButtonPressed)
		{
			blackCanvasGameStart.color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
			newAlphaGameStart = 0.0f;
		}	

	}
}
