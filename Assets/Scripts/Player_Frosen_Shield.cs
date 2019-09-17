using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Frosen_Shield : MonoBehaviour {

	
	public SpriteRenderer frozen_shield_renderer;

	Color colorAlpha;
	float alpha = 0.0f;
	
	// Use this for initialization
	void Start () {

		colorAlpha.a = 0f;
		alpha = 0.0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(this.gameObject.activeSelf == true && colorAlpha.a < 1)
		{
			alpha += 0.05f;
		}

		colorAlpha = new Color(1.0f, 1.0f, 1.0f, alpha);
		frozen_shield_renderer.color = colorAlpha;	
	}
	void Update()
	{
		if(this.gameObject.activeSelf == false)
		{
			alpha = 0.0f;
		}
	}
}
