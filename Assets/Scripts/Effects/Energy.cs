using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : Effect {

	float timer = 0f; // timer
	float speed = 4.5f;
	AudioSource sound;
	Vector2 newTargetPosition;
	public override void Start () {
		
			name = "Energy_Effect";
			base.Start();
			sound = gameObject.GetComponent<AudioSource>();
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
			collidingBubble.OnCollisionWithEnergy(collidingBubble);
			DestroyObject();
			
		}
	}

	public void SetRandomWorldPosition()
	{
		float position_x = Random.Range(gameManagerRef.go_LeftPlatforms_Container.transform.position.x + 0.5f, gameManagerRef.go_RightPlatforms_Container.transform.position.x - 0.5f);
        float position_y = Random.Range(gameManagerRef.go_BottonPlatforms_Container.transform.position.y + 0.5f, gameManagerRef.go_TopPlatforms_Container.transform.position.y - 0.5f);
		
        this.gameObject.transform.position = new Vector3(position_x, position_y);
		newTargetPosition = GetTargetPosition();

		sound.mute = false;
		float randomPitch = Random.Range(-3,3);
		sound.pitch = randomPitch;
		sound.Play();	
	}


	Vector2 GetTargetPosition()
	{

		float position_x = Random.Range(gameManagerRef.go_LeftPlatforms_Container.transform.position.x + 0.5f, gameManagerRef.go_RightPlatforms_Container.transform.position.x - 0.5f);
        float position_y = Random.Range(gameManagerRef.go_BottonPlatforms_Container.transform.position.y + 0.5f, gameManagerRef.go_TopPlatforms_Container.transform.position.y - 0.5f);
		
		Vector2 targetPosition = new Vector2(position_x, position_y);
		return targetPosition;
	}

	public void DestroyObject()
	{
		Destroy(this.gameObject);
	} 
	
	// Update is called once per frame
	void Update () {
		
		if(this.gameObject.activeSelf == true)
		{
			float step =  speed * Time.deltaTime; // calculate distance to move
			//this.gameObject.transform.Rotate(new Vector3(0,0, 1.5f));
        	transform.position = Vector2.MoveTowards(this.transform.position, newTargetPosition, step);
		}
		if((Vector2)this.transform.position == newTargetPosition || gameManagerRef.deleteGame)
		{
			
			DestroyObject();
		}
		if(this.gameObject.activeSelf == false)
		{
			gameObject.transform.position = effectsContainer.transform.position;
			sound.Stop();
			sound.mute = true;
			
		}

	}
}
