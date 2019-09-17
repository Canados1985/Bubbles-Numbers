using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : Effect {

	float timer = 0f; // timer

	public override void Start () {
		
		base.Start();
		timer = 0f;
		InitEffect();
		this.gameObject.SetActive(false);	
	}
	
	public void SetTimerToZero()
	{	
		timer = 0f; // timer
		//Debug.Log(this.transform.position);
	}	


	void FixedUpdate()
	{
		if(this.gameObject.activeSelf)
		{
			timer += Time.deltaTime;
		}
		if(timer >= 0.85f)
		{
			timer = 0;
			this.gameObject.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update () {
		
		if(this.gameObject.activeSelf == false && timer != 0)
		{
			gameObject.transform.position = effectsContainer.transform.position;
		}
	}
}
