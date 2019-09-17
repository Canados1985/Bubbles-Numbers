using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour{
	
	public GameManager gameManagerRef;
	public GameObject lootContainer;

	public Rigidbody2D rb_loot; 

	public CircleCollider2D circleCollider;
	public int lifeTime;
	public Animator spriteAnimator;

	// Use this for initialization
	public virtual void Start () {
		
		gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>(); // here we assign game manager refference	
		lootContainer = GameObject.Find("LootContainer");
		rb_loot = gameObject.GetComponent<Rigidbody2D>();
		circleCollider = gameObject.GetComponent<CircleCollider2D>();
		
		gameObject.transform.parent = lootContainer.transform;		
		gameObject.transform.position = lootContainer.transform.position;

		//Debug.Log("Loot is ready for duty!...");

		spriteAnimator = this.gameObject.GetComponent<Animator>();

	}

	public void InitLoot()
	{
		if(this.gameObject.name == "Bomb_Loot")
		{
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("bomb_controller");
		}
		if(this.gameObject.name == "Life_Loot")
		{
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("heart_controller");
		}
		if(this.gameObject.name == "Shield_Loot")
		{
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("shield_controller");
		}
		if(this.gameObject.name == "Copy_Loot")
		{
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("copy_controller");
		}
		if(this.gameObject.name == "Lightning_Loot")
		{
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Lightning_Loot_controller");
		}
		if(this.gameObject.name == "Frozen_Loot")
		{
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("frozen_controller");
		}
		if(this.gameObject.name == "Poison_Loot")
		{
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("poison_controller");
		}


		TakeRandomPosition();

		gameObject.SetActive(true); // make loot object not active
		int randomTime = Random.Range(2,4); 
		lifeTime = randomTime;
		gameManagerRef.loot.Add(this.gameObject); // Here we add loot class object into GameManager.loot list;
		

	}
	

	void TakeRandomPosition()
    {


        float playerPosX = gameManagerRef.playerRef.transform.transform.position.x;
        float playerPosY = gameManagerRef.playerRef.transform.transform.position.y;

		float position_x = Random.Range(gameManagerRef.go_LeftPlatforms_Container.transform.position.x + 1.0f, gameManagerRef.go_RightPlatforms_Container.transform.position.x - 1.0f);
        float position_y = Random.Range(gameManagerRef.go_BottonPlatforms_Container.transform.position.y + 1.0f, gameManagerRef.go_TopPlatforms_Container.transform.position.y - 1.0f);

		this.transform.position = new Vector3(position_x, position_y);
        
    } 

	



	// Update is called once per frame
	void Update () {
		


	}
}
