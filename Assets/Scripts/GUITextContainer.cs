using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent (typeof (GUIText))]
public class GUITextContainer : GUIContainer {
	
	//a list of textures we can switch between
	public List<string> texts;
	private int curContainer = 0;
	public GUIText guiText = null;
	
	override protected void Start (){
		//getcomponents
		guiText = GetComponent<GUIText>();
		
		//auto initialize to work intuitively with only one text string
		if(texts.Count == 0 && guiText.text!=null)
			texts.Add(guiText.text);
		
		guiText.text = texts[curContainer];
			
		//set texture to take up 1/8 of screen
		/*float screenPercent = 8;
		float w = Screen.width/screenPercent;
		float h = Screen.height/screenPercent;
		guiText.pixelInset = 
		    new Rect(-w/2, -h/2, 
		    w, h);*/
			
		base.Start();
	}
	
	//make the texture invisible
	override public bool visible
	{
		get
		{
			return (guiText.text!=null);
		}
		set
		{
			//only change values if they need changing
			if(value!=visible)
			{
				if(value)
					guiText.text = texts[curContainer];
				else guiText.text = null;
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
		curContainer += 1;
		if(curContainer >= texts.Count)
			curContainer = 0;
		if(visible)
		{
			guiText.text = texts[curContainer];
		}
	}
}