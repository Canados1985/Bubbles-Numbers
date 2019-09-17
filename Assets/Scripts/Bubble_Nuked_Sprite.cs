using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble_Nuked_Sprite : MonoBehaviour {


	public SpriteRenderer bubble_nuked_renderer;

	Color colorAlpha;
	float alpha = 0.0f;
	bool alphaFull = false;
	
	// Use this for initialization
	void Start () {

		colorAlpha.a = 0f;
		alpha = 0.0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(this.gameObject.activeSelf == true)
		{

			if(colorAlpha.a < 1 && !alphaFull)
			{
				alpha += 0.05f;		
			}
			if(colorAlpha.a >= 1)
			{
				alphaFull = true;
			}
			if(alphaFull && colorAlpha.a > 0)
			{
				alpha -= 0.05f;	
			}
			if(colorAlpha.a <= 0)
			{
				alphaFull = false;
			}
			
		}

		colorAlpha = new Color(1.0f, 1.0f, 1.0f, alpha);
		bubble_nuked_renderer.color = colorAlpha;	
	}
	void Update()
	{
		if(this.gameObject.activeSelf == false)
		{
			alpha = 0.0f;
		}
	}
}
