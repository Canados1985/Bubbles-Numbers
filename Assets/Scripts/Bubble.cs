using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {


   public SpriteRenderer spriteRenderer;

   public GameObject go_bubble_parent;
   public GameObject go_bubble_child;
   public GameObject go_BubbleSize;

   public GameObject go_Bubble_nuked_Sprite;
   public MeshRenderer bubbleScoreRenderer;
   public GameObject bubbleAnimPrefab;
   public Rigidbody2D rb_bubble;


   GameManager gameManagerRef; 

   Player playerRef; 
   public Animator animator_Child;
   
   public TextMesh bubbleSizeOnText; 

    //*********************** BUBBLE SPECIFICATIONS ***********************//

    public string startName;
    public float timerAwaking;
    bool bubbleIsModified = false;
    public bool collidesWithExplosion = false;
    public bool bubbleIsMoving = false;

    public bool isNuked = false;
    int randomSprite;
    float randomRotaion;
    public float randomScaleStart;
    public float timeNotActive; // this we use for track status size bubble after it turns invisable
    public int bubbleSize;
    public int storeBubbleSize;
    public int storeBubbleSizeExplosion;

    public int NUMBER_in_the_list;

    int tempEdgeArgument;

    bool bubbleInTopPortal = false;
    bool bubbleInBottomPortal = false;
    bool bubbleInLeftPortal = false;
    bool bubbleInRightPortal = false;
    


    //*********************************************************************//

    // Use this for initialization
    void Start () { 

        InitBubble();
        TakeRandomPosition();
	}

    void InitBubble()
    {

        gameManagerRef = GameManager.gameManager;
        playerRef = gameManagerRef.playerRef; //GameObject.Find("player").GetComponent<Player>();
        go_BubbleSize = GameObject.Find("bubble_size");
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        animator_Child = gameObject.GetComponentInChildren<Animator>();
        bubbleSizeOnText = gameObject.GetComponentInChildren<TextMesh>();
        bubbleScoreRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        bubbleScoreRenderer.enabled = true; // Making active child Mesh Renderer 
        
        go_bubble_parent = gameObject;
        go_bubble_child = go_bubble_parent.gameObject;
        go_Bubble_nuked_Sprite.SetActive(false); // Set not active nuked sprite here

        bubbleInTopPortal = false;
        bubbleInBottomPortal = false;
        bubbleInLeftPortal = false;
        bubbleInRightPortal = false;
        bubbleIsModified = false;
        collidesWithExplosion = false;
        bubbleIsMoving = false;
        isNuked = false;

        rb_bubble = gameObject.GetComponent<Rigidbody2D>();
        rb_bubble.simulated = true;
        go_bubble_parent.transform.parent = null;
               
        rb_bubble.gravityScale = 0f; // gravity is here
        rb_bubble.angularDrag = 0.05f; // angular drag
        rb_bubble.drag = 0.8f; // linear drag
        randomRotaion = Random.Range(-1f, 1f);
        randomSprite = Random.Range(0, 7);
        
        go_bubble_parent.transform.localScale = new Vector3(0,0,0);
        //this.transform.parent = gameManagerRef.stateManagerRef.go_GameState.transform; // here we change parent location for each bubble
    }

    public void TakeModificators()
    {
        randomScaleStart = Random.Range(1f, 2f) / 10; // set speed scale for bubbles
        bubbleSize = Random.Range(1, 20);

        if (randomSprite == 0)
        { 
            
            animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[0];
            gameObject.name = "blue";
            startName = gameObject.name;
            Color newColor = new Color(0.0f, 0.9f, 0.9f, 1.0f);
            bubbleSizeOnText.color = newColor;
        }
        if (randomSprite == 1)
        { 
               
             animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[1];
             gameObject.name = "green";
             startName = gameObject.name;
             Color newColor = new Color(0.4f, 0.8f, 0.5f, 1.0f);
             bubbleSizeOnText.color = newColor; 
        }
        if (randomSprite == 2) 
        { 
            
            animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[2];
            gameObject.name = "pink";
            startName = gameObject.name;
            Color newColor = new Color(0.9f, 0.0f, 0.43f, 1.0f);
            bubbleSizeOnText.color = newColor;
        }
        if (randomSprite == 3) 
        { 
            
            animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[3];
            gameObject.name = "purple";
            startName = gameObject.name; 
            bubbleSizeOnText.color = Color.magenta;
        }
        if (randomSprite == 4) 
        { 
            
            animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[4];
            gameObject.name = "red";
            startName = gameObject.name;
            Color newColor = new Color(0.9f, 0.3f, 0.0f, 1.0f);
            bubbleSizeOnText.color = newColor; 
        }
        if (randomSprite == 5) 
        { 
            
            animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[5];
            gameObject.name = "white";
            startName = gameObject.name; 
            bubbleSizeOnText.color = Color.white;
        }
        if (randomSprite == 6) 
        { 
            
            animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[6];  
            gameObject.name = "yellow";
            startName = gameObject.name; 
            bubbleSizeOnText.color = Color.yellow;
        }
       
        bubbleIsModified = true; // control bool to make this method happens ones
        ScaleControll(bubbleSize);
        storeBubbleSize = bubbleSize;
        storeBubbleSizeExplosion = bubbleSize;

        
        gameManagerRef.progressBar.f_fill += (float)bubbleSize / 1000; // here we make effect on player progress bar on UI
        
        int randomSoundPopUp = Random.Range(0,7);
         if(randomSoundPopUp == 0){FindObjectOfType<AudioManager>().Play("bubble_popup_01");}
        if(randomSoundPopUp == 1){FindObjectOfType<AudioManager>().Play("bubble_popup_02");}
        if(randomSoundPopUp == 2){FindObjectOfType<AudioManager>().Play("bubble_popup_03");}
        if(randomSoundPopUp == 3){FindObjectOfType<AudioManager>().Play("bubble_popup_04");}
        if(randomSoundPopUp == 4){FindObjectOfType<AudioManager>().Play("bubble_popup_05");}
        if(randomSoundPopUp == 5){FindObjectOfType<AudioManager>().Play("bubble_popup_06");} 
        if(randomSoundPopUp == 6){FindObjectOfType<AudioManager>().Play("bubble_popup_07");}


    }

    //HERE WE SET UP RANDOM POSITION FOR NEW BUBBLES
    void TakeRandomPosition()
    {
        //Debug.Log(0 - GameManager.gameManager.playableWidth * 0.515f);
        //Debug.Log(GameManager.gameManager.playableWidth * 0.515f);
        //Debug.Log(0 - GameManager.gameManager.playableHeight * 0.5f);
        //Debug.Log(GameManager.gameManager.playableHeight * 0.5f);


        float playerPosX = playerRef.transform.transform.position.x;
        float playerPosY = playerRef.transform.transform.position.y;

        float randomSetStart = Random.Range(0,4);

		float position_x = Random.Range(gameManagerRef.go_LeftPlatforms_Container.transform.position.x + 1.75f, gameManagerRef.go_RightPlatforms_Container.transform.position.x - 1.75f);
        float position_y = Random.Range(gameManagerRef.go_BottonPlatforms_Container.transform.position.y + 1.75f, gameManagerRef.go_TopPlatforms_Container.transform.position.y - 1.75f);
        go_bubble_parent.transform.position = new Vector3(position_x, position_y); 
    } 

    
    ////// Pool Score Text on Screen and apply color, if we pool extra we instantiate new and add it to the same container ///////
    public GameObject GetScore(int argument, Color newColor, string newEnding, string sign, Vector2 position, bool scoreStatus, string scoreName, bool damageFromBomb)
    {

        for(int i=0; i<gameManagerRef.score.Count; i++)
        {
            if(gameManagerRef.score[i].activeInHierarchy && gameManagerRef.score[i].name == "BubbleChangeArgument_" + NUMBER_in_the_list.ToString() && !damageFromBomb)
            { gameManagerRef.score[i].SetActive(false); }

            else if(!gameManagerRef.score[i].activeInHierarchy)
            {
                gameManagerRef.score[i].SetActive(true);
                gameManagerRef.score[i].name = scoreName;
                gameManagerRef.score[i].transform.position = position;
                Score scoreTemp = gameManagerRef.score[i].GetComponent<Score>();
                scoreTemp.timerActive = 10;
                scoreTemp.GettingNumber(argument, newColor, newEnding, sign, this.transform, scoreStatus);               
                return gameManagerRef.score[i];
            }
            
        }    
         
        GameObject temp = Instantiate(gameManagerRef.score_Prefab);
        Score newScoreTemp = temp.GetComponent<Score>();
        newScoreTemp.timerActive = 10;

        gameManagerRef.score.Add(temp);
        temp.transform.position = this.gameObject.transform.position;
        temp.SetActive(true);
        temp.name = scoreName;
        newScoreTemp.GettingNumber(argument, newColor, newEnding, sign, this.transform, scoreStatus);
        
        return null;
        
    }

    //Deactivation Bubble
    public GameObject GetBubbleEnd()
    {

        int randomSoundPopUp = Random.Range(0,8);
        if(randomSoundPopUp == 0){FindObjectOfType<AudioManager>().Play("bubble_expl_01");}
        if(randomSoundPopUp == 1){FindObjectOfType<AudioManager>().Play("bubble_expl_02");}
        if(randomSoundPopUp == 2){FindObjectOfType<AudioManager>().Play("bubble_expl_03");}
        if(randomSoundPopUp == 3){FindObjectOfType<AudioManager>().Play("bubble_expl_04");}
        if(randomSoundPopUp == 4){FindObjectOfType<AudioManager>().Play("bubble_expl_05");}
        if(randomSoundPopUp == 5){FindObjectOfType<AudioManager>().Play("bubble_expl_06");}
        if(randomSoundPopUp == 6){FindObjectOfType<AudioManager>().Play("bubble_expl_07");}
        if(randomSoundPopUp == 7){FindObjectOfType<AudioManager>().Play("bubble_expl_08");}

        for(int i=0; i<gameManagerRef.bubblesEnd.Count; i++)
        {
            if(!gameManagerRef.bubblesEnd[i].activeInHierarchy)
            {
                gameManagerRef.bubblesEnd[i].name = this.gameObject.name;
                gameManagerRef.bubblesEnd[i].SetActive(true);
                gameManagerRef.bubblesEnd[i].transform.position = this.gameObject.transform.position;
                BubbleEnd tempBubbleEnd = gameManagerRef.bubblesEnd[i].GetComponent<BubbleEnd>();
                tempBubbleEnd.BubbleEndInst(this.transform.localScale);
                return gameManagerRef.bubblesEnd[i];
                
            }
        }

        GameObject temp = Instantiate(gameManagerRef.bubbleEndPrefab);
        temp.name = this.gameObject.name;
        temp.SetActive(true);
        temp.transform.position = this.gameObject.transform.position;
        return temp;
    }

    public GameObject GetImpactEffect(bool panelPositive, Vector3 newPosition, string name)
    {
        if(panelPositive)
        {
            for(int i=0; i<gameManagerRef.effects.Count; i++)
            {
                if(!gameManagerRef.effects[i].activeInHierarchy && gameManagerRef.effects[i].name == "impact_GREEN")
                {

                    gameManagerRef.effects[i].SetActive(true);
                    if(name == "LeftPlatform") { gameManagerRef.effects[i].transform.position = new Vector2(newPosition.x - 0.15f, newPosition.y);  }
                    if(name == "RightPlatform") { gameManagerRef.effects[i].transform.position = new Vector2(newPosition.x + 0.15f, newPosition.y);  }
                    if(name == "TopPlatform") { gameManagerRef.effects[i].transform.position = new Vector2(newPosition.x, newPosition.y + 0.15f);  }
                    if(name == "BottomPlatform") { gameManagerRef.effects[i].transform.position = new Vector2(newPosition.x, newPosition.y - 0.15f);  }
                    
                    Impact tempBubbleEnd = gameManagerRef.effects[i].GetComponent<Impact>();
                    return gameManagerRef.effects[i];

                }
            }
        }
        if(!panelPositive)
        {
            for(int i=0; i<gameManagerRef.effects.Count; i++)
            {
                if(!gameManagerRef.effects[i].activeInHierarchy && gameManagerRef.effects[i].name == "impact_RED")
                {

                    gameManagerRef.effects[i].SetActive(true);
                    if(name == "LeftPlatform") { gameManagerRef.effects[i].transform.position = new Vector2(newPosition.x - 0.25f, newPosition.y);  }
                    if(name == "RightPlatform") { gameManagerRef.effects[i].transform.position = new Vector2(newPosition.x + 0.25f, newPosition.y);  }
                    if(name == "TopPlatform") { gameManagerRef.effects[i].transform.position = new Vector2(newPosition.x, newPosition.y + 0.25f);  }
                    if(name == "BottomPlatform") { gameManagerRef.effects[i].transform.position = new Vector2(newPosition.x, newPosition.y - 0.25f);  }
                    Impact tempBubbleEnd = gameManagerRef.effects[i].GetComponent<Impact>();
                    return gameManagerRef.effects[i];

                }
            }    

        }

        /// NOT FINISHED ****************************************
        GameObject temp = Instantiate(gameManagerRef.impact_Prefab);
        temp.name = this.gameObject.name;
        temp.SetActive(true);
        temp.transform.position = this.gameObject.transform.position;
        return temp;
        ///// **************************************************
    }


    /////WE DESTROY BUBBLES IF PLAYER PICK UP THE BOMB EVENT/////
    public void OnCollisionWithExplosion(Bubble bubbleRef)
    {
        if(bubbleRef == this)
        {
            this.storeBubbleSizeExplosion = this.bubbleSize;

            GetScore(this.storeBubbleSizeExplosion, this.bubbleSizeOnText.color, " pts", " +", this.transform.position, false, "BubbleChangeArgument_" + NUMBER_in_the_list.ToString(), true);
            gameManagerRef.progressBar.f_fill -= (float)storeBubbleSizeExplosion / 1000;
            playerRef.playerScore += this.storeBubbleSizeExplosion;
            playerRef.playerKills +=1;
            GetBubbleEnd();
            Invoke("DestroyBubble", 0.005f);  
        }
   
    }
    ////////////////////////////////////////////////////////////

    
    /////WE DESTROY BUBBLES IF PLAYER PICK UP THE BOMB EVENT/////
    public void OnCollisionWithNuke(Bubble bubbleRef)
    {
        if(bubbleRef == this)
        {
            int RandomDamage = Random.Range(1,21);
            this.storeBubbleSizeExplosion = this.bubbleSize;

            this.bubbleSize -= RandomDamage;
            if(this.bubbleSize <= 0)
            {
                GetScore(this.storeBubbleSizeExplosion, this.bubbleSizeOnText.color, " pts", " +", this.transform.position, false, "playerGetsScore", true);
                gameManagerRef.progressBar.f_fill -= (float)storeBubbleSizeExplosion / 1000;
                playerRef.playerScore += this.storeBubbleSizeExplosion;
                playerRef.playerKills +=1;
                GetBubbleEnd();
                Invoke("DestroyBubble", 0.005f); 
            }
            else
            {
                isNuked = true;
                GetScore(RandomDamage, Color.red, " ", " -", this.transform.position, true, "BubbleChangeArgument_" + NUMBER_in_the_list.ToString(), false);
                gameManagerRef.progressBar.f_fill -= (float)RandomDamage / 1000;
                bubbleScoreRenderer.enabled = false;
                timeNotActive = 0;
                storeBubbleSize = bubbleSize;
                ScaleControll(bubbleSize);

            }    

 
        }
   
    }
    ////////////////////////////////////////////////////////////

    /////WE DESTROY BUBBLES IF PLAYER PICK UP THE ENERGY/////
    public void OnCollisionWithEnergy(Bubble bubbleRef)
    {
        if(bubbleRef == this)
        {   
            this.storeBubbleSize = this.bubbleSize;

            GetScore(this.storeBubbleSize, this.bubbleSizeOnText.color, " pts", " +", this.transform.position, false, "playerGetsScore", true);
            gameManagerRef.progressBar.f_fill -= (float)storeBubbleSize / 1000;
            playerRef.playerScore += this.storeBubbleSize;
            playerRef.playerKills +=1;
            GetBubbleEnd();
            Invoke("DestroyBubble", 0.005f);  
        }
    }
    ////////////////////////////////////////////////////////////



    /////////////////  BUBBLE HITS TELEPORT PANNEL /////////////////////////
    public void BubbleTeleportOnImpactPortal(EdgeBoard temp)
    {
        FindObjectOfType<AudioManager>().Play("portal"); 
        if(temp.gameObject.name == "TopPlatform")
        {  
            playerRef.bottomPortalPlatform.child_capsuleCollider2D.isTrigger = true;
            this.transform.position = new Vector2(playerRef.bottomPortalPlatform.transform.position.x, playerRef.bottomPortalPlatform.transform.position.y);
            rb_bubble.velocity = rb_bubble.velocity * -1;
            bubbleInBottomPortal = true;

        }
        if(temp.gameObject.name == "BottomPlatform")
        {
            playerRef.topPortalPlatform.child_capsuleCollider2D.isTrigger = true;
            this.transform.position = new Vector2(playerRef.topPortalPlatform.transform.position.x, playerRef.topPortalPlatform.transform.position.y);
            rb_bubble.velocity = rb_bubble.velocity * -1;
            bubbleInTopPortal = true;

        }
        if(temp.gameObject.name == "LeftPlatform")
        {
            playerRef.rightPortalPlatform.child_capsuleCollider2D.isTrigger = true;
            this.transform.position = new Vector2(playerRef.rightPortalPlatform.transform.position.x, playerRef.rightPortalPlatform.transform.position.y);
            rb_bubble.velocity = rb_bubble.velocity * -1;
            bubbleInRightPortal = true;

        } 
        if(temp.gameObject.name == "RightPlatform")
        {
            playerRef.leftPortalPlatform.child_capsuleCollider2D.isTrigger = true;
            this.transform.position = new Vector2(playerRef.leftPortalPlatform.transform.position.x, playerRef.leftPortalPlatform.transform.position.y);
            rb_bubble.velocity = rb_bubble.velocity * -1;
            bubbleInLeftPortal = true;
        }
                
    }

    //////// HERE PLAYER STARTS TELEPORT  ////////
    void OnTriggerExit2D(Collider2D collider)
	{

        if(collider.gameObject.name == "EdgeSpriteCollider")
		{
			EdgeBoard tempEdgeBoard = collider.transform.parent.gameObject.GetComponent<EdgeBoard>();
            if(tempEdgeBoard.panelIsPortal)
            {
                if(playerRef.bottomPortalPlatform != null){ playerRef.bottomPortalPlatform.child_capsuleCollider2D.isTrigger = false; bubbleInBottomPortal = false; }
                if(playerRef.topPortalPlatform != null){ playerRef.topPortalPlatform.child_capsuleCollider2D.isTrigger = false; bubbleInTopPortal = false; }
                if(playerRef.rightPortalPlatform !=  null) { playerRef.rightPortalPlatform.child_capsuleCollider2D.isTrigger = false; bubbleInRightPortal = false; }
                if(playerRef.leftPortalPlatform != null) { playerRef.leftPortalPlatform.child_capsuleCollider2D.isTrigger = false; bubbleInLeftPortal = false; } 
            }
		}
        	
	}
    ///////////////////////////////////////////////
    


    ///////////////////////////////////////////////////////////////////////

    //////////////////// ON COLLISION STAY AVOID BUBBLES STAY CLOSE TO EACH OTHER ++++ PLAYER //////////////////////////
    void OnCollisionStay2D(Collision2D collision)
    {
        //Checking collison with others bubbles
        if((collision.gameObject.name == "blue" || collision.gameObject.name == "green" || collision.gameObject.name == "pink" || collision.gameObject.name == "red" ||
           collision.gameObject.name == "purple" || collision.gameObject.name == "white" || collision.gameObject.name == "yellow")
           
           || (collision.gameObject.name == "blue_idle" || collision.gameObject.name == "green_idle" || collision.gameObject.name == "pink_idle" || collision.gameObject.name == "red_idle" ||
           collision.gameObject.name == "purple_idle" || collision.gameObject.name == "white_idle" || collision.gameObject.name == "yellow_idle")
            
           ) 
           {
                var newDirection = this.transform.position - collision.transform.position;   
                rb_bubble.AddForce(newDirection * (1f + 5));
           }

        if(collision.gameObject.transform.name == "EdgeSpriteCollider" && 
        (bubbleInBottomPortal || bubbleInTopPortal || bubbleInLeftPortal || bubbleInRightPortal) || collision.gameObject.transform.name == "EdgeSpriteCollider" && !gameManagerRef.gameIsStarts && 
        (!bubbleInBottomPortal || !bubbleInTopPortal || !bubbleInLeftPortal || !bubbleInRightPortal) 
        )
        {
            if(bubbleInBottomPortal)
            {
                var newDirection = this.transform.position + collision.transform.position * 1.5f;   
                rb_bubble.AddForce(newDirection * (1f + 3));
                bubbleInBottomPortal = false;
            }
            if(bubbleInTopPortal)
            {
                var newDirection = this.transform.position - collision.transform.position * 1.5f;   
                rb_bubble.AddForce(newDirection * (1f + 3));
                bubbleInTopPortal = false;
            }
            if(bubbleInLeftPortal)
            {
                var newDirection = this.transform.position + collision.transform.position * 1.5f;   
                rb_bubble.AddForce(newDirection * (1f + 3));
                bubbleInLeftPortal = false;
            }
            if(bubbleInRightPortal)
            {
                var newDirection = this.transform.position - collision.transform.position * 1.5f;   
                rb_bubble.AddForce(newDirection * (1f + 3));
                bubbleInRightPortal = false;
            }


            if(collision.gameObject.transform.parent.name == "TopPlatform")
            {
                var newDirection = this.transform.position - collision.transform.position * 1.5f;   
                rb_bubble.AddForce(newDirection * (1f + 1));
            }
            if(collision.gameObject.transform.parent.name == "BottomPlatform")
            {
                var newDirection = this.transform.position + collision.transform.position * 1.5f;   
                rb_bubble.AddForce(newDirection * (1f + 1));
            }
            if(collision.gameObject.transform.parent.name == "LeftPlatform")
            {
                var newDirection = this.transform.position - collision.transform.position * 1.5f;   
                rb_bubble.AddForce(newDirection * (1f + 1));
            }
            if(collision.gameObject.transform.parent.name == "RightPlatform")
            {
                var newDirection = this.transform.position + collision.transform.position * 1.5f;   
                rb_bubble.AddForce(newDirection * (1f + 1));
            }

           
        }

            if(collision.gameObject.name == "player_idle")
            {
                var newDirection = this.transform.position - collision.transform.position;   
                rb_bubble.AddForce(newDirection * (1f + bubbleSize));
            }

            if(collision.gameObject.name == "Bomb_Loot" || collision.gameObject.name == "Copy_Loot" || 
				collision.gameObject.name == "Life_Loot" || 
               collision.gameObject.name == "Lightning_Loot" || collision.gameObject.name == "Shield_Loot" || 
			   collision.gameObject.name == "Frozend_Loot" || collision.gameObject.name == "Poison_Loot"
              )
            {
                var newDirection = this.transform.position - collision.transform.position;   
                rb_bubble.AddForce(newDirection * (1f + 15));
            }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnCollisionEnter2D(Collision2D collision)
    {   

        //////// HERE IS COLLISION WITH PLAYER IF IT"S ACTIVE  /////////     
       if(playerRef != null && this.gameObject.activeSelf == true)
       {
            if(collision.gameObject.name == "player" && gameManagerRef.gameIsStarts || collision.gameObject.name == "player_idle" && gameManagerRef.gameIsStarts)
            {   
                int tempPlayerSize = playerRef.playerBubbleSize; // store temp Player's bubble size
                int tempThisSize = bubbleSize; // store temp Enemy bubble size
                bubbleSize -= tempPlayerSize; // here calculating enemy's bubble size
                storeBubbleSize = bubbleSize;
                storeBubbleSizeExplosion = bubbleSize;        
                if(playerRef.frozenDurability > 0){ bubbleSize -= bubbleSize; FindObjectOfType<AudioManager>().Play("impact_W_ICE"); } //If Player has frozen bubble

                //if player has NO ANY shield
                if(playerRef.shieldDurability <= 0 && playerRef.frozenDurability <= 0)
                {
                        playerRef.playerBubbleSize -=tempThisSize; // here calculating player's bubble argument after collision
                        if(playerRef.playerBubbleSize <=0)
                        {
                            playerRef.GetScore(1, Color.red, " Bubble", " -", "playerGetsScore", playerRef.transform.position, false);
                        }    
                        if(playerRef.playerBubbleSize > 0)
                        {
                            //Debug.Log("PLAYER SHOULD SEE MINUS");
                            playerRef.go_player_BubbleSize.SetActive(false);
                            playerRef.timeNotActive = 0;
                            playerRef.GetScore(tempThisSize, Color.red, " ", " -", "PlayerChangeArgument", playerRef.transform.position, true);
                            playerRef.ScaleControll(playerRef.playerBubbleSize); // set new scale for player
                            ScaleControll(playerRef.playerBubbleSize); // Set new scale for player bubble
                        }
               
                }

                /// CHECKING THIS BUBBLE STATUS AFTER COLLISION WITH PLAYER    
                if(bubbleSize <= 0)
                {
                    
                    GetScore(tempThisSize, bubbleSizeOnText.color, " pts", " +", this.transform.position, false, "playerGetsScore", false);
                    playerRef.playerScore += tempThisSize;
                    playerRef.playerKills +=1;
                    gameManagerRef.progressBar.f_fill -= (float)tempThisSize / 1000;
                    GetBubbleEnd();
                    Invoke("DestroyBubble", 0.005f); 
                }
                else
                {
                    FindObjectOfType<AudioManager>().Play("impact_DOWN");
                    //GetScore(tempPlayerSize, Color.red," ", " -", this.transform.position, true, "changeArgument", true);
                    GetScore(tempPlayerSize, Color.red, " ", " -", this.transform.position, true, "BubbleChangeArgument_" + NUMBER_in_the_list.ToString(), false);
                    gameManagerRef.progressBar.f_fill -= (float)tempPlayerSize / 1000;
                    this.bubbleScoreRenderer.enabled = false;
                    this.timeNotActive = 0;
                    
                    ScaleControll(bubbleSize);
                }  

                if(playerRef.shieldDurability > 0)
                {
                    playerRef.shieldDurability -=1; // decrease durability for player's shield ability
                    //playerRef.GetScore(1, Color.red, " Shield", " -", "playerGetsScore", playerRef.transform.position, false);
                }
                if(playerRef.frozenDurability > 0)
                {
                    playerRef.frozenDurability -=1; // decrease durability for player's shield ability
                    //playerRef.GetScore(1, Color.red, " Freeze", " -", "playerGetsScore", playerRef.transform.position, false);
                }      
            
            }
        
       }


        //Checking collison with others bubbles
        if((collision.gameObject.name == "blue" || collision.gameObject.name == "green" || collision.gameObject.name == "pink" || collision.gameObject.name == "red" ||
           collision.gameObject.name == "purple" || collision.gameObject.name == "white" || collision.gameObject.name == "yellow")
             && !gameManagerRef.playerTurn && gameManagerRef.gameIsStarts
           
           || (collision.gameObject.name == "blue" || collision.gameObject.name == "green" || collision.gameObject.name == "pink" || collision.gameObject.name == "red" ||
           collision.gameObject.name == "purple" || collision.gameObject.name == "white" || collision.gameObject.name == "yellow")
            && playerRef.gameObject.activeSelf == false && playerRef != null  && !gameManagerRef.playerTurn && gameManagerRef.gameIsStarts

           || (collision.gameObject.name == "blue_idle" || collision.gameObject.name == "green_idle" || collision.gameObject.name == "pink_idle" || collision.gameObject.name == "red_idle" ||
           collision.gameObject.name == "purple_idle" || collision.gameObject.name == "white_idle" || collision.gameObject.name == "yellow_idle")
             && !gameManagerRef.playerTurn && gameManagerRef.gameIsStarts
           
           || (collision.gameObject.name == "blue_idle" || collision.gameObject.name == "green_idle" || collision.gameObject.name == "pink_idle" || collision.gameObject.name == "red_idle" ||
           collision.gameObject.name == "purple_idle" || collision.gameObject.name == "white_idle" || collision.gameObject.name == "yellow_idle")
            && playerRef.gameObject.activeSelf == false && playerRef != null && !gameManagerRef.playerTurn && gameManagerRef.gameIsStarts
           ) 
        {   
            Bubble collisionBubble = collision.gameObject.GetComponent<Bubble>();
            this.bubbleScoreRenderer.enabled = false;
            this.timeNotActive = 0;
            gameManagerRef.helperRef.ControllOverBubbleCollideEachOther(collisionBubble, this);     

        }


        if(collision.gameObject.name == "PlayerCopy_Effect_0" || collision.gameObject.name == "PlayerCopy_Effect_1" || collision.gameObject.name == "PlayerCopy_Effect_2")
        {   
            PlayerCopy tempPlayerCopy = collision.gameObject.GetComponent<PlayerCopy>();

            int tempCopyPlayerSize = tempPlayerCopy.playerCopyBubbleSize; // store temp player's Copy bubble size
            int tempThisSize = bubbleSize; // store temp Enemy bubble size   
            bubbleSize -= tempCopyPlayerSize; // here calculating enemy's bubble size
            tempPlayerCopy.playerCopyBubbleSize -= Mathf.Abs(tempThisSize); // here calculating player's bubble argument after collision

            if(bubbleSize <= 0)
            {
                    bubbleSize = 0;
                    GetScore(tempThisSize, bubbleSizeOnText.color, " pts", " +", this.transform.position, false, "playerGetsScore", false);
                    tempPlayerCopy.GetScore(Mathf.Abs(tempThisSize), Color.red, " ", " -", "BubbleChangeArgument_" + NUMBER_in_the_list.ToString(), tempPlayerCopy.transform.position, true);
                    playerRef.playerScore += tempThisSize;
                    playerRef.playerKills +=1;
                    gameManagerRef.progressBar.f_fill -= (float)tempThisSize / 1000;
                    GetBubbleEnd();
                    Invoke("DestroyBubble", 0.005f); 
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("impact_DOWN");
                GetScore(Mathf.Abs(tempCopyPlayerSize), Color.red," ", " -", this.transform.position, true, "BubbleChangeArgument_" + NUMBER_in_the_list.ToString(), false);
                gameManagerRef.progressBar.f_fill -= (float)tempCopyPlayerSize / 1000;
                this.bubbleScoreRenderer.enabled = false;
                this.timeNotActive = 0;
                storeBubbleSize = bubbleSize;
                storeBubbleSizeExplosion = bubbleSize;
                ScaleControll(bubbleSize);
                
            } 
            if(tempPlayerCopy.playerCopyBubbleSize > 0)
            {   
                tempPlayerCopy.go_playerCopy_BubbleSize.SetActive(false);
                tempPlayerCopy.timeNotActive = 0;             
                tempPlayerCopy.GetScore(Mathf.Abs(tempThisSize), Color.red, " ", " -", tempPlayerCopy.name + "_ChangeArgument", tempPlayerCopy.transform.position, true);
                tempPlayerCopy.ScaleControll(tempPlayerCopy.playerCopyBubbleSize); // set new scale for player
            }   

        }


        //Checking collision with platfomrs 
        if(collision.gameObject.transform.name == "EdgeSpriteCollider" && bubbleIsMoving && playerRef.gameObject.activeSelf == true && playerRef != null && gameManagerRef.gameIsStarts || 
           collision.gameObject.transform.name == "EdgeSpriteCollider" && bubbleIsMoving && playerRef.gameObject.activeSelf == false && playerRef != null && gameManagerRef.gameIsStarts)
        {   
            EdgeBoard temp = collision.gameObject.transform.parent.GetComponent<EdgeBoard>();
            int tempBubbleSize = bubbleSize; // here we store bubble size by using new variable
            storeBubbleSize = bubbleSize;
            storeBubbleSizeExplosion = bubbleSize;
			if(temp.negativePositiveChance < 2  && !temp.panelIsPortal && !temp.panelIsZero)
			{
                FindObjectOfType<AudioManager>().Play("impact_UP");
				gameManagerRef.progressBar.f_fill += (float)temp.edgeArgument / 1000;
                //Here we need calculate number argument, make it bugger    
                tempEdgeArgument = temp.edgeArgument;
                bubbleSize += tempEdgeArgument;
                storeBubbleSize = bubbleSize;
                storeBubbleSizeExplosion = bubbleSize;
                this.bubbleScoreRenderer.enabled = false;
                this.timeNotActive = 0;
                GetScore(tempEdgeArgument, Color.green, " ", " +", collision.transform.position, true, "BubbleChangeArgument_" + NUMBER_in_the_list.ToString(), false);
                ScaleControll(bubbleSize); // Set new scale for this bubble
                //Debug.Log("COLLISION WITH POSITIVE EDGE " + tempEdgeArgument);
                GetImpactEffect(true, this.transform.position, temp.gameObject.name);
                

			}
			if(temp.negativePositiveChance >= 2  && !temp.panelIsPortal && !temp.panelIsZero)
			{
                //Here we need calculate number, make it smaller
                
                tempEdgeArgument = temp.edgeArgument;
                bubbleSize -= tempEdgeArgument;
                

                if(bubbleSize <= 0)
                {   
                    bubbleSize = 0;
                    GetScore(storeBubbleSize, bubbleSizeOnText.color, " pts", " +", this.transform.position, false, "playerGetsScore", false);
                    playerRef.playerScore += storeBubbleSize;
                    playerRef.playerKills +=1;
                    gameManagerRef.progressBar.f_fill -= (float)storeBubbleSize / 1000;
                    GetBubbleEnd();
                    Invoke("DestroyBubble", 0.005f); 
                }
                else
                {
                    FindObjectOfType<AudioManager>().Play("impact_DOWN");
                    GetScore(tempEdgeArgument, Color.red, " ", " -", collision.transform.position, true, "BubbleChangeArgument_" + NUMBER_in_the_list.ToString(), false);
                    this.bubbleScoreRenderer.enabled = false;
                    this.timeNotActive = 0;
                    storeBubbleSize = bubbleSize;
                    storeBubbleSizeExplosion = bubbleSize;
                    gameManagerRef.progressBar.f_fill -= (float)tempEdgeArgument / 1000;
                    ScaleControll(bubbleSize); // Set new scale for this bubble
                    
                }
                GetImpactEffect(false, this.transform.position, temp.gameObject.name);
			}
            if(temp.panelIsPortal && rb_bubble.velocity.magnitude > 0.25f)
            {
               FindObjectOfType<AudioManager>().Play("portal");
               BubbleTeleportOnImpactPortal(temp);     
            }
            if(temp.panelIsZero)
            {
                    bubbleSize = 0;
                    GetScore(storeBubbleSize, bubbleSizeOnText.color, " pts", " +", this.transform.position, false, "playerGetsScore", false);
                    playerRef.playerScore += storeBubbleSize;
                    playerRef.playerKills +=1;
                    gameManagerRef.progressBar.f_fill -= (float)storeBubbleSize / 1000;
                    GetBubbleEnd();
                    Invoke("DestroyBubble", 0.005f); 
            }

            
        }
        

    }


    ///////////////// WE USING THIS AFTER BUBBLE HITS OTHER BUBBLE TO GET RIGHT STORE BUBBLE SIZE /////////////////   
    void OnCollisionExit2D(Collision2D collision)
    {
        //Checking collison with others bubbles
        if((collision.gameObject.name == "blue" || collision.gameObject.name == "green" || collision.gameObject.name == "pink" || collision.gameObject.name == "red" ||
           collision.gameObject.name == "purple" || collision.gameObject.name == "white" || collision.gameObject.name == "yellow")
             && !gameManagerRef.playerTurn && gameManagerRef.gameIsStarts
           
           || (collision.gameObject.name == "blue" || collision.gameObject.name == "green" || collision.gameObject.name == "pink" || collision.gameObject.name == "red" ||
           collision.gameObject.name == "purple" || collision.gameObject.name == "white" || collision.gameObject.name == "yellow")
            && playerRef.gameObject.activeSelf == false && playerRef != null  && !gameManagerRef.playerTurn && gameManagerRef.gameIsStarts

           || (collision.gameObject.name == "blue_idle" || collision.gameObject.name == "green_idle" || collision.gameObject.name == "pink_idle" || collision.gameObject.name == "red_idle" ||
           collision.gameObject.name == "purple_idle" || collision.gameObject.name == "white_idle" || collision.gameObject.name == "yellow_idle")
             && !gameManagerRef.playerTurn && gameManagerRef.gameIsStarts
           
           || (collision.gameObject.name == "blue_idle" || collision.gameObject.name == "green_idle" || collision.gameObject.name == "pink_idle" || collision.gameObject.name == "red_idle" ||
           collision.gameObject.name == "purple_idle" || collision.gameObject.name == "white_idle" || collision.gameObject.name == "yellow_idle")
            && playerRef.gameObject.activeSelf == false && playerRef != null && !gameManagerRef.playerTurn && gameManagerRef.gameIsStarts
           ) 
        {   
            this.storeBubbleSize = this.bubbleSize;     
            this.storeBubbleSizeExplosion = this.bubbleSize;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void ScaleControll(int bubbleSize)
    {

            float scaleTempX = (float)(1.0f + (float)bubbleSize / 50.0f);
            float scaleTempY = (float)(1.0f + (float)bubbleSize / 50.0f);
            float scaleTempZ = (float)(1.0f + (float)bubbleSize / 50.0f);   

        go_bubble_parent.transform.localScale = new Vector3(
        scaleTempX, scaleTempY, scaleTempZ);
        
    }

    void SetTextSizeBasedOnScale()
    {
        ////// CONTROL OVER PLAYER"S BUBBLE TEXT //////
        if(bubbleSize < 10)
        {
            bubbleSizeOnText.fontSize = 300;
        }
        if(bubbleSize >= 10 && bubbleSize < 100)
        {
            bubbleSizeOnText.fontSize = 250;
        }
        if(bubbleSize >= 100)
        {
            bubbleSizeOnText.fontSize = 200;
        }
        ///////////////////////////////////////////////
    }


    public void DestroyBubble()
    {
        gameManagerRef.bubbles.Remove(this.gameObject);

        if(gameManagerRef.deleteGame)
        {
            GetBubbleEnd();
            gameManagerRef.progressBar.f_fill -= (float)tempEdgeArgument / 1000;
        }
        
        storeBubbleSize = 0;
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {


        ///// Timer for taking modifications on start() ///////////
        if(gameObject.activeSelf && timerAwaking > 0)
        {
            timerAwaking -= 0.05f;
        }
        if(timerAwaking <= 0 && !bubbleIsModified)
        {
            TakeModificators();
            timerAwaking = 0;
        }
        //////////////////////////////////////////////////////////


        /////////////////Checking velocity of the RB/////////////////////////////////
        if(rb_bubble.velocity.magnitude > 0.25f  && !gameManagerRef.playerTurn && !bubbleIsMoving)
        {
            
            bubbleIsMoving = true;
            this.name = startName;
            //Debug.Log("SetUp Bubble is Moving" + bubbleIsMoving);
        }
        if(rb_bubble.velocity.magnitude <= 0.25f && !gameManagerRef.playerTurn && bubbleIsMoving)
        {
            rb_bubble.velocity = Vector3.zero;
            //bubbleIsMoving = false;
            this.name = startName + "_idle";
            
            bubbleIsMoving = false;
            //Debug.Log("SetUp Bubble is Moving" + bubbleIsMoving);
        }
        if(rb_bubble.velocity.magnitude != 0)
        {
            go_bubble_child.transform.Rotate(new Vector3(0,0, rb_bubble.velocity.magnitude));
            
        }
        //////////////////////////////////////////////////////////////////////////////


        
        if(bubbleSize > 0 && this.bubbleScoreRenderer.enabled == false)
        {
            this.timeNotActive += 0.125f;
            if(timeNotActive >= 4.5f)
            {
                this.bubbleScoreRenderer.enabled = true;
                this.timeNotActive = 0;
            }
        }
        

    }

	// Update is called once per frame
	void Update () { 


        SetTextSizeBasedOnScale();     

        if(bubbleSize > 0)
        {
            bubbleSizeOnText.text = bubbleSize.ToString(); //Here we just make size text appears based on the random bubble size
        }
        else
        {
            bubbleSize = 0;
            bubbleSizeOnText.text = bubbleSize.ToString(); //Here we just make size text appears based on the random bubble size
        }
        if(isNuked)
        {
            go_Bubble_nuked_Sprite.SetActive(true);
        }
        
        
         
    }
}
