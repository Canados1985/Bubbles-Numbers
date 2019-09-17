using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleEnd : MonoBehaviour {

	GameManager gameManagerRef;
	SpriteRenderer spriteRenderer;
	Animator animator_Child;

	public void BubbleEndInst(Vector3 newScale)
	{
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator_Child = gameObject.GetComponent<Animator>();


		//gameObject.transform.parent = null;
		gameManagerRef = GameManager.gameManager;
		this.transform.localScale = newScale;

		if(gameObject.name == "blue" || gameObject.name == "blue_idle")
		{
			
			animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[0];
		}
		
		if(gameObject.name == "green"|| gameObject.name == "green_idle")
		{
			
			animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[1];
		}

		
		if(gameObject.name == "pink" || gameObject.name == "pink_idle")
		{
			
			animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[2];
		}

		if(gameObject.name == "purple" || gameObject.name == "purple_idle")
		{
			
			animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[3];
		}

		
		if(gameObject.name == "red" || gameObject.name == "red_idle")
		{
			
			animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[4];
		}

		
		if(gameObject.name == "white" || gameObject.name == "white_idle")
		{
			
			animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[5];
		}

		
		if(gameObject.name == "yellow" || gameObject.name == "yellow_idle")
		{
			
			animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[6];
		}

		if(gameObject.name == "player" || gameObject.name == "PlayerCopy_Effect_0" || gameObject.name == "PlayerCopy_Effect_1" || gameObject.name == "PlayerCopy_Effect_2")
		{
			animator_Child.runtimeAnimatorController = gameManagerRef.bubblesController[7];
		}
		animator_Child.SetBool("anim_starts", true);
		Invoke("DeactivateBubbleEnd", 0.5f);
	}

	// Use this for initialization
	void Start () {
		
		//BubbleEndInst();
	}

	public void DestroyOnGameReload()
	{
		this.gameObject.SetActive(true);
		Destroy(this.gameObject);
	}

	void DeactivateBubbleEnd()
	{
		this.gameObject.SetActive(false);
		this.gameObject.transform.position = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
