using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent (typeof (GUITexture))]
public class GUITextureContainer : GUIContainer {
	
	//texture to draw
	public Texture2D texture;
	//a list of textures we can switch between
	public List<Texture2D> textures;
	private int curTexture = 0;
	private GUITexture guiTexture = null;
	public float screenPercent = 0.125f; //default 1/8 of screen
	
	override protected void Start (){
		//getcomponents
		guiTexture = GetComponent<GUITexture>();
		
		if(textures.Count==0)
			textures.Add(texture);
		
		guiTexture.texture = textures[curTexture];
			
		float w = Screen.width*screenPercent;
		float h = Screen.height*screenPercent;
		guiTexture.pixelInset = 
		    new Rect(-w/2, -h/2, 
		    w, h);
			
		base.Start();
	}
	
	//make the texture invisible
	override public bool visible
	{
		get
		{
			return (guiTexture.texture!=null);
		}
		set
		{
			//only change values if they need changing
			if(value!=visible)
			{
				if(value)
					guiTexture.texture = textures[curTexture];
				else guiTexture.texture = null;
			}
		}
	}
	
/*	public void setTexture(Texture2D tex)
	{
		texture = tex;
		if (visible)
		{
			guiTexture.texture = tex;
		}
	}*/
	
	override public void switchToNext()
	{
		curTexture += 1;
		if(curTexture >= textures.Count)
			curTexture = 0;
		if(visible)
		{
			guiTexture.texture = textures[curTexture];
		}
	}
}