using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Menu))]
public class StartMenu : MonoBehaviour {

	private Menu menuScript;
	
	void Awake () {
		menuScript = gameObject.GetComponent<Menu>();
		menuScript.AddFunction(0,
			delegate()
			{
				Application.LoadLevel("Level1_part2_v1");
			});
	}
	
	void Start () {
		Screen.showCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetKeyDown("enter") )
		{
			menuScript.callMethod(0);
		}
	}
	
}