using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Menu : MonoBehaviour
{
	public List<GUIButton> buttons;
	
	public void Awake()
	{
		
	}
	
	public void AddFunction(int i, GUIButton.Method m)
	{
		buttons[i].method = m;
	}
	
	public void callMethod(int i)
	{
		if(i<buttons.Count)
			buttons[i].callMethod();
	}
	
	public bool visible
	{
		get
		{
			foreach (GUIButton b in buttons)
			{
				if(b.visible == false)
					return false;
			}
			return true;
		}
		set
		{
			foreach (GUIButton b in buttons)
			{
				b.visible = value;
			}
		}
	}
}