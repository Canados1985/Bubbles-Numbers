using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : Effect {

	float timer = 0f; // timer

	public override void Start () {
		
			
			base.Start();
		
			InitEffect();
			
	}
	
	void FixedUpdate()
	{
		if(this.gameObject.activeSelf)
		{
			timer += Time.deltaTime;
		}
		if(timer >= 0.25f)
		{
			timer = 0;
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
