using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Loot {


	public GameObject energyPrefab;

	public List<GameObject> energy = new List<GameObject>();

	// Use this for initialization
	public override void Start () {
		
		base.Start();
		InitLoot();


		///// HERE WE ADD 5 ENERGY BALLS ///// 
		for (int i = 0; i <5; i++ )
        {
            GameObject tempEnergy = Instantiate(energyPrefab);
			tempEnergy.transform.parent = gameManagerRef.transform;
			tempEnergy.name = "Energy_Effect";
            energy.Add(tempEnergy);
			
        }
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
			lifeTime = 0;
			
			foreach (var item in energy)
			{
				Energy tempEnergy = item.GetComponent<Energy>();
				tempEnergy.gameObject.SetActive(true);
				
				tempEnergy.SetRandomWorldPosition();
			}
			
			gameManagerRef.loot.Remove(this.gameObject);
			Destroy(this.gameObject);
		}
	}

	void DestroyObject()
	{
		for (int i = 0; i < energy.Count; i++)
		{
			Energy tempEnergy = energy[i].GetComponent<Energy>();
			tempEnergy.DestroyObject();
		}
		Destroy(this.gameObject);
	}

	// Update is called once per frame
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
