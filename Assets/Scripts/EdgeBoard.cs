using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeBoard : MonoBehaviour {


	GameManager gameManagerRef;
	public Player playerRef;
	SpriteRenderer spriteRenderer;

  	public GameObject go_edgeBoard_parent;
  	public GameObject go_edgeBoard_child;
	public GameObject go_edgeBoard_childCollider;  
	public GameObject go_portalSprite;

	public GameObject go_zeroSkullSprite;
	
	public CapsuleCollider2D child_capsuleCollider2D;
	public Animator animator_Child;

	public TextMesh edgeArgumentOnText;

	public bool tableReady = false;

	public bool panelIsPortal = false;

	public bool panelIsZero = false;

   //*********************** EDGE BOARD SPECIFICATIONS ***********************//

    public float timerAwaking;

	public float timerDestroy = 50;
	public float timerAwakingInMemory;
    int randomSprite;
    public int edgeArgument;
	public int negativePositiveChance;

	public int numberInList = 0;


    //*********************************************************************//

	void InitEdgeBoard()
	{
		go_edgeBoard_parent = gameObject; // assign parent GameObect
		go_edgeBoard_childCollider.SetActive(false);
		gameManagerRef = GameManager.gameManager;
		playerRef = GameObject.Find("player_idle").GetComponent<Player>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        edgeArgumentOnText = gameObject.GetComponentInChildren<TextMesh>();
		

		if(panelIsPortal)
		{
			animator_Child.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("panel_portal_controller");
			panelIsPortal = true;
			go_edgeBoard_child.SetActive(false);
			//child_capsuleCollider2D.isTrigger = true;
			
			if(this.gameObject.name == "TopPlatform")
			{
				playerRef.topPortalPlatform = this;
			}
			if(this.gameObject.name == "BottomPlatform")
			{
				playerRef.bottomPortalPlatform = this;
			} 
			if(this.gameObject.name == "RightPlatform")
			{
				playerRef.rightPortalPlatform = this;
			} 
			if(this.gameObject.name == "LeftPlatform")
			{
				playerRef.leftPortalPlatform = this;
			}   
			
		}
		if(panelIsZero)
		{	
			Debug.Log("THIS PANEL IS ZERO");
			animator_Child.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("panel_zero_controller");
			panelIsZero = true;
			go_edgeBoard_child.SetActive(false);
		}
		else
		{
			//child_capsuleCollider2D.isTrigger = false;	
			negativePositiveChance = Random.Range(0, 7);	
			go_edgeBoard_child.SetActive(true);
			if(negativePositiveChance < 2)
			{ 
				edgeArgumentOnText.color = Color.green;
				animator_Child.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("panel_controller");
				panelIsPortal = false;
			}
			else
			{
				edgeArgumentOnText.color = Color.red;
				animator_Child.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("panel_negative_controller");
				panelIsPortal = false;
				go_portalSprite.SetActive(false);
			}	

			edgeArgument = Random.Range(1, 11);
			edgeArgumentOnText.text = "";
			go_edgeBoard_child.SetActive(false);
			
		}



	}


	// Use this for initialization
	void Start () {
		
		InitEdgeBoard();
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.name == "player")
		{
			Player tempPlayer = collider.gameObject.GetComponent<Player>();
			tempPlayer.PlayerTeleportOnImpactPortal(this);	
			Debug.Log("FROM EDGE");	
		}	
	}


	public void TakeNewArgument()
	{	
		go_edgeBoard_child.SetActive(false);
		timerAwakingInMemory = 20;
		timerAwaking = timerAwakingInMemory;
		
		// THIS PANEL IS PORTAL
		if(panelIsPortal && !panelIsZero)
		{
			//Debug.Log("THIS PANEL IS PORTAL");
			animator_Child.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("panel_portal_controller");
			panelIsPortal = true;
			go_portalSprite.SetActive(true);
			go_zeroSkullSprite.SetActive(false);
			go_edgeBoard_child.SetActive(false); 

			//child_capsuleCollider2D.isTrigger = true;
			if(this.gameObject.name == "TopPlatform")
			{
				playerRef.topPortalPlatform = this;
			}
			if(this.gameObject.name == "BottomPlatform")
			{
				playerRef.bottomPortalPlatform = this;
			} 
			if(this.gameObject.name == "RightPlatform")
			{
				playerRef.rightPortalPlatform = this;
			} 
			if(this.gameObject.name == "LeftPlatform")
			{
				playerRef.leftPortalPlatform = this;
			} 

		}

		//THIS PANEL IS ZERO
		if(panelIsZero && !panelIsPortal)
		{	
			//Debug.Log("THIS PANEL IS ZERO");
			animator_Child.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("panel_zero_controller");
			panelIsZero = true;
			go_zeroSkullSprite.SetActive(true);
			go_portalSprite.SetActive(false);
			go_edgeBoard_child.SetActive(false);

			if(this.gameObject.name == "TopPlatform")
			{
				go_zeroSkullSprite.transform.rotation = new Quaternion(0,0,0,0);	
			}
			if(this.gameObject.name == "BottomPlatform")
			{
				go_zeroSkullSprite.transform.rotation = new Quaternion(0,0,0,180);
			} 
			if(this.gameObject.name == "RightPlatform")
			{
				go_zeroSkullSprite.transform.rotation = new Quaternion(0,0,0,180);
			} 
			if(this.gameObject.name == "LeftPlatform")
			{
				go_zeroSkullSprite.transform.rotation = new Quaternion(0,0,0,180);
			}  


		}

		//THIS IS PANEL NOT PORTAL OR ZERO
		if(!panelIsZero && !panelIsPortal)
		{
			negativePositiveChance = Random.Range(0, 7);	
			//child_capsuleCollider2D.isTrigger = false;	
			if(negativePositiveChance < 2)
			{ 
				edgeArgumentOnText.color = Color.green;
				animator_Child.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("panel_controller");
				panelIsPortal = false;
			}
			else
			{
				edgeArgumentOnText.color = Color.red;
				animator_Child.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("panel_negative_controller");
				panelIsPortal = false;
			}

			edgeArgument = Random.Range(1, 11);
			edgeArgumentOnText.text = "";
			go_edgeBoard_child.SetActive(true);
			go_zeroSkullSprite.SetActive(false);
			go_portalSprite.SetActive(false);	
		}


	}


	void DestroyEdgeBoard()
	{
		Destroy(this.gameObject);
	}

	void FixedUpdate()
	{

		if(timerAwaking > 0)
		{
			timerAwaking -= Time.deltaTime * 35;
		}

		if(timerAwaking <= 0)
		{
			timerAwaking = 0;
			edgeArgumentOnText.text = edgeArgument.ToString();
			go_edgeBoard_childCollider.SetActive(true);
			animator_Child = gameObject.GetComponentInChildren<Animator>();

		}
		if(timerAwaking <= 0 && !panelIsPortal && !panelIsZero)
		{
			go_edgeBoard_child.SetActive(true);
				
		}
		if(timerAwaking <= 0 && panelIsPortal)
		{
			go_portalSprite.SetActive(true);
		}




		if(playerRef.playerBubbleLife <= 0)
		{	

			if(timerDestroy > 0)
			{
				timerDestroy -= Time.deltaTime * 35;
			}
			if(timerDestroy <=0)
			{
				go_edgeBoard_childCollider.SetActive(false);
				edgeArgumentOnText.text = " ";
				Invoke("DestroyEdgeBoard", 0.5f);
				
			}
			if(timerDestroy <= 0 && !panelIsPortal)
			{
				go_edgeBoard_child.SetActive(true);
					
			}
			if(timerDestroy <= 0 && panelIsPortal)
			{
				go_portalSprite.SetActive(true);
			} 

			
			/* if(edgeArgument > 0)
			{
				edgeArgument -= 1 + (int)Time.deltaTime / 2;
			}
			if(edgeArgument <= 0)
			{
				Invoke("DestroyEdgeBoard", 1);	
			} */
			

		}	
	}

	
}
