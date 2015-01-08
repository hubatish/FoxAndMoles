using UnityEngine;
using System.Collections;

public class Points : MonoBehaviour {
	
	public GUITextContainer pointsText;
	
	public void Awake()
	{
		pointsText = gameObject.GetComponent<GUITextContainer>();
	}
	
	public int points {
		get {
			return _points;
		}
		set {
			pointsText.guiText.text = value.ToString();
			_points = value;
		}
	}
	private int _points = 0;
	
	public bool visible
	{
		get
		{
			return (pointsText.visible);
		}
		set
		{
			pointsText.visible = value;
		}
	}
	
}