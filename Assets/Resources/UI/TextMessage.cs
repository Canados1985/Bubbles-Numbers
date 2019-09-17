using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMessage : MonoBehaviour {


	Text thisText;

	float timer;

	// Use this for initialization
	void Start () {

		if(this.gameObject.activeSelf == true)
		{
		thisText = this.gameObject.GetComponent<Text>();
		timer = 0;

			if(this.gameObject.name=="It is you turn")
			{
				//Debug.Log("Apply text");
				thisText.text = "GO";
			}
			if(this.gameObject.name=="Tap for reborn your bubble")
			{
				thisText.text = "TAP FOR REVIVE";
			}
		}
	}
	/* 
	void FixedUpdate()
	{
		timer += Time.deltaTime;	

		if(timer <= 0.25f)
		{	

		}
		if(timer >= 0.5f)
		{
			thisText.text = "";
			
		}
		if(timer >= 0.75f)
		{
			timer = 0;
		}

	}
	*/
	// Update is called once per frame
	void Update () {
		
	}
}
