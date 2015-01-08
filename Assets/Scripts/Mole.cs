using UnityEngine;
using System.Collections;

public class Mole : MonoBehaviour {

	private bool h_isAxisInUse = false;
	private bool v_isAxisInUse = false;
	
	public Indicator indicator;
	
	public GridManager grid;
	public GameObject victory;
	public Points points;
	
	void Start()
	{
		grid = GridManager.instance;
		victory.SetActive(false);
		indicator = gameObject.GetComponent<Indicator>();
		if(!networkView.isMine)
			points.visible = false;
	}
	 
	void Update()
	{
		if(!networkView.isMine)
		{
			return;
		}
		//take directional input from user
		Vector2 dir = new Vector2(0,0);
		if( Input.GetAxisRaw("Horizontal") != 0)
		{
			if(h_isAxisInUse == false)
			{
				// Call your event function here.
				h_isAxisInUse = true;
				dir.x = Input.GetAxisRaw("Horizontal");
			}
		}
		if( Input.GetAxisRaw("Horizontal") == 0)
		{
			h_isAxisInUse = false;
		}    
		
		if( Input.GetAxisRaw("Vertical") != 0)
		{
			if(v_isAxisInUse == false)
			{
				// Call your event function here.
				v_isAxisInUse = true;
				dir.y = Input.GetAxisRaw("Vertical");
			}
		}
		if( Input.GetAxisRaw("Vertical") == 0)
		{
			v_isAxisInUse = false;
		}    
		
		//actually move character on a grid
		if(dir.magnitude>0)
		{
			Vector3 dir3 = new Vector3(dir.x,dir.y,0);
			gameObject.SendMessage("SetMove",dir3);//,SendMessageOptions.DontRequireReceiver);
			indicator.SpawnIndicator(transform.position+dir3,transform.rotation);
			//Act(new Vector3(dir.x,dir.y,0));
		}
		if (Input.GetKeyDown("space"))
		{
			gameObject.SendMessage("BeReady");
		}
	}
	
	public string getTagAtPos(Vector3 pos, out Collider2D col)
	{
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero,Mathf.Infinity,~(1<<8));
		if(hit != null)
		{
			if(hit.collider != null)
			{
				col = hit.collider;
				return hit.collider.tag;
			}
			else 
			{
				col = null;
				return "";
			}
		}
	}

	public void Act(Vector3 pos = default(Vector3))
	{
		//Debug.Log("I, said the mole with a chest full of air, simply act.");
		indicator.TryToDestroyArrow();
		Vector2 dir = new Vector2(0,0);
		if(pos!=null && pos!=default(Vector3))
			dir = new Vector2(pos.x,pos.y);
		Vector2 curPos = grid.xyzToij(transform.position);
		Vector3 newPos = grid.ijToxyz(curPos + dir);
		Collider2D col = null;
		string tag = getTagAtPos(newPos,out col);
		Debug.Log("mole is trying to move and will hit "+tag+"maybe even "+col);
		if(tag!="Hole")
		{
			newPos.z = 0;
			transform.position = newPos;
		}
		if(tag=="Worm")
			EatWorm(col.gameObject);
	}
	
	void EatWorm(GameObject worm)
	{
		GameObject.Destroy(worm);
		points.points += 100;
		if(GameObject.FindGameObjectsWithTag("Worm").Length<2)
			Victory();
	}
	
	void Victory()
	{
		points.points += 200;
		victory.SetActive(true);
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag=="Worm")
		{
			EatWorm(other.gameObject);
		}
	}
}