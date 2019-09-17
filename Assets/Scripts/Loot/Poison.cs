using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : Loot {

public GameObject poisonBubblePrefab;
	
	public List<GameObject> poisonBubble = new List<GameObject>();


	// Use this for initialization
	public override void Start () {
		
		base.Start();

		InitLoot();
						
            GameObject tempBubble = Instantiate(poisonBubblePrefab);
			tempBubble.transform.parent = gameManagerRef.transform;
			tempBubble.name = "PoisonBubble_Effect";
            poisonBubble.Add(tempBubble);
	}


	//////////////////// ON COLLISION STAY AVOID PLAYER"S BUBBLE STAY CLOSE TO LOOT //////////////////////////
    void OnCollisionStay2D(Collision2D collision)
    {

            if(collision.gameObject.name == "player_idle" || collision.gameObject.name == "player")
            {
				Player tempPlayer = collision.gameObject.GetComponent<Player>();

                var newDirection = this.transform.position - collision.transform.position;   
                rb_loot.AddForce(newDirection * (1f + tempPlayer.playerBubbleSize));
            }
            if(collision.gameObject.name == "Bomb_Loot" || collision.gameObject.name == "Copy_Loot" || 
				collision.gameObject.name == "Life_Loot" || 
               collision.gameObject.name == "Lightning_Loot" || collision.gameObject.name == "Shield_Loot" || 
			   collision.gameObject.name == "Frozend_Loot" || collision.gameObject.name == "Poison_Loot"
              )
			{	

                var newDirection = this.transform.position - collision.transform.position;   
                rb_loot.AddForce(newDirection * (1f + 10));
            }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////


	void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.name == "player" && this.gameObject.activeSelf || 
		collision.gameObject.name == "player_idle" && this.gameObject.activeSelf
		)
		{
			Player tempPlayer = collision.GetComponent<Player>();
			
			
			//Here we call function 
			foreach (var item in poisonBubble)
			{
				Poison_Bubble tempPoisonBubble = item.GetComponent<Poison_Bubble>();
				tempPoisonBubble.gameObject.SetActive(true);
				tempPoisonBubble.randomRot = Random.Range(1, 5);
				tempPoisonBubble.transform.position = tempPlayer.transform.position;
				tempPoisonBubble.SetWorldPosition(collision.transform.position.x, collision.transform.position.y);
				//tempPlayerCopy.ScaleControll(tempPlayerCopy.playerCopyBubbleSize);
				//tempPlayerCopy.lifeTime = 10;
				//tempPlayerCopy.timer = 3;
				//tempPoisonBubble.ApplySpeedRB(gameManagerRef.helperRef.TakeRandomPosition());
			}

			this.gameObject.SetActive(false);
			lifeTime = 0;
			gameObject.transform.position = lootContainer.transform.position;
			gameManagerRef.loot.Remove(this.gameObject);
			Destroy(this.gameObject);
		}
	}


	void DestroyObject()
	{
		for (int i = 0; i < poisonBubble.Count; i++)
		{
			Poison_Bubble tempPoisonBubble = poisonBubble[i].GetComponent<Poison_Bubble>();
			tempPoisonBubble.DestroyObject();
		}
		Destroy(this.gameObject);
	}
	
	void Update () {
		
		if(lifeTime == 0)
		{
			gameObject.SetActive(false);
			gameObject.transform.position = lootContainer.transform.position;
			gameManagerRef.loot.Remove(this.gameObject);
			DestroyObject();
		}
		if(gameManagerRef.touchEvent)
		{
			circleCollider.isTrigger = true;
		}
		if(gameManagerRef.playerTurn)
		{
			circleCollider.isTrigger = false;
		}
		if(rb_loot.velocity.magnitude < 0.15f)
		{
			rb_loot.velocity = Vector3.zero;
		}
	}
}
