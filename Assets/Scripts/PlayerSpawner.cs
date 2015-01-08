using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
	public List<Transform> spawns;
	public List<GameObject> playerPrefabs;
	private List<int> order = new List<int>();
	private WormSpawner wormSpawner;
 
	void Start()
	{
		Debug.Log("player spawner start is server? "+Network.isServer);
		wormSpawner = gameObject.GetComponent<WormSpawner>();
		//let server decide who spawns who
		if(Network.isServer)
			DecidePlayerOrder();
		else BeReady();
	}
	
	void Update()
	{
		if (Input.GetKeyDown("r"))
		{
			networkView.RPC("RestartLevel",RPCMode.OthersBuffered);
			RestartLevel();
		}
	}
	
	[RPC]
	public void RestartLevel()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
	
	/* BAD CODE HERE COPIED FROM NETWORKREADY.CS */
	int numReady = 0;
	
	public void BeReady()
	{
		Debug.Log("abstract network is ready in PlayerSpawner: "+ " to others "+Network.connections.Length);
		if(!Network.isServer)
			networkView.RPC ("AnyReady",RPCMode.Server);
		else AnyReady();
	}
	
	//Called by network
	[RPC]
	public void AnyReady()
	{
		//Each client and server calls BeReady on RPC which increments server's numReady variable
		//When server receives enough/correct number of numReady calls it starts the game
		numReady+=1;
		int numConnected = Network.connections.Length;
		Debug.Log("any ready with connected: "+numConnected + " and ready: "+numReady + "isServer?"+Network.isServer);
		//Network.connections.Length is the number of clients connected to the server (doesn't count the server itself
		if(Network.isServer && numConnected>0 && numReady==numConnected+1)
		{
			Debug.Log("server et all are ready.");
			SendAct();
		}
	}//*/
	
	private void DecidePlayerOrder()
	{
		order.Add(0); //add one for the server
		for(int i =0; i<Network.connections.Length;i++)
		{
			order.Add(i+1);
		}
		order.Shuffle();
		//FakePlayerOrder();
		BeReady();
	}
	
	/* fake the order for testing, so mole is server, fox is not */
	private void FakePlayerOrder()
	{
		order[0] = 1;
		order[1] = 0;
	}
	
	private void SendAct()
	{
		//then each spawns own individual character
		for(int i=0; i<Network.connections.Length;i++)
		{
			Debug.Log("to player "+Network.connections[i]+" sent some order"+order[i+1]);
			networkView.RPC("SpawnPlayer",Network.connections[i],order[i+1]);
		}
		SpawnPlayer(order[0]);
		wormSpawner.SpawnWorms();
	}
	 
	[RPC]
	public void SpawnPlayer(int whichPlayer)
	{
		Debug.Log("trying to spawnPlayer "+whichPlayer);
		Vector3 spawnPos = spawns[whichPlayer].position;
		GameObject playerPrefab = playerPrefabs[whichPlayer];
		Debug.Log(spawnPos);
		GameObject player = (GameObject) Network.Instantiate(playerPrefab, spawnPos, Quaternion.identity, 0);
	}
	

}