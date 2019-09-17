using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Loot {

	//public Explosion explosionEffectRef;

	// Use this for initialization
	public override void Start () {

			base.Start();
			//explosionEffectRef = GameObject.Find("EffectsContainer/Explosion_Effect").GetComponent<Explosion>();
			InitLoot();
			
	}

	public GameObject GetExplosion(Vector2 position)
    {

        for(int i=0; i<gameManagerRef.effects.Count; i++)
        {
            if(!gameManagerRef.effects[i].activeInHierarchy && gameManagerRef.effects[i].name == "Explosion_Effect")
            {
               
                gameManagerRef.effects[i].SetActive(true);
                gameManagerRef.effects[i].transform.position = position;
                return gameManagerRef.bubblesEnd[i];
            }
        }

        GameObject temp = Instantiate(gameManagerRef.explosionPrefab);
        temp.SetActive(true);
        temp.transform.position = position;
        return temp;
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
			lifeTime = 0;
			FindObjectOfType<AudioManager>().Play("explosion");
			GetExplosion(this.transform.position);
			gameManagerRef.helperRef.Invoke("PlayerPickUpBomb", 0.25f);
			gameManagerRef.loot.Remove(this.gameObject);
			Destroy(this.gameObject);

		}
	}
	


	// Update is called once per frame
	void Update () {

		if(lifeTime == 0)
		{
			gameObject.SetActive(false);
			gameObject.transform.position = lootContainer.transform.position;
			gameManagerRef.loot.Remove(this.gameObject);
			Destroy(this.gameObject);
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
