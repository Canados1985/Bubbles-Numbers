using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_Bubble : Effect {

    	public Rigidbody2D rb_poisonBubble;

		//public Explosion_Poison explosion_PoisonRef;
		Vector2 newTargetPosition;

		float speed = 4.5f;
		public float randomRot;
		public int tempNumber;

		public float timeNotActive; // this we use for track status size player copy bubble after it turns invisable
		public float timer = 3f; // timer

	// Use this for initialization
	public override void Start () {
		
			base.Start();
			name = "PosionBubble_Effect";
			rb_poisonBubble = gameObject.GetComponent<Rigidbody2D>();
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("poison_bubble_controller");
			//explosion_PoisonRef = GameObject.Find("EffectsContainer/Explosion_Poison_Effect").GetComponent<Explosion_Poison>();
			//ScaleControll(playerCopyBubbleSize);
			InitEffect();
			
	}
	
	public void SetWorldPosition(float posX, float posY)
	{
		FindObjectOfType<AudioManager>().Play("nuclear");
        this.gameObject.transform.position = new Vector3(posX, posY);
		GetTargetPosition();
	}

	
	Vector2 GetTargetPosition()
	{

		float position_x = Random.Range(gameManagerRef.go_LeftPlatforms_Container.transform.position.x + 0.5f, gameManagerRef.go_RightPlatforms_Container.transform.position.x - 0.5f);
        float position_y = Random.Range(gameManagerRef.go_BottonPlatforms_Container.transform.position.y + 0.5f, gameManagerRef.go_TopPlatforms_Container.transform.position.y - 0.5f);
		
		Vector2 targetPosition = new Vector2(position_x, position_y);
		newTargetPosition = targetPosition;
		return targetPosition;
	}

	
	public GameObject GetPoisonExplosion(Vector2 position)
    {

        for(int i=0; i<gameManagerRef.effects.Count; i++)
        {
            if(!gameManagerRef.effects[i].activeInHierarchy && gameManagerRef.effects[i].name == "Explosion_Poison_Effect")
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

	void OnCollisionEnter2D(Collision2D collision)
    {   

        if(collision.gameObject.transform.name == "EdgeSpriteCollider")
        {
			GetPoisonExplosion(this.transform.position);			
			FindObjectOfType<AudioManager>().Stop("nuclear");
			FindObjectOfType<AudioManager>().Play("explosion");
			DestroyObject();
		}
		if((collision.gameObject.name == "blue" || collision.gameObject.name == "green" || collision.gameObject.name == "pink" || collision.gameObject.name == "red" ||
           collision.gameObject.name == "purple" || collision.gameObject.name == "white" || collision.gameObject.name == "yellow" && this.gameObject.activeSelf)
		   
		   ||

		   (collision.gameObject.name == "blue_idle" || collision.gameObject.name == "green_idle" || collision.gameObject.name == "pink_idle" || collision.gameObject.name == "red_idle" ||
           collision.gameObject.name == "purple_idle" || collision.gameObject.name == "white_idle" || collision.gameObject.name == "yellow_idle" && this.gameObject.activeSelf)) 
		{

			GetPoisonExplosion(this.transform.position);
			FindObjectOfType<AudioManager>().Stop("nuclear");
			FindObjectOfType<AudioManager>().Play("explosion");
			DestroyObject();
		}

    }

	public void DestroyObject()
	{
		Destroy(this.gameObject);	
	}

	void FixedUpdate()
	{
		if(this.gameObject.activeSelf)
		{
			this.gameObject.transform.Rotate(new Vector3(0,0, randomRot));
		}
		

		if(lifeTime > 0)
		{
			timer -= Time.deltaTime;
		}
		if(timer <=0)
		{
			timer = 0;
			this.gameObject.SetActive(false);
		}
		if((Vector2)this.transform.position == newTargetPosition || gameManagerRef.deleteGame)
		{
			GetPoisonExplosion(this.transform.position);
			FindObjectOfType<AudioManager>().Stop("nuclear");
			FindObjectOfType<AudioManager>().Play("explosion");
			DestroyObject();
		}

			
	}


	

	// Update is called once per frame
	void Update () {
		if(this.gameObject.activeSelf == true)
		{
			float step =  speed * Time.deltaTime; // calculate distance to move
			//this.gameObject.transform.Rotate(new Vector3(0,0, 1.5f));
        	transform.position = Vector2.MoveTowards(this.transform.position, newTargetPosition, step);
		}
		
	}
}
