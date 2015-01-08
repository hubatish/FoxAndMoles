using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class GUIContainer : MonoBehaviour {
	
	public bool detach = false;
	
	protected virtual void Start (){
		//we don't want to be parented to anything - that would make us copy it's position, scale, rotation, etc
		//parents also behave oddly with guitextures (seems to erase local position when setting parent=null)
		Vector3 pos = transform.localPosition;
		if(!detach)
		{
			Transform prevParent = transform.parent;
			if(transform.parent!=null)
			{
				transform.parent = null;
				transform.parent = prevParent;
			}
		}
		else transform.parent = null;
		//our position needs to be in the range of (0..1,0..1,0)
		if(pos.x<0)
			pos.x = 0;
		else if (pos.x>1)
			pos.x=1;
		if(pos.y<0)
			pos.y = 0;
		else if (pos.y>1)
			pos.y = 1;
		transform.position = pos;
	}
	
	//make the texture invisible
	public abstract bool visible
	{
		get;
		set;
	}
	
/*	public void setTexture(Texture2D tex)
	{
		texture = tex;
		if (visible)
		{
			guiTexture.texture = tex;
		}
	}*/
	
	public abstract void switchToNext();
}