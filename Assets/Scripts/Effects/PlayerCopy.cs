using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCopy : Effect {

	    public Rigidbody2D rb_playerCopy;
		public TextMesh playerCopySizeOnText;
		public GameObject go_playerCopy_BubbleSize;
        public GameObject go_playerCopy_Star;

        public SpriteRenderer playerCopySpriteRenderer;

        public AudioSource impact_UP;
        public AudioSource impact_DOWN;

        bool copyPlayerInTopPortal = false;
        bool copyPlayerInBottomPortal = false;
        bool copyPlayerInLeftPortal = false;
        bool copyPlayerInRightPortal = false;


		public int playerCopyBubbleSize; // here is playerCopy bubble number
		public int tempNumber;

		public float timeNotActive; // this we use for track status size player copy bubble after it turns invisable
		public float timer = 3f; // timer

	// Use this for initialization
	public override void Start () {
		
			base.Start();
			//name = "PlayerCopy_Effect" + ;
			rb_playerCopy = gameObject.GetComponent<Rigidbody2D>();
			playerCopySizeOnText = gameObject.GetComponentInChildren<TextMesh>();
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("player_controller");
			//ScaleControll(playerCopyBubbleSize);
			InitEffect();
            impact_UP.mute = true;
            impact_DOWN.mute = true;
            

	}
	
	

	//Shows positive number here
    public GameObject GetScore(int argument, Color color, string newEnding, string sign, string scoreName, Vector2 position, bool scoreStatus)
    {

        for(int i=0; i<gameManagerRef.score.Count; i++)
        {   
            if(gameManagerRef.score[i].activeInHierarchy && gameManagerRef.score[i].name == name + "_ChangeArgument")
            { gameManagerRef.score[i].SetActive(false); }
            if(!gameManagerRef.score[i].activeInHierarchy)
            {
                gameManagerRef.score[i].SetActive(true);
                gameManagerRef.score[i].name = scoreName;
                gameManagerRef.score[i].transform.position = position;
                Score scoreTemp = gameManagerRef.score[i].GetComponent<Score>();
                scoreTemp.timerActive = 10;
                scoreTemp.GettingNumber(argument, color, newEnding, sign, this.transform, scoreStatus);
                return gameManagerRef.score[i];
            }
        }    

        GameObject temp = Instantiate(gameManagerRef.score_Prefab);
        Score newScoreTemp = temp.GetComponent<Score>();
        newScoreTemp.timerActive = 10;

        gameManagerRef.score.Add(temp);
        temp.transform.position = new Vector3(this.gameObject.transform.position.x + 0.2f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        temp.SetActive(true);
        temp.name = scoreName;
        return temp;
    }

	/// GET BUBBLE END HERE ////
	public GameObject GetPlayerCopyBubbleEnd()
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

	///// APPLY SPEED TO RB //////
	public void ApplySpeedRB(Vector3 newDirection)
	{
		rb_playerCopy.AddForce(newDirection * 100f);
	}	

    public void CopyPlayerTeleportOnImpactPortal(EdgeBoard temp)
    {
        FindObjectOfType<AudioManager>().Play("portal"); 
        Player tempPlayer = FindObjectOfType<Player>();
        if(temp.gameObject.name == "TopPlatform")
        {  
            tempPlayer.bottomPortalPlatform.child_capsuleCollider2D.isTrigger = true;
            this.transform.position = new Vector2(tempPlayer.bottomPortalPlatform.transform.position.x, tempPlayer.bottomPortalPlatform.transform.position.y);
            rb_playerCopy.velocity = rb_playerCopy.velocity * -1;
            copyPlayerInBottomPortal = true;

        }
        if(temp.gameObject.name == "BottomPlatform")
        {
            tempPlayer.topPortalPlatform.child_capsuleCollider2D.isTrigger = true;
            this.transform.position = new Vector2(tempPlayer.topPortalPlatform.transform.position.x, tempPlayer.topPortalPlatform.transform.position.y);
            rb_playerCopy.velocity = rb_playerCopy.velocity * -1;
            copyPlayerInTopPortal = true;

        }
        if(temp.gameObject.name == "LeftPlatform")
        {
            tempPlayer.rightPortalPlatform.child_capsuleCollider2D.isTrigger = true;
            this.transform.position = new Vector2(tempPlayer.rightPortalPlatform.transform.position.x, tempPlayer.rightPortalPlatform.transform.position.y);
            rb_playerCopy.velocity = rb_playerCopy.velocity * -1;
            copyPlayerInRightPortal = true;

        } 
        if(temp.gameObject.name == "RightPlatform")
        {
            tempPlayer.leftPortalPlatform.child_capsuleCollider2D.isTrigger = true;
            this.transform.position = new Vector2(tempPlayer.leftPortalPlatform.transform.position.x, tempPlayer.leftPortalPlatform.transform.position.y);
            rb_playerCopy.velocity = rb_playerCopy.velocity * -1;
            copyPlayerInLeftPortal = true;
        }
                
    }

    //////// HERE PLAYER ENDS TELEPORT  ////////
    void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.gameObject.name == "EdgeSpriteCollider")
		{
            Player tempPlayer = FindObjectOfType<Player>();
			EdgeBoard tempEdgeBoard = collider.transform.parent.gameObject.GetComponent<EdgeBoard>();
            if(tempEdgeBoard.panelIsPortal)
            {
                if(tempPlayer.bottomPortalPlatform != null){ tempPlayer.bottomPortalPlatform.child_capsuleCollider2D.isTrigger = false; copyPlayerInBottomPortal = false; }
                if(tempPlayer.topPortalPlatform != null){ tempPlayer.topPortalPlatform.child_capsuleCollider2D.isTrigger = false; copyPlayerInTopPortal = false; }
                if(tempPlayer.rightPortalPlatform !=  null) { tempPlayer.rightPortalPlatform.child_capsuleCollider2D.isTrigger = false; copyPlayerInRightPortal = false; }
                if(tempPlayer.leftPortalPlatform != null) { tempPlayer.leftPortalPlatform.child_capsuleCollider2D.isTrigger = false; copyPlayerInLeftPortal = false; } 
            }
		}
        	
	}
    ///////////////////////////////////////////////

	void OnCollisionEnter2D(Collision2D collision2D)
    {   

        if(collision2D.gameObject.transform.name == "PlayerCopy_Effect_0" || collision2D.gameObject.transform.name == "PlayerCopy_Effect_1" || collision2D.gameObject.transform.name == "PlayerCopy_Effect_2")
        {           
                PlayerCopy tempPlayerCopy = collision2D.gameObject.GetComponent<PlayerCopy>();

                int storeColliderCopyPlayerBubbleSize = tempPlayerCopy.playerCopyBubbleSize;

                this.playerCopyBubbleSize += storeColliderCopyPlayerBubbleSize;

                impact_UP.mute = false;
                impact_UP.Play();
                this. 
                go_playerCopy_BubbleSize.SetActive(false);
                timeNotActive = 0;
                GetScore(storeColliderCopyPlayerBubbleSize, Color.green, " ", " +", name + "_ChangeArgument", this.transform.position, true);
                ScaleControll(playerCopyBubbleSize);
        }

        if(collision2D.gameObject.transform.name == "EdgeSpriteCollider")
        {   
            EdgeBoard temp = collision2D.gameObject.transform.parent.GetComponent<EdgeBoard>();

			if(temp.negativePositiveChance < 2 && !temp.panelIsPortal && !temp.panelIsZero)
			{
				//Debug.Log("COLLISION WITH POSITIVE EDGE");
				
                impact_UP.mute = false;
                impact_UP.Play();
                tempNumber = temp.edgeArgument;
                playerCopyBubbleSize += tempNumber;
                go_playerCopy_BubbleSize.SetActive(false);
                timeNotActive = 0;
                GetScore(tempNumber, Color.green, " ", " +", name + "_ChangeArgument", this.transform.position, true);
                ScaleControll(playerCopyBubbleSize);
                
			}
			if(temp.negativePositiveChance >= 2 && !temp.panelIsPortal && !temp.panelIsZero)
			{
				//Debug.Log("COLLISION WITH NEGATIVE EDGE")
                
                tempNumber = temp.edgeArgument;
                playerCopyBubbleSize -= tempNumber;

                if(playerCopyBubbleSize <=0)
                {
                    timer = 0;
                    this.gameObject.SetActive(false);
                    GetPlayerCopyBubbleEnd();
                    Invoke("DestroyPlayerCopy", 0.05f);
                }    
                else
                {
                    impact_DOWN.mute = false;
                    impact_DOWN.Play();
                    this.go_playerCopy_BubbleSize.SetActive(false);
                    this.timeNotActive = 0;
                    GetScore(tempNumber, Color.red, " ", " -", name + "_ChangeArgument", this.transform.position, true);
                }

                ScaleControll(playerCopyBubbleSize);
			}
            if(temp.panelIsPortal && !temp.panelIsZero)
            {
                
               CopyPlayerTeleportOnImpactPortal(temp); 
                  
            }
            if(temp.panelIsZero && !temp.panelIsPortal)
            {
                    timer = 0;
                    this.gameObject.SetActive(false);
                    GetPlayerCopyBubbleEnd();
                    Invoke("DestroyPlayerCopy", 0.05f);
            }
            
        }
    }


	public void ScaleControll(int playerCopyBubbleSize)
    {

            float scaleTempX = (float)(1.0f + (float)playerCopyBubbleSize / 50.0f);
            float scaleTempY = (float)(1.0f + (float)playerCopyBubbleSize / 50.0f);
            float scaleTempZ = (float)(1.0f + (float)playerCopyBubbleSize / 50.0f);   

            this.gameObject.transform.localScale = new Vector3(
            scaleTempX, scaleTempY, scaleTempZ);

    }

    void SetTextSizeBasedOnScale()
    {
        ////// CONTROL OVER PLAYER"S BUBBLE TEXT //////
        if(playerCopyBubbleSize < 10)
        {
            playerCopySizeOnText.fontSize = 300;
        }
        if(playerCopyBubbleSize >= 10 && playerCopyBubbleSize < 100)
        {
            playerCopySizeOnText.fontSize = 250;
        }
        if(playerCopyBubbleSize >= 100)
        {
            playerCopySizeOnText.fontSize = 200;
        }
        ///////////////////////////////////////////////
    }

    public void DestroyPlayerCopy()
    {
        Destroy(this.gameObject);
    }

	void FixedUpdate()
	{

		if(rb_playerCopy.velocity.magnitude > 0.25f)
        {
            this.gameObject.transform.Rotate(new Vector3(0,0, rb_playerCopy.velocity.magnitude));
        }	

		if(lifeTime > 0)
		{
			timer -= Time.deltaTime;
		}
		if(timer <=0 && playerCopySpriteRenderer.enabled == true || gameManagerRef.deleteGame)
		{
			timer = 0;
			playerCopySpriteRenderer.enabled = false;
            this.go_playerCopy_BubbleSize.SetActive(false);
            this.go_playerCopy_Star.SetActive(false);
            GetPlayerCopyBubbleEnd();
            Invoke("DestroyPlayerCopy", 0.5f);
		}


		if(this.go_playerCopy_BubbleSize.activeSelf == false)
        {
            this.timeNotActive += 0.125f;
        
            if(this.timeNotActive >= 4.5f)
            {
                this.go_playerCopy_BubbleSize.SetActive(true);
                this.timeNotActive = 0;
            }
        }
	}

	// Update is called once per frame
	void Update () {

        SetTextSizeBasedOnScale();

        if(playerCopyBubbleSize <= 0 && playerCopySpriteRenderer.enabled == true)
        {
            timer = 0;
        }

		playerCopySizeOnText.text = playerCopyBubbleSize.ToString();
	}
}
