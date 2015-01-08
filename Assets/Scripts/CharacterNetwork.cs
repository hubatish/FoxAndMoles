using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterNetwork : MonoBehaviour {
	//this script should probably go on each character object
	//each character object will send/be sent messages by this script about what action is/when it should be executed
	//each of these scripts should send message to server with "ready"
	//server should wait for all of them and then send out "go!"
	Vector3 toMove = new Vector3(0,0,0);
	public bool moveReady = false;
	
	public int numReady = 0;
	
	//public Actionable action;
	//public List<GameObject> characters = new List<GameObject>();
	
	void Start()
	{
		//action = gameObject.GetComponent<Actionable>();
	}
	
	void Update()
	{
		if (Input.GetMouseButtonDown (1) || Input.GetMouseButtonDown (2)) 
		{
			toMove = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
		if (Input.GetKeyDown("space"))
		{
			SendAct(toMove);
		}
	}
	
	public void SetMove(Vector3 pos)
	{
		Debug.Log("move set");
		toMove = pos;
		moveReady = true;
	}
	
	[RPC]
	public void BeReady()
	{
		Debug.Log("I'm so ready!"+Network.connections.Length);
		if(networkView.isMine)
				networkView.RPC ("BeReady",RPCMode.OthersBuffered);
		numReady+=1;
		//Network.connections.Length is the number of clients connected to the server
		if(numReady==Network.connections.Length-1)
		{
			SendAct(toMove);
			numReady = 0;
		}
	}
	
	[RPC]
	public void SendAct(Vector3 pos)
	{
		if(!moveReady)
			return;
		moveReady = false;
		
		if(networkView.isMine)
		{
			networkView.RPC ("SendAct",RPCMode.OthersBuffered,pos);
		}
		Debug.Log ("delayed at position "+pos+ "? ");
		
	}
}