using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GUIContainer))]
public class GUIButton : MonoBehaviour
{
	//either define a delegate for the button using code
	public delegate void Method();
	public Method method;
	
	//or invoke a function with invokeName on the inspector defined scriptDestination gameObject
	public GameObject scriptDestination;
	public string invokeName;
	
	public GUIContainer icon;
	
	void Start()
	{
		icon = gameObject.GetComponent<GUIContainer>();
	}
	
	void OnMouseDown()
	{
		callMethod();
	}
	
	public void callMethod()
	{
		if(method==null)
		{
			if(scriptDestination!=null && invokeName!="")
				scriptDestination.SendMessage(invokeName,SendMessageOptions.DontRequireReceiver);
			else
				throw new System.ArgumentException("Button's method is not defined");
		}
		else
			method();
	}
	
	//make the texture invisible
	public bool visible
	{
		get 
		{
			return icon.visible;
		}
		set
		{
			icon.visible = value;
		}
	}
	
	/*public void setTexture(Texture2D tex)
	{
		icon.setTexture(tex);
	}*/
	
	public void switchToNext()
	{
		icon.switchToNext();
	}
}

