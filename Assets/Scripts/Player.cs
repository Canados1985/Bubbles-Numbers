using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player player;
    #region // Player Class Initialization
    void InitClass()
    {
            player = this;
    }
    #endregion

    public GameObject playerSpriteRenderer;
    public Animator animator_Child;

    public GameObject bubbleAnimPrefab;
    private GameObject go_player_parent;
    public GameObject go_player_BubbleSize;
    public GameObject go_player_shield_child;

    public GameObject go_frozen_shield_child;
    public Rigidbody2D rb_player;
    public GameManager gmInst;

    public Vector3 newPlayerPosition;

    public TextMesh playerSizeOnText;
    float playerSpeed;
    float randomScaleStart;

    public int tempBubbleNumber;

    public int playerScore;

    public int playerScoreRecord;
    public int playerKills;

    public int playerKillRecord;
    public int playerBubbleSizeRecord;
    public int playerBubbleLife;

    //*********************** PLAYER BUBBLE SPECIFICATIONS ***********************//

    public int playerBubbleSize; // here is player bubble number
    double scalePlayerBubble;
    public bool playerLostBubble;
    public bool playerIsMoving;
    bool playerFirstSpawn = true;
    bool playerInTopPortal = false;
    bool playerInBottomPortal = false;
    bool playerInLeftPortal = false;
    bool playerInRightPortal = false;
    
    public float timeNotActive; // this we use for track status size bubble after it turns invisable
    public int shieldDurability = 0;
    public int frozenDurability = 0;
    public float position_x;
    public float position_y;

    //*********************************************************************//


    /////// PORTAL PLATFORMS REFERENSE HERE /////////

    public EdgeBoard topPortalPlatform;
    public EdgeBoard bottomPortalPlatform;
    public EdgeBoard leftPortalPlatform;
    public EdgeBoard rightPortalPlatform;


    ////////////////////////////////////////////////


    void InitPlayer()
    {
        
        go_player_parent = gameObject;
        playerSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>().gameObject;
        animator_Child = gameObject.GetComponentInChildren<Animator>();
        go_player_shield_child = GameObject.Find("player_shield_sprite");
        go_frozen_shield_child = GameObject.Find("player_frozen_sprite");
        go_player_BubbleSize = GameObject.Find("player_bubble_size");
        go_player_shield_child.SetActive(false);    
        go_frozen_shield_child.SetActive(false);

        rb_player = gameObject.GetComponent<Rigidbody2D>();
        playerSizeOnText = gameObject.GetComponentInChildren<TextMesh>();
        go_player_parent.transform.parent = null;
        go_player_parent.name = "player_idle";

        rb_player.drag = 1f;
        rb_player.angularDrag = 0.5f;
        randomScaleStart = Random.Range(0.05f, 0.15f);    

        animator_Child.runtimeAnimatorController = gmInst.bubblesController[7];

        if(playerFirstSpawn)
        {
            position_x = Random.Range(0 - GameManager.gameManager.playableWidth * 0.525f, GameManager.gameManager.playableWidth * 0.525f);
            position_y = Random.Range(0 - GameManager.gameManager.playableHeight * 0.5f, GameManager.gameManager.playableHeight * 0.5f);
            go_player_parent.transform.position = new Vector3(position_x * 0.75f, position_y * 0.75f); // set start position for player
        }
        else
        {
            go_player_parent.transform.position = new Vector3(position_x, position_y); // here start position for player for other times
        }
        
        
        go_player_parent.transform.localScale = new Vector3(0,0,0); // set start scale for player

        playerBubbleSize = 1; // player size starts here
        scalePlayerBubble = 1; // set scale for player bubble 
        playerLostBubble = false;
        playerFirstSpawn = true;
        playerInTopPortal = false;
        playerInBottomPortal = false;
        playerInLeftPortal = false;
        playerInRightPortal = false;
        gmInst.mainCanvas.SetPlayerRef(this);

        int randomSoundPopUp = Random.Range(0,7);
         if(randomSoundPopUp == 0){FindObjectOfType<AudioManager>().Play("bubble_popup_01");}
        if(randomSoundPopUp == 1){FindObjectOfType<AudioManager>().Play("bubble_popup_02");}
        if(randomSoundPopUp == 2){FindObjectOfType<AudioManager>().Play("bubble_popup_03");}
        if(randomSoundPopUp == 3){FindObjectOfType<AudioManager>().Play("bubble_popup_04");}
        if(randomSoundPopUp == 4){FindObjectOfType<AudioManager>().Play("bubble_popup_05");}
        if(randomSoundPopUp == 5){FindObjectOfType<AudioManager>().Play("bubble_popup_06");} 
        if(randomSoundPopUp == 6){FindObjectOfType<AudioManager>().Play("bubble_popup_07");}
    }


    // Use this for initialization
    void Start () {

        gmInst = GameManager.gameManager;
        gmInst.playerRef = this;
        
        InitClass();
        InitPlayer();
        ScaleControll(playerBubbleSize);
        playerSpeed = rb_player.velocity.magnitude;
        playerScore = 0;
        playerBubbleLife = 3;
	}
    
    //Shows positive number here
    public GameObject GetScore(int argument, Color color, string newEnding, string sign, string scoreName, Vector2 position, bool scoreStatus)
    {

        for(int i=0; i<gmInst.score.Count; i++)
        {   
            if(gmInst.score[i].activeInHierarchy && gmInst.score[i].name == "PlayerChangeArgument")
            { gmInst.score[i].SetActive(false); }
            else if(!gmInst.score[i].activeInHierarchy)
            {
                //Debug.Log("SET ACTIVE SCORE");
                gmInst.score[i].SetActive(true);
                gmInst.score[i].name = scoreName;
                gmInst.score[i].transform.position = position;
                Score scoreTemp = gmInst.score[i].GetComponent<Score>();
                scoreTemp.timerActive = 10;
                scoreTemp.GettingNumber(argument, color, newEnding, sign, this.transform, scoreStatus);
                return gmInst.score[i];
            }
        }    

         
        GameObject temp = Instantiate(gmInst.score_Prefab);
        Score newScoreTemp = temp.GetComponent<Score>();
        newScoreTemp.timerActive = 10;

        gmInst.score.Add(temp);
        temp.transform.position = new Vector3(this.gameObject.transform.position.x + 0.2f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        temp.SetActive(true);
        temp.name = scoreName;
        return null;
    }

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

        for(int i=0; i<gmInst.bubblesEnd.Count; i++)
        {
            if(!gmInst.bubblesEnd[i].activeInHierarchy)
            {
                gmInst.bubblesEnd[i].name = this.gameObject.name;
                gmInst.bubblesEnd[i].SetActive(true);
                gmInst.bubblesEnd[i].transform.position = this.gameObject.transform.position;
                BubbleEnd tempBubbleEnd = gmInst.bubblesEnd[i].GetComponent<BubbleEnd>();
                tempBubbleEnd.BubbleEndInst(this.transform.localScale);
                return gmInst.bubblesEnd[i];
            }
        }

        GameObject temp = Instantiate(gmInst.bubbleEndPrefab);
        temp.name = this.gameObject.name;
        temp.SetActive(true);
        temp.transform.position = this.gameObject.transform.position;
        return temp;
    }


    public GameObject GetImpactEffect(bool panelPositive, Vector3 newPosition, string name)
    {   
        if(panelPositive)
        {
            for(int i=0; i<gmInst.effects.Count; i++)
            {
                if(!gmInst.effects[i].activeInHierarchy && gmInst.effects[i].name == "impact_GREEN")
                {

                    gmInst.effects[i].SetActive(true);
                    if(name == "LeftPlatform") { gmInst.effects[i].transform.position = new Vector2(newPosition.x - 0.25f, newPosition.y);  }
                    if(name == "RightPlatform") { gmInst.effects[i].transform.position = new Vector2(newPosition.x + 0.25f, newPosition.y);  }
                    if(name == "TopPlatform") { gmInst.effects[i].transform.position = new Vector2(newPosition.x, newPosition.y + 0.25f);  }
                    if(name == "BottomPlatform") { gmInst.effects[i].transform.position = new Vector2(newPosition.x, newPosition.y - 0.25f);  }
                    
                    Impact tempBubbleEnd = gmInst.effects[i].GetComponent<Impact>();
                    return gmInst.effects[i];

                }
            }
        }
        if(!panelPositive)
        {
            for(int i=0; i<gmInst.effects.Count; i++)
            {
                if(!gmInst.effects[i].activeInHierarchy && gmInst.effects[i].name == "impact_RED")
                {

                    gmInst.effects[i].SetActive(true); 
                    if(name == "LeftPlatform") { gmInst.effects[i].transform.position = new Vector2(newPosition.x - 0.25f, newPosition.y);  }
                    if(name == "RightPlatform") { gmInst.effects[i].transform.position = new Vector2(newPosition.x + 0.25f, newPosition.y);  }
                    if(name == "TopPlatform") { gmInst.effects[i].transform.position = new Vector2(newPosition.x, newPosition.y + 0.25f);  }
                    if(name == "BottomPlatform") { gmInst.effects[i].transform.position = new Vector2(newPosition.x, newPosition.y - 0.25f);  }
                    Impact tempBubbleEnd = gmInst.effects[i].GetComponent<Impact>();
                    return gmInst.effects[i];

                }
            }    

        }

        /// NOT FINISHED ****************************************
        GameObject temp = Instantiate(gmInst.impact_Prefab);
        temp.name = this.gameObject.name;
        temp.SetActive(true);
        temp.transform.position = this.gameObject.transform.position;
        return temp;
        ///// **************************************************
    }


    public void PlayerTeleportOnImpactPortal(EdgeBoard temp)
    {
        FindObjectOfType<AudioManager>().Play("portal"); 
        if(temp.gameObject.name == "TopPlatform")
        {  
            bottomPortalPlatform.child_capsuleCollider2D.isTrigger = true;
            player.transform.position = new Vector2(bottomPortalPlatform.transform.position.x, bottomPortalPlatform.transform.position.y);
            rb_player.velocity = rb_player.velocity * -1;
            playerInBottomPortal = true;

        }
        if(temp.gameObject.name == "BottomPlatform")
        {
            topPortalPlatform.child_capsuleCollider2D.isTrigger = true;
            player.transform.position = new Vector2(topPortalPlatform.transform.position.x, topPortalPlatform.transform.position.y);
            rb_player.velocity = rb_player.velocity * -1;
            playerInTopPortal = true;

        }
        if(temp.gameObject.name == "LeftPlatform")
        {
            rightPortalPlatform.child_capsuleCollider2D.isTrigger = true;
            player.transform.position = new Vector2(rightPortalPlatform.transform.position.x, rightPortalPlatform.transform.position.y);
            rb_player.velocity = rb_player.velocity * -1;
            playerInRightPortal = true;

        } 
        if(temp.gameObject.name == "RightPlatform")
        {
            leftPortalPlatform.child_capsuleCollider2D.isTrigger = true;
            player.transform.position = new Vector2(leftPortalPlatform.transform.position.x, leftPortalPlatform.transform.position.y);
            rb_player.velocity = rb_player.velocity * -1;
            playerInLeftPortal = true;
        }
                
    }

    //////// HERE PLAYER ENDS TELEPORT  ////////
    void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.gameObject.name == "EdgeSpriteCollider")
		{
			EdgeBoard tempEdgeBoard = collider.transform.parent.gameObject.GetComponent<EdgeBoard>();
            if(tempEdgeBoard.panelIsPortal)
            {
                if(bottomPortalPlatform != null){ bottomPortalPlatform.child_capsuleCollider2D.isTrigger = false; playerInBottomPortal = false; }
                if(topPortalPlatform != null){ topPortalPlatform.child_capsuleCollider2D.isTrigger = false; playerInTopPortal = false; }
                if(rightPortalPlatform !=  null) { rightPortalPlatform.child_capsuleCollider2D.isTrigger = false; playerInRightPortal = false; }
                if(leftPortalPlatform != null) { leftPortalPlatform.child_capsuleCollider2D.isTrigger = false; playerInLeftPortal = false; } 
            }
		}
        	
	}
    ///////////////////////////////////////////////




    //////////////////// ON COLLISION STAY AVOID PLAYER"S BUBBLE STAY CLOSE TO LOOT //////////////////////////
    void OnCollisionStay2D(Collision2D collision)
    {

            if(collision.gameObject.name == "Bomb_Loot" || collision.gameObject.name == "Copy_Loot" || 
				collision.gameObject.name == "Life_Loot" || 
               collision.gameObject.name == "Lightning_Loot" || collision.gameObject.name == "Shield_Loot" || 
			   collision.gameObject.name == "Frozend_Loot" || collision.gameObject.name == "Poison_Loot"
              )
			{	
                var newDirection = this.transform.position - collision.transform.position;   
                rb_player.AddForce(newDirection * 10);
            }
            if(collision.gameObject.name == "TopPlatform" || collision.gameObject.name == "BottomPlatform" || 
            collision.gameObject.name == "LeftPlatform" || collision.gameObject.name == "RightPlatform")
            {
                var newDirection = this.transform.position - collision.transform.position;   
                rb_player.AddForce(newDirection * (1f + playerBubbleSize));
            }
            if(collision.gameObject.transform.name == "EdgeSpriteCollider" && 
            (playerInBottomPortal || playerInTopPortal || playerInLeftPortal || playerInRightPortal)
            )
            {
                if(playerInBottomPortal)
                {
                    var newDirection = this.transform.position + collision.transform.position * 1.5f;   
                    rb_player.AddForce(newDirection * (1f + 3));
                    playerInBottomPortal = false;
                }
                if(playerInTopPortal)
                {
                    var newDirection = this.transform.position - collision.transform.position * 1.5f;   
                    rb_player.AddForce(newDirection * (1f + 3));
                    playerInTopPortal = false;
                }
                if(playerInLeftPortal)
                {
                    var newDirection = this.transform.position + collision.transform.position * 1.5f;   
                    rb_player.AddForce(newDirection * (1f + 3));
                    playerInLeftPortal = false;
                }
                if(playerInRightPortal)
                {
                    var newDirection = this.transform.position - collision.transform.position * 1.5f;   
                    rb_player.AddForce(newDirection * (1f + 3));
                    playerInRightPortal = false;
                }

                Debug.Log("HERE WE CAN MOVE BUBBLE OFF THE PANEL");
            }
            
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////



    void OnCollisionEnter2D(Collision2D collision2D)
    {   

        if(collision2D.gameObject.transform.name == "EdgeSpriteCollider" && playerIsMoving && gmInst.gameIsStarts)
        {   
            EdgeBoard temp = collision2D.gameObject.transform.parent.GetComponent<EdgeBoard>();
            

			if(temp.negativePositiveChance < 2 && !temp.panelIsPortal && !temp.panelIsZero && go_frozen_shield_child.activeSelf == false)
			{
				//Debug.Log("COLLISION WITH POSITIVE EDGE");
                GetImpactEffect(true, this.transform.position, temp.gameObject.name);
                FindObjectOfType<AudioManager>().Play("impact_UP");
				playerBubbleSize = playerBubbleSize + temp.edgeArgument;
                tempBubbleNumber = temp.edgeArgument;
                go_player_BubbleSize.SetActive(false); // here we turn off player bubble size number
                timeNotActive = 0;
                GetScore(temp.edgeArgument, Color.green, " ", " +", "PlayerChangeArgument", collision2D.gameObject.transform.position, true);
                ScaleControll(playerBubbleSize);
                
                
                
			}
			if(temp.negativePositiveChance >= 2  && !temp.panelIsPortal  && !temp.panelIsZero && go_frozen_shield_child.activeSelf == false)
			{
				//Debug.Log("COLLISION WITH NEGATIVE EDGE");
                GetImpactEffect(false, this.transform.position, temp.gameObject.name);
				playerBubbleSize = playerBubbleSize - temp.edgeArgument;
                tempBubbleNumber = temp.edgeArgument;
            
                if(playerBubbleSize <=0)
                {
                    GetScore(1, Color.red, " Bubble", " -", "playerGetsScore", this.transform.position, false);
                }    
                else
                {
                    FindObjectOfType<AudioManager>().Play("impact_DOWN");
                    go_player_BubbleSize.SetActive(false);
                    timeNotActive = 0;
                    GetScore(temp.edgeArgument, Color.red, " ", " -", "PlayerChangeArgument", this.transform.position, true);
                }

                ScaleControll(playerBubbleSize);
                
			}
            if(temp.panelIsPortal)
            {
                
               PlayerTeleportOnImpactPortal(temp); 
                  
            }
            if(temp.panelIsZero)
            {
                playerBubbleSize = 0;
                GetScore(1, Color.red, " Bubble", " -", "playerGetsScore", this.transform.position, false);    
                frozenDurability = 0;
            }
            if(go_frozen_shield_child.activeSelf == true)
            {
                FindObjectOfType<AudioManager>().Play("impact_W_ICE");
                GetImpactEffect(false, this.transform.position, temp.gameObject.name);
                frozenDurability -=1;
            }
            
        }
             

    }

    //This method controls rescale player bubble
    public void ScaleControll(int playerBubbleSize)
    {

            float scaleTempX = (float)(1.0f + (float)playerBubbleSize / 50.0f);
            float scaleTempY = (float)(1.0f + (float)playerBubbleSize / 50.0f);
            float scaleTempZ = (float)(1.0f + (float)playerBubbleSize / 50.0f);   

            go_player_parent.transform.localScale = new Vector3(
            scaleTempX, scaleTempY, scaleTempZ);

    }

    void DestroyBubble()
    {    
        playerLostBubble = true;
        gmInst.playerLostBubbleGM = playerLostBubble;
        gmInst.notSpawnNewBubbles = false;
        go_player_shield_child.SetActive(false);
        go_frozen_shield_child.SetActive(false);
        shieldDurability = 0;
        frozenDurability = 0;
        playerBubbleSize = 0;
        playerBubbleLife -=1;
        //Remove all loot items from table
        foreach (var item in gmInst.loot)
        {
            Loot tempLoot = item.GetComponent<Loot>();
            tempLoot.lifeTime = 0;
        }
        this.gameObject.SetActive(false);
             
    }

    void SetSizeOffOnCollision()
    {
        if(go_player_BubbleSize.activeSelf == false)
        {
            timeNotActive += 0.125f;
        }
        if(timeNotActive >= 5 && !playerLostBubble)
        {
            go_player_BubbleSize.SetActive(true);
            timeNotActive = 0;
        }
    }

    void SetTextSizeBasedOnScale()
    {
        ////// CONTROL OVER PLAYER"S BUBBLE TEXT //////
        if(playerBubbleSize < 10)
        {
            playerSizeOnText.fontSize = 300;
        }
        if(playerBubbleSize >= 10 && playerBubbleSize < 100)
        {
            playerSizeOnText.fontSize = 250;
        }
        if(playerBubbleSize >= 100)
        {
            playerSizeOnText.fontSize = 200;
        }
        ///////////////////////////////////////////////
    }
    
    //Fix Update here
    void FixedUpdate()
    {

        //Bool GAME IS STARTS -turns ON here
        if(rb_player.velocity.magnitude != 0)
        {
            gmInst.gameIsStarts = true;
            playerIsMoving = true;
            this.name = "player";
            go_player_parent.transform.Rotate(new Vector3(0,0, rb_player.velocity.magnitude));
        }



        //Applying force to player on touch event
        if (gmInst.touchEvent)
        {
            var newDirection = newPlayerPosition - gameObject.transform.position;   
            rb_player.AddForce(newDirection * 100f);
            gmInst.touchEvent = false; // turn off right away to avoid next input event
        }

        SetSizeOffOnCollision();

    }


	// Update is called once per frame
	void Update () {
      
        //playerBubbleSizeRecord = playerBubbleSize;
        playerSizeOnText.text = playerBubbleSize.ToString(); // add player size on Canvas and keeping track of it
        SetTextSizeBasedOnScale();

        if((gmInst.gameIsStarts && !playerIsMoving && !playerLostBubble) || (playerLostBubble && gmInst.helperRef.allBubbleMoving))
        {
            gmInst.helperRef.CheckEnemyBubbleStatus();
        }


        if(playerBubbleSize <= 0 && !playerLostBubble)
        {
            playerBubbleSize = 0;
            GetBubbleEnd(); // spawn object animation 
            Invoke("DestroyBubble", 0.005f);

            playerLostBubble = true;
            playerIsMoving = false;
        } 

        if(rb_player.velocity.magnitude < 0.25f && gmInst.gameIsStarts && playerIsMoving)
        {
             rb_player.velocity = Vector3.zero;
             playerIsMoving = false;
        }

        if (go_player_parent.transform.localScale.x < scalePlayerBubble & go_player_parent.transform.localScale.y < scalePlayerBubble & go_player_parent.transform.localScale.z < scalePlayerBubble)
        {
            go_player_parent.transform.localScale += new Vector3(randomScaleStart, randomScaleStart, randomScaleStart);
           
        }

        // Condition for starting saving new player size record
        if(playerBubbleSize > playerBubbleSizeRecord)
        {
            playerBubbleSizeRecord = playerBubbleSize;
        }
        if(playerScore > playerScoreRecord)
        {
            playerScoreRecord = playerScore;
        }
        

        /////////////////////////////////////////////////   
        ////////////// CONTROL OVER SHIELD //////////////
        //////////////   AND FROZEN SHIELD //////////////
        /////////////////////////////////////////////////
        //Shield Sprite Rotation
        if(go_player_shield_child.activeSelf)
        {
            go_player_shield_child.transform.Rotate(0,0,5);
        }
        //Control over shield ability
        if(shieldDurability <= 0)
        {
            go_player_shield_child.SetActive(false);
        }
        if(frozenDurability <= 0)
        {
            go_frozen_shield_child.SetActive(false);
        }
        /////////////////////////////////////////////////

    

	}
}
