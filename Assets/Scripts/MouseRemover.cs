//Whenever the player clicks on a block, delete that block
using UnityEngine;

public class MouseRemover : MonoBehaviour
{
	public void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			//Check 2D collisions where we hit
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
			if(hit != null)
			{
				if(hit.collider != null && hit.collider.tag == "Block")
				{
					hit.collider.gameObject.SendMessage("Click", SendMessageOptions.DontRequireReceiver);
					//Destroy(hit.collider.gameObject);
				}
				//Debug.Log ("Target Name: " + hit.collider.gameObject.name);
			}
		}
	}

}