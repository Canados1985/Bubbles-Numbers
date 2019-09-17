using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {

	public GameManager gameManagerRef;
	public GameObject effectsContainer;

	public int lifeTime = 0;
	public Animator spriteAnimator;
	// Use this for initialization
	public virtual void Start () {

		gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>(); // here we assign game manager refference	
		effectsContainer = GameObject.Find("EffectsContainer");
				
		gameObject.transform.parent = effectsContainer.transform;		
		gameObject.transform.position = effectsContainer.transform.position;

		//Debug.Log("Loot is ready for duty!...");

		spriteAnimator = this.gameObject.GetComponent<Animator>();	
		
	}
	

	public void InitEffect()
	{
		if(this.gameObject.name == "Explosion_Effect")
		{
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("explosion_controller");
			//THIS EFFECT HAS BEEN ADDED INTO EFFECTS LIST IN GAMEMANAGER ----->
		}
		if(this.gameObject.name == "Explosion_Poison_Effect")
		{
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("poison_blast_controller");
			//THIS EFFECT HAS BEEN ADDED INTO EFFECTS LIST IN GAMEMANAGER ----->
		}
		if(this.gameObject.name == "Energy_Effect")
		{
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Lightning_controller");
			gameManagerRef.effects.Add(this.gameObject); // Here we add loot class object into GameManager.loot list;
		}
		if(this.gameObject.name == "impact_RED")
		{
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("impact_red_controller");
			gameManagerRef.effects.Add(this.gameObject); // Here we add loot class object into GameManager.loot list;
		}
		if(this.gameObject.name == "impact_GREEN")
		{
			spriteAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("impact_green_controller");
			gameManagerRef.effects.Add(this.gameObject); // Here we add loot class object into GameManager.loot list;
		}
		if(this.gameObject.name == "Touch_Effect")
		{
			gameManagerRef.effects.Add(this.gameObject); // Here we add loot class object into GameManager.loot list;
		}
		
		gameObject.SetActive(false); // make loot object not active
		
	}

	public void DestroyOnGameReload()
	{
		//gameObject.SetActive(true);
		//Destroy(gameObject);
	}	

	// Update is called once per frame
	void Update () {
		
	}
}
