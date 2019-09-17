using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Effect {

	float timer = 0f; // timer

	public override void Start () {
		
			name = "Explosion_Effect";
			base.Start();
		
			InitEffect();
			
	}
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if((collision.gameObject.name == "blue" || collision.gameObject.name == "green" || collision.gameObject.name == "pink" || collision.gameObject.name == "red" ||
           collision.gameObject.name == "purple" || collision.gameObject.name == "white" || collision.gameObject.name == "yellow" && this.gameObject.activeSelf)
		   
		   ||

		   (collision.gameObject.name == "blue_idle" || collision.gameObject.name == "green_idle" || collision.gameObject.name == "pink_idle" || collision.gameObject.name == "red_idle" ||
           collision.gameObject.name == "purple_idle" || collision.gameObject.name == "white_idle" || collision.gameObject.name == "yellow_idle" && this.gameObject.activeSelf)) 
		{
			Bubble collidingBubble = collision.GetComponent<Bubble>();
			//HERE WE NEED RUN FUNCTION FROM BUBBLE BUBBLE CLASS TO DESTROY ENEMY BUBBLE//
			collidingBubble.OnCollisionWithExplosion(collidingBubble);
			
		}
	}


	void FixedUpdate()
	{
		if(this.gameObject.activeSelf)
		{
			timer += Time.deltaTime;
			name = "Explosion_Active";
		}
		if(timer >= 0.65f)
		{
			timer = 0;
			name = "Explosion_Effect";
			this.gameObject.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update () {
		
		if(this.gameObject.activeSelf == false)
		{
			gameObject.transform.position = effectsContainer.transform.position;
		}
	}
}
