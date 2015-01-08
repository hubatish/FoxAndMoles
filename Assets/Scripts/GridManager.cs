using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour {

	public int numRows = 10;
	public int numColumns = 10;
	
	public static GridManager instance;
	
	//lots of grid variables
	Vector3 center;
	Vector3 ext;
	float w;
	float h;
	float xGridSize;
	float yGridSize;
	float xOffset;
	float yOffset;

	// Use this for initialization
	void Awake () {
		
		center = collider.bounds.center;
		ext = collider.bounds.extents;
		w = ext.x*2f;
		h = ext.y*2f;
		xGridSize = w/numRows;
		yGridSize = h/numColumns;
		xOffset = (center.x-ext.x+xGridSize/2);
		yOffset = (center.y-ext.y+yGridSize/2);
		GridManager.instance = this;
	}
	
	public void Update()
	{
		if (Input.GetKeyDown("r"))
			Application.LoadLevel(Application.loadedLevel);
		if (Input.GetMouseButtonDown (0)) {
		//	Debug.Log ("are we good at clicked position?" + checkGoodPosition (Input.mousePosition));
			//Debug.Log ("did we win?" + checkGrid ());
		} 
		else if (Input.GetMouseButtonDown (1) || Input.GetMouseButtonDown (2)) 
		{
			Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Debug.Log ("clicked at position "+mPos+ "? " + xyzToij(mPos));
		}
		
	}

	//just for debugging and helping me figure out where the corners of my rectangle were
	public void drawBounds2()
	{
		Vector3 c = collider.bounds.center;
		Vector3 e = collider.bounds.extents;
		//Get all four corners of the front face
		Vector3 pt1 = new Vector3(c.x-e.x,c.y+e.y,c.z);
		Vector3 pt2 = new Vector3(c.x-e.x,c.y-e.y,c.z);
		Vector3 pt3 = new Vector3(c.x+e.x,c.y+e.y,c.z);
		Vector3 pt4 = new Vector3(c.x+e.x,c.y-e.y,c.z);
		//Draw them to the screen as a rectangle
		Color lineColor = Color.green;
		Debug.DrawLine (pt1, pt2, lineColor);
		Debug.DrawLine (pt1, pt3, lineColor);
		Debug.DrawLine (pt2, pt4, lineColor);
		Debug.DrawLine (pt3, pt4, lineColor);
		//Print them to the screen individually
		Debug.Log ("top left: "+pt1);
		Debug.Log ("bottom left: "+pt2);
		Debug.Log ("top right: "+pt3);
		Debug.Log ("bottom left: "+pt4);
	}	
	
	//where i is x position, j is y position
	
	//given an xyz, return what that would be in i, j, from grid
	public Vector2 xyzToij(Vector3 xyzPos)
	{
		Vector2 ijPos = new Vector2();
		ijPos.x = Mathf.Round(  (xyzPos.x-xOffset)/xGridSize  );
		ijPos.y = Mathf.Round(  (xyzPos.y-yOffset)/yGridSize  );
		return ijPos;
	}
	
	//given an ij in grid, return that block's real x,y,z location
	public Vector3 ijToxyz(Vector2 ijPos)
	{
		return new Vector3((float)xOffset+ijPos.x*xGridSize,(float)yOffset+ijPos.y*yGridSize,-10f);
	}
	
	//given some i and j, return whether there is a block at that position
	public bool isThereABlockAtxyz(Vector3 xyzPos)
	{
		Vector2 pos = (Vector2) xyzPos;
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		if(hit.collider != null)
		{
			return true;
		}
		else return false;
	}

	//check each square on a "grid" of the plane to see if the player has completed the puzzle
	public bool checkGrid()
	{

		//Debug.Log("grid size x: "+xGridSize+" y:"+yGridSize);
        Vector3 lastPos = new Vector3(0,0,0);
        Vector3 debugOffset = new Vector3(0, 0, 5);
        bool rValue = true;

	    for(int j=0;j<numColumns;j++)
		{
            for (int i = 0; i < numRows; i++)
            {
				Vector2 ijPos = new Vector2(i,j);
				Vector3 pos = ijToxyz(ijPos);
            //    Debug.DrawLine(pos+ debugOffset,lastPos+debugOffset,Color.blue);
                lastPos = pos;
				//Debug.Log("grid checked: "+pos);
				if(!checkGoodPosition3D(pos))
				{
					//Debug.Log("pos: "+pos+" returned false");
					//return false;
                    //Debug.Log("pos failed:"+pos);
                    rValue = false;
				}
			}
		}
		return rValue;
	}
	
	public bool checkGoodPosition3D(Vector3 xyzPos)
	{
		//Check 2D collisions where we hit
		bool hitBlock = false;
		bool hitGoal = false;
		hitBlock = isThereABlockAtxyz(xyzPos);
		
		//Check 3D collisions where we hit
		//Ray ray = Camera.main.ScreenPointToRay(v);
		RaycastHit hit3D;
		bool weHit = Physics.Raycast(xyzPos,Vector3.forward,out hit3D,50);
        //Debug.DrawRay(v,Vector3.forward*50,Color.blue);
		if(weHit && hit3D.collider.tag=="Goal")
		{
			//if(checkColliderOpaque(hit3D))
				hitGoal = true;
		//	else Debug.Log("missed transparency at pos: "+v);
		}
		//return true if both casts hit, or both miss
		return !(hitBlock ^ hitGoal);
	}
	
}
