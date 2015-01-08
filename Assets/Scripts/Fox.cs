using UnityEngine;
using System.Collections;

[System.Serializable]
public class Fox : MonoBehaviour, Actionable {
	Vector3 toMove;
	
	GridManager grid;
	public GameObject hole;
	public GameObject victory;
	public Points points;
	
	public Indicator indicator;
	Vector3 zOffset = new Vector3(0,0,10);
	
	void Start()
	{
		grid = GridManager.instance;
		victory.transform.parent = null;
		victory.SetActive(false);
		indicator = gameObject.GetComponent<Indicator>();
		if(!networkView.isMine)
			points.visible = false;
	}
	
	public void Update()
	{
		if(!networkView.isMine)
		{
			return;
		}
		if(Input.GetMouseButtonDown(0))
		{
			//Get location pressed and store it for later
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 ijPos = grid.xyzToij(mousePos);
			mousePos = grid.ijToxyz(ijPos);
			Debug.Log("we try"+mousePos+" snapped to "+ijPos);
			gameObject.SendMessage("SetMove",mousePos);//,SendMessageOptions.DontRequireReceiver);
			indicator.SpawnIndicator(mousePos+zOffset,Quaternion.identity);
		}
		if (Input.GetKeyDown("space"))
		{
			gameObject.SendMessage("BeReady");
		}
	}
	
	Vector3 toDig;
	
	public void Act(Vector3 pos = default(Vector3))
	{
		toDig = pos;
		Debug.Log("fox wants to act dig holes!");
		Invoke("DigHoles",1.0f); /* fox digs holes after the moles move */
	}
	
	public void DigHoles()
	{
		Vector3 pos = toDig;
		Debug.Log("I am digging a deep hole said the fox!");
		indicator.TryToDestroyArrow();
		if(pos==default(Vector3))
			return;
		//dig at pos
		RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.zero);
		for(int i=0;i<hits.Length;i++)
		{
			RaycastHit2D hit = hits[i];
			if(hit != null)
			{
				if(hit.collider != null)
				{
					if(hit.collider.tag == "Dirt")
					{
						Network.Instantiate(hole,hit.collider.transform.position,hit.collider.transform.rotation,0);
						//hit.collider.gameObject.SendMessage("Click", SendMessageOptions.DontRequireReceiver);
						//Destroy(hit.collider.gameObject);
					}
					if(hit.collider.tag == "Mole")
					{
						EatMole(hit.collider.gameObject);
					}
					//Debug.Log ("Target Name: " + hit.gameObject.name);
				}
			}
		}
	}
	
	private void EatMole(GameObject mole)
	{
		GameObject.Destroy(mole);
		points.points += 100;
		Debug.Log("ate a mole"+GameObject.FindGameObjectsWithTag("Mole").Length);
		if(GameObject.FindGameObjectsWithTag("Mole").Length<2)
			Victory();
	}
	
	private void Victory()
	{
		points.points += 200;
		victory.SetActive(true);
		Debug.Log("Victory!");
	}
	
	[RPC]
	public void HideWorms()
	{
		if(!networkView.isMine)
			return;
		foreach(GameObject worm in GameObject.FindGameObjectsWithTag("Worm"))
		{
			worm.SetActive(false);
		}
	}
}