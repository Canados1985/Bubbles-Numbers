using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour {

	public static Helper helper;
	GameManager gameManagerRef;

  ///////  LOOT PREFABS   ///////
  public GameObject bomb_Loot;
  public GameObject copy_Loot;
  public GameObject life_Loot;
  public GameObject lightning_Loot;
  public GameObject shield_Loot;
  public GameObject frozen_Loot;
  public GameObject poison_Loot;

  /////////////////////////////

	public bool allBubbleMoving = true;
  public bool playerCanBeSpawnAgain = false;


	void Start () {
		helper = this;
		gameManagerRef = gameObject.GetComponent<GameManager>();
	}
	

	  // Here is the main player pick up bom event
  	public void PlayerPickUpBomb()
	{
  		for(int i=0; i<gameManagerRef.bubbles.Count; i++)
  		{
			  
     		Bubble tempBubble = gameManagerRef.bubbles[i].GetComponent<Bubble>();

            tempBubble.storeBubbleSize = tempBubble.bubbleSize;
            tempBubble.storeBubbleSizeExplosion = tempBubble.bubbleSize; // storing bubble size for once
      			tempBubble.bubbleSize -= 1;
      
      			if(tempBubble.bubbleSize <=0)
      			{   
          		tempBubble.GetScore(tempBubble.storeBubbleSizeExplosion, tempBubble.bubbleSizeOnText.color, " pts", " +", tempBubble.transform.position, false, "playerGetsScore", true);
          		gameManagerRef.progressBar.f_fill -= tempBubble.storeBubbleSize / 1000;
          		gameManagerRef.playerRef.playerScore += tempBubble.storeBubbleSize;
          		gameManagerRef.playerRef.playerKills +=1;
          		tempBubble.GetBubbleEnd(); // spawn bubble explosion object
         		 tempBubble.DestroyBubble(); // deactivate enemy bubble object if it has o strength
      			}
      			else
      			{	
		     	  tempBubble.GetScore(1, Color.red," ", " -", tempBubble.transform.position, true, "BubbleChangeArgument_" + tempBubble.NUMBER_in_the_list.ToString(), true);
        		gameManagerRef.progressBar.f_fill -= 0.001f;
        		tempBubble.bubbleScoreRenderer.enabled = false;
            tempBubble.timeNotActive = 0;
        		tempBubble.storeBubbleSize = tempBubble.bubbleSize;
            tempBubble.storeBubbleSizeExplosion = tempBubble.bubbleSize;
        		tempBubble.ScaleControll(tempBubble.storeBubbleSize);
      			}

			} 
	}


	//////////////////////////////////////////////////////////////////////////// 
 //////// WE KEEP LOGIC ABOUT BUBBLES COLLISION WITH EACH OTHER HERE ////////
  public void ControllOverBubbleCollideEachOther(Bubble collider, Bubble attackedBubble)
  {
    int colliderArgumentForPlayerProgress = collider.storeBubbleSize;
    attackedBubble.bubbleSize -= Mathf.Abs(collider.storeBubbleSize);

    if(attackedBubble.bubbleSize > 0)
    {
          attackedBubble.GetScore(collider.storeBubbleSize, Color.red," ", " -", collider.transform.position, true, "BubbleChangeArgument_" + attackedBubble.NUMBER_in_the_list.ToString(), false);
          attackedBubble.bubbleScoreRenderer.enabled = false;
          gameManagerRef.progressBar.f_fill -= (float)colliderArgumentForPlayerProgress / 1000;
          attackedBubble.timeNotActive = 0;
          attackedBubble.ScaleControll(attackedBubble.bubbleSize);
  
    }
    //if attacked destroyed
    if(attackedBubble.bubbleSize <= 0)
    {     


          gameManagerRef.playerRef.playerScore += attackedBubble.storeBubbleSize;
          gameManagerRef.playerRef.playerKills +=1;
          attackedBubble.GetScore(attackedBubble.storeBubbleSize, attackedBubble.bubbleSizeOnText.color, " pts", " +", attackedBubble.transform.position, false, "playerGetsScore", true);
          attackedBubble.GetBubbleEnd(); // call bubble end

          Debug.Log((float)attackedBubble.storeBubbleSize / 1000);

          gameManagerRef.progressBar.f_fill -= (float)attackedBubble.storeBubbleSize / 1000;
          collider.storeBubbleSize = collider.bubbleSize;
          collider.storeBubbleSizeExplosion = collider.bubbleSize;

          attackedBubble.Invoke("DestroyBubble", 0.005f); 
    }


  }
 
 ////////////////////////////////////////////////////////////////////////////
 ////////////////////////////////////////////////////////////////////////////
 

	void SpawnLootInGameWorld()
{

      if(!gameManagerRef.deleteGame)
      {
            int randomChanse = Random.Range(0,11); // Random chanse for call event 

            //Debug.Log(randomChanse);
           if(randomChanse >= 3 && !gameManagerRef.playerRef.playerLostBubble)
           {
            int randomItem = Random.Range(0,12);
              //Debug.Log(randomItem);

            //POISON  
            if(randomItem == 7)
            {
                Poison tempLoot = Instantiate(poison_Loot).GetComponent<Poison>();
                tempLoot.name = "Poison_Loot";
                tempLoot.gameObject.SetActive(true);

            }
            //FROZEN  
            if(randomItem == 6)
            {
                Frozen tempLoot = Instantiate(frozen_Loot).GetComponent<Frozen>();
                tempLoot.name = "Frozen_Loot";
                tempLoot.gameObject.SetActive(true);

            }
            //BOMB  
            if(randomItem == 5)
            {
                Bomb tempLoot = Instantiate(bomb_Loot).GetComponent<Bomb>();
                tempLoot.name = "Bomb_Loot";
                tempLoot.gameObject.SetActive(true);

            }
            //LIGHTNING
            if(randomItem == 4)
            {
                Lightning tempLoot = Instantiate(lightning_Loot).GetComponent<Lightning>();
                tempLoot.name = "Lightning_Loot";
                tempLoot.gameObject.SetActive(true);

            }
            //COPY
            if(randomItem == 3)
            {
                Copy tempLoot = Instantiate(copy_Loot).GetComponent<Copy>();
                tempLoot.name = "Copy_Loot";
                tempLoot.gameObject.SetActive(true);

            }
            //SHIELD
            if(randomItem == 2)
            {
                Shield tempLoot = Instantiate(shield_Loot).GetComponent<Shield>();
                tempLoot.name = "Shield_Loot";
                tempLoot.gameObject.SetActive(true);

            }
            //LIFE
            if(randomItem == 1)
            {
              int chance50 = Random.Range(0,10);
                if(chance50 >= 5)
                {
                  Life tempLoot = Instantiate(life_Loot).GetComponent<Life>();
                  tempLoot.name = "Life_Loot";
                  tempLoot.gameObject.SetActive(true);
                }
            }

          }

          Invoke("LetPlayerMakeNextTurn", 0.75f);

      }

}

void LetPlayerMakeNextTurn()
{

          if(!gameManagerRef.playerLostBubbleGM)
          {
              gameManagerRef.playerTurnTextUI.gameObject.SetActive(true); // Activation Canvas Text
          }
          else
          {
            gameManagerRef.rebornPlayerBubbleTextUI.gameObject.SetActive(true);
            playerCanBeSpawnAgain = true;
          } 
          gameManagerRef.notSpawnNewBubbles = false;
          gameManagerRef.playerTurn = true;
          gameManagerRef.touchEvent = false;
}

	void ResetPlayerTurnLabelText()
  {


    if(!gameManagerRef.deleteGame)
    {
            //Here we can set quantaty of the bubbles for respawn each time after player made turn 
            //Number randomChanse we can change base on how many bubbles we want to spawn after eact player's turn ---->
            //// HERE WE ADD RANDOMLY BubbleSize for enemy bubbles
            Bubble tempBubble;
            int randomChanse;
            for(int i=0; i<gameManagerRef.bubbles.Count; i++)
            { 
              tempBubble = gameManagerRef.bubbles[i].GetComponent<Bubble>();

              randomChanse = Random.Range(0,10);
              if(randomChanse == 1 || randomChanse == 2 || randomChanse == 3)
              { 
                
                if(!tempBubble.isNuked)
                {
                    tempBubble.bubbleSize += randomChanse; 

                    tempBubble.GetScore(randomChanse, tempBubble.bubbleSizeOnText.color, " ", " +", tempBubble.transform.position, false, "score_NoName", true);
                    tempBubble.ScaleControll(tempBubble.bubbleSize);
                    //tempBubble.bubbleScoreRenderer.enabled = false;
                    tempBubble.timeNotActive = 0;

                    //Debug.Log((float)randomChanse / 1000); /// we need to fix float here

                    gameManagerRef.progressBar.f_fill += (float)randomChanse / 1000; // apply effect on progress bar
                    
                }
                else
                {
                    //HERE WE CAN ADD DAMAGE FROM NUKED
                    tempBubble.bubbleSize -= randomChanse; 
                    tempBubble.GetScore(randomChanse, tempBubble.bubbleSizeOnText.color, " ", " -", tempBubble.transform.position, false, "score_NoName", true);
                    tempBubble.ScaleControll(tempBubble.bubbleSize);
                    //tempBubble.bubbleScoreRenderer.enabled = false;
                    tempBubble.timeNotActive = 0;
                    gameManagerRef.progressBar.f_fill -= (float)randomChanse / 1000; // apply effect on progress bar
                    
                      if(tempBubble.bubbleSize <= 0)
                      {
                          tempBubble.GetBubbleEnd();
                          tempBubble.Invoke("DestroyBubble", 0.005f); 
                      }
                }

              }

              tempBubble.storeBubbleSize = tempBubble.bubbleSize;
              tempBubble.storeBubbleSizeExplosion = tempBubble.bubbleSize;

            }

            //////// CHECKING THE PLAYER PROGRESS //////// 
            if(gameManagerRef.progressBar.f_fill >= 1)
            {
              gameManagerRef.playerRef.playerBubbleLife = 0;
              gameManagerRef.playerRef.playerBubbleSize = 0;
            }
            //////////////////////////////////////////////   

            //Here we can set quantaty of the bubbles for respawn each time after player made turn 
            //Number 5 we can change base on how many bubbles we want to spawn after eact player's turn ---->
            randomChanse = Random.Range(0,3);
            for (int i = 0; i <randomChanse; i++ )
            {
                gameManagerRef.bubbles.Add(Instantiate(gameManagerRef.bubblePrefab));
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //SET SPECIFIC NUMBER FOR EACH BUBBLE IN THE LIST
            for (int i = 0; i < gameManagerRef.bubbles.Count; i++)
            {
                Bubble newBubble = gameManagerRef.bubbles[i].GetComponent<Bubble>();
                newBubble.NUMBER_in_the_list = i;
            }
            ////////////////////////////////////////////////

            ///// HERE WE DO SUBSTRACTION FOR LOOT OBJECT EXISTING TIME /////
            for (int i=0; i<gameManagerRef.loot.Count; i++ )
            {
                if(gameManagerRef.loot[i].gameObject.activeSelf == true)
                { 
                   //Debug.Log("Loop is working");
                   Loot tempLoot = gameManagerRef.loot[i].GetComponent<Loot>();
                   tempLoot.lifeTime -= 1;
                }
            }
          

          /// HERE WE SPAWN LOOT OBJECTS ON TABLE  ///
          /// WE NEED TO MAKE THAT ONLY ONE OBJECT CAN ACTIVE AT ONE TIME ///
          
          SpawnLootInGameWorld();

    }  

  }

	  ///// SPAWN BUBBLES AFTER PLAYERS TURN //////
  	public void ResetPlatformsAfterPlayerTurn()    
    {
        
        gameManagerRef.chanceToHavePortalTopBottomPanel = Random.Range(0, 8);
        gameManagerRef.chanceToHavePortalLeftRightPanel = Random.Range(0, 4); 
        gameManagerRef.chanceToHavePotralOnTurn = Random.Range(0, 11);
        gameManagerRef.chanceToHaveZero = Random.Range(0, 5);

        if(!gameManagerRef.playerTurn && gameManagerRef.playerRef.name == "player" && !gameManagerRef.deleteGame)
        {   

            //Debug.Log("RESET PLATFORMS HERE");
            ///BEFORE WE SAID WHICH SIDE OF TABLE WILL HAVE A PORTALS
            int RandomChanceToHaveAllPortals = Random.Range(0,3);
            int RandomChanceToHaveZoroOnTopThisTurn = Random.Range(0, gameManagerRef.topPlatformsList.Count);
            int RandomChanceToHaveZoroOnBottomThisTurn = Random.Range(0, gameManagerRef.bottomPlatformsList.Count);
            int RandomChanceToHaveZoroOnRightThisTurn = Random.Range(0, gameManagerRef.rightPlatformsList.Count);
            int RandomChanceToHaveZoroOnLeftThisTurn = Random.Range(0, gameManagerRef.leftPlatformsList.Count);
            //Debug.Log("RESET PLATFORMS");
            ///////////// HERE WE CHANGE ARGUMENT ON EACH PLATFORM /////////////////
            for(int i=0; i<gameManagerRef.topPlatformsList.Count; i++)
            {
              EdgeBoard temp = gameManagerRef.topPlatformsList[i].GetComponent<EdgeBoard>();
              temp.child_capsuleCollider2D.isTrigger = false;
              temp.panelIsPortal = false;
              temp.panelIsZero = false;
              if(i==gameManagerRef.chanceToHavePortalTopBottomPanel && gameManagerRef.chanceToHavePotralOnTurn > 5 && RandomChanceToHaveAllPortals == 0
                || i==gameManagerRef.chanceToHavePortalTopBottomPanel && gameManagerRef.chanceToHavePotralOnTurn > 5 && RandomChanceToHaveAllPortals == 2
              )
              {
                temp.panelIsPortal = true;
              }

              if(gameManagerRef.chanceToHaveZero == 0 && !temp.panelIsPortal && i == RandomChanceToHaveZoroOnTopThisTurn)
              {
                temp.panelIsZero = true;
              }

              temp.TakeNewArgument();
            }
            for(int i=0; i<gameManagerRef.bottomPlatformsList.Count; i++)
            {
              EdgeBoard temp = gameManagerRef.bottomPlatformsList[i].GetComponent<EdgeBoard>();
              temp.child_capsuleCollider2D.isTrigger = false;
              temp.panelIsPortal = false;
              temp.panelIsZero = false;
              if(i==gameManagerRef.chanceToHavePortalTopBottomPanel && gameManagerRef.chanceToHavePotralOnTurn > 5  && RandomChanceToHaveAllPortals == 0
                || i==gameManagerRef.chanceToHavePortalTopBottomPanel && gameManagerRef.chanceToHavePotralOnTurn > 5 && RandomChanceToHaveAllPortals == 2
              )
              {
                temp.panelIsPortal = true;
              }

              if(gameManagerRef.chanceToHaveZero == 1 && !temp.panelIsPortal && i == RandomChanceToHaveZoroOnBottomThisTurn)
              {
                temp.panelIsZero = true;
              }  

              temp.TakeNewArgument();
            }
            for(int i=0; i<gameManagerRef.rightPlatformsList.Count; i++)
            {
              EdgeBoard temp = gameManagerRef.rightPlatformsList[i].GetComponent<EdgeBoard>();
              temp.child_capsuleCollider2D.isTrigger = false;
              temp.panelIsPortal = false;
              temp.panelIsZero = false;

              if(i==gameManagerRef.chanceToHavePortalLeftRightPanel && gameManagerRef.chanceToHavePotralOnTurn > 5  && RandomChanceToHaveAllPortals == 1
              || i==gameManagerRef.chanceToHavePortalTopBottomPanel && gameManagerRef.chanceToHavePotralOnTurn > 5 && RandomChanceToHaveAllPortals == 2
              )
              {
                temp.panelIsPortal = true;
              }

              if(gameManagerRef.chanceToHaveZero == 2 && !temp.panelIsPortal && i == RandomChanceToHaveZoroOnRightThisTurn)
              {
                temp.panelIsZero = true;
              }  


              temp.TakeNewArgument();
            }
            for(int i=0; i<gameManagerRef.leftPlatformsList.Count; i++)
            {
              EdgeBoard temp = gameManagerRef.leftPlatformsList[i].GetComponent<EdgeBoard>();
              temp.child_capsuleCollider2D.isTrigger = false;
              temp.panelIsPortal = false;
              temp.panelIsZero = false;

              if(i==gameManagerRef.chanceToHavePortalLeftRightPanel && gameManagerRef.chanceToHavePotralOnTurn > 5  && RandomChanceToHaveAllPortals == 1
              || i==gameManagerRef.chanceToHavePortalTopBottomPanel && gameManagerRef.chanceToHavePotralOnTurn > 5 && RandomChanceToHaveAllPortals == 2
              )
              {
                temp.panelIsPortal = true;
              }

              if(gameManagerRef.chanceToHaveZero == 3 && !temp.panelIsPortal && i == RandomChanceToHaveZoroOnLeftThisTurn)
              {
                temp.panelIsZero = true;
              }  

              temp.TakeNewArgument();
            }
            /////////////////////////////////////////////////////////////////////////
			    gameManagerRef.playerRef.name = "player_idle"; 
        	Invoke("ResetPlayerTurnLabelText", 1f);
        }


    }

	///////////////////////////////////////////////////////////////////////////////////////////
  ///////////////////////////////////////////////////////////////////////////////////////////
  //////////CHECK ENEMIE BUBBLES STATUS AND MOVING STATUS TO START RESET TABLE //////////////
  public void CheckEnemyBubbleStatus()
  {

	  if(!gameManagerRef.playerTurn && gameManagerRef.gameIsStarts && !gameManagerRef.deleteGame)
	  {
	  	for(int i=0; i<gameManagerRef.bubbles.Count; ++i)
      	{
     		  Bubble tempBubble = gameManagerRef.bubbles[i].GetComponent<Bubble>();

     	 	  if(tempBubble.rb_bubble.velocity != Vector2.zero)
     		  {
        		allBubbleMoving = true;
				    break;
				
      		}
			      allBubbleMoving = false;
    	  }

	  }

    if(!allBubbleMoving && !gameManagerRef.playerRef.playerLostBubble && !gameManagerRef.deleteGame && gameManagerRef.playerRef != null)
    {
        allBubbleMoving = false;
        gameManagerRef.gameIsStarts = false;
        Invoke("ResetPlatformsAfterPlayerTurn", 0.5f);
    }
    else if(!allBubbleMoving && gameManagerRef.playerRef.playerLostBubble && !gameManagerRef.deleteGame && gameManagerRef.playerRef != null)       
    {
        allBubbleMoving = false;
        gameManagerRef.gameIsStarts = false;
        Invoke("ResetPlatformsAfterPlayerTurn", 0.5f); 
    }
  }
  ////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////


	public void TrackingPlayerRecords()
	{
		    //////// HERE WE SET AND GET RECORD TRACKING LOGIC /////////
        if(gameManagerRef.playerRef != null)
        {
          gameManagerRef.highScore = gameManagerRef.playerRef.playerScore;
          gameManagerRef.highBubbleSize = gameManagerRef.playerRef.playerBubbleSize;
          gameManagerRef.highKills = gameManagerRef.playerRef.playerKills;
        }
        if(PlayerPrefs.GetInt("HighScore") < gameManagerRef.highScore)
        {
            PlayerPrefs.SetInt("HighScore", gameManagerRef.highScore);
        }
        if(PlayerPrefs.GetInt("HighBubble") < gameManagerRef.highBubbleSize)
        {
            PlayerPrefs.SetInt("HighBubble", gameManagerRef.highBubbleSize);
        }
        if(PlayerPrefs.GetInt("HighKills") < gameManagerRef.highKills)
        {
            PlayerPrefs.SetInt("HighKills", gameManagerRef.highKills);
        }
        ///////////////////////////////////////////////////////////

	}

  /////// CALL TOUCH EFFECT ON TOUCH EVENT
  GameObject GetTouchEffect(Vector2 position)
  {
      for (int i = 0; i < gameManagerRef.effects.Count; i++)
      {
          if(!gameManagerRef.effects[i].activeInHierarchy && gameManagerRef.effects[i].name == "Touch_Effect")
          { 
            Touch tempTouch = gameManagerRef.effects[i].GetComponent<Touch>();
            tempTouch.gameObject.SetActive(true);
            tempTouch.transform.position = position;
            tempTouch.SetTimerToZero();
            return tempTouch.gameObject;
          }
      } 

        GameObject temp = Instantiate(gameManagerRef.touch_Effect);
        Touch newTouch = temp.GetComponent<Touch>();
        temp.SetActive(true);
        temp.gameObject.name = "Touch_Effect";
        temp.transform.position = position;
        newTouch.SetTimerToZero();
        return temp;
  }
///////////////////////////////////
  public void TouchScreenEvent()
    {

      if(gameManagerRef.playerRef)
      {
        Vector3 mouseClick;
        Touch tempTouch = gameManagerRef.touch_Effect.GetComponent<Touch>();
        mouseClick = transform.position;
        Camera.main.ScreenPointToRay(Input.mousePosition);

        // HERE WE ACTIVATES PLAYERS MOVEMENT TOWARDS TOUCH EVENT
        if (Input.GetMouseButtonDown(0) && gameManagerRef.playerRef.gameObject.activeSelf && gameManagerRef.playerTurn && !playerCanBeSpawnAgain)
        {

                    mouseClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    gameManagerRef.notSpawnNewBubbles = true;
                    if(mouseClick.x > gameManagerRef.go_TopPlatforms_Container.transform.position.x + 0.15f && mouseClick.x < gameManagerRef.go_RightPlatforms_Container.transform.position.x - 0.15f &&
                       mouseClick.y < gameManagerRef.go_TopPlatforms_Container.transform.position.y + 0.5f && mouseClick.y > gameManagerRef.go_RightPlatforms_Container.transform.position.y - 0.5f && !gameManagerRef.mainCanvas.panelFAQIsActive
                      )
                    { 
                    
                      GetTouchEffect(mouseClick); // add touch effect on screen

                      gameManagerRef.playerRef.newPlayerPosition = mouseClick; //Here player starts moving
                      gameManagerRef.touchEvent = true;
                      gameManagerRef.playerTurn = false;
                      
                      gameManagerRef.playerTurnTextUI.gameObject.SetActive(false); // Remove UI text element from Canvas
                    }        

        }

        // HERE WE RESPAWN PLAYER'S BUBBLE
        if (Input.GetMouseButtonDown(0) && !gameManagerRef.playerRef.gameObject.activeSelf && gameManagerRef.playerRef.playerBubbleLife > 0)
        {

                    mouseClick = gameManagerRef.mainCamera.ScreenToWorldPoint(Input.mousePosition);

                    if(mouseClick.x > gameManagerRef.go_TopPlatforms_Container.transform.position.x + 1.25f && mouseClick.x < gameManagerRef.go_RightPlatforms_Container.transform.position.x - 1.25f &&
                       mouseClick.y < gameManagerRef.go_TopPlatforms_Container.transform.position.y - 0.5f && mouseClick.y > gameManagerRef.go_BottonPlatforms_Container.transform.position.y + 0.5f && !gameManagerRef.mainCanvas.panelFAQIsActive
                      )

                    {

                      if(playerCanBeSpawnAgain)

                      { 


                          gameManagerRef.playerLostBubbleGM = false;
                          gameManagerRef.playerRef.playerLostBubble = false;
                          gameManagerRef.playerRef.transform.position = new Vector2(mouseClick.x, mouseClick.y);
                          gameManagerRef.notSpawnNewBubbles = false;
                          gameManagerRef.gameIsStarts = false;
                          gameManagerRef.playerTurn = true;
                          gameManagerRef.playerRef.go_player_BubbleSize.SetActive(true);
                          gameManagerRef.playerRef.gameObject.SetActive(true); 
                          gameManagerRef.rebornPlayerBubbleTextUI.gameObject.SetActive(false); // this is UI text "TAP TO REBORN"
                          gameManagerRef.playerRef.playerBubbleSize = 1;
                          gameManagerRef.playerRef.transform.localScale = new Vector3(1,1,1);
                          
                          gameManagerRef.playerRef.name = "player_idle";
                          Invoke("ResetBoolSpawnPlayer", 1.0f);
                          int randomSoundPopUp = Random.Range(0,6);
                          if(randomSoundPopUp == 0){FindObjectOfType<AudioManager>().Play("bubble_popup_01");}
                          if(randomSoundPopUp == 1){FindObjectOfType<AudioManager>().Play("bubble_popup_02");}
                          if(randomSoundPopUp == 2){FindObjectOfType<AudioManager>().Play("bubble_popup_03");}
                          if(randomSoundPopUp == 3){FindObjectOfType<AudioManager>().Play("bubble_popup_04");}
                          if(randomSoundPopUp == 4){FindObjectOfType<AudioManager>().Play("bubble_popup_05");}
                          if(randomSoundPopUp == 5){FindObjectOfType<AudioManager>().Play("bubble_popup_06");}
                      }

                    }

        }
      }
    }
    
    void ResetBoolSpawnPlayer()
    {
        playerCanBeSpawnAgain = false;
        gameManagerRef.playerTurnTextUI.gameObject.SetActive(true);  // this is UI text "GO"
    }





    void Update()
    {
        /// HERE WE CHECKIN STATUS OF BUBBLES FOR RELOAD TABLE IF PLAYER LOST BUBBLE AN NOT ACTIVE ////
        if(gameManagerRef.playerLostBubbleGM)
        {
            CheckEnemyBubbleStatus();
        }
        
    }

    ////////////////////// SET SCREEN EDGES //////////////////////
    public void  SetScreenEdges()
    {

        if (!gameManagerRef.mainCamera.orthographic) { Debug.LogError("Camera.main is not Orthographic, failed to create edge colliders"); return; }

        var bottomLeft = (Vector2)gameManagerRef.mainCamera.ScreenToWorldPoint(new Vector3(0, 0, gameManagerRef.mainCamera.nearClipPlane));
        var topLeft = (Vector2)gameManagerRef.mainCamera.ScreenToWorldPoint(new Vector3(0, gameManagerRef.mainCamera.pixelHeight, gameManagerRef.mainCamera.nearClipPlane));
        var topRight = (Vector2)gameManagerRef.mainCamera.ScreenToWorldPoint(new Vector3(gameManagerRef.mainCamera.pixelWidth, gameManagerRef.mainCamera.pixelHeight, gameManagerRef.mainCamera.nearClipPlane));
        var bottomRight = (Vector2)gameManagerRef.mainCamera.ScreenToWorldPoint(new Vector3(gameManagerRef.mainCamera.pixelWidth, 0, gameManagerRef.mainCamera.nearClipPlane));

        // add or use existing EdgeCollider2D
        //var screenEdge = GetComponent<EdgeCollider2D>() == null ? gameObject.AddComponent<EdgeCollider2D>() : GetComponent<EdgeCollider2D>();

        var edgePoints = new[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
       // screenEdge.points = edgePoints;

        gameManagerRef.playableWidth = bottomLeft.x;
        gameManagerRef.playableHeight = bottomRight.y;

    }
    //////////////////////////////////////////////////////////////////

    //////////////////// METHOD FOR GETTING RANDOM POSITION ////////////////////
    public Vector2 TakeRandomPosition()
    {
		    float position_x = Random.Range(gameManagerRef.go_LeftPlatforms_Container.transform.position.x + 0.5f, gameManagerRef.go_RightPlatforms_Container.transform.position.x - 0.5f);
        float position_y = Random.Range(gameManagerRef.go_BottonPlatforms_Container.transform.position.y + 0.5f, gameManagerRef.go_TopPlatforms_Container.transform.position.y - 0.5f);

        Vector2 newPosition = new Vector2(position_x, position_y);
        
        return newPosition;
    } 
    //////////////////////////////////////////////////////////////////

}
