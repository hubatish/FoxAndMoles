using UnityEngine;
using System.Collections;

public class WormSpawner : MonoBehaviour {
	public GameObject worm;
	
	private GridManager grid;
	private int numWorms = 2;
	
	void Start()
	{
		grid = GridManager.instance;
	//	if(Network.isServer)
		//	SpawnWorms();
	}
	
	public void SpawnWorms()
	{
		Debug.Log("spawning some worms");
		for(int i=0;i<numWorms*Network.connections.Length;i++)
		{
			Vector2 ij = new Vector2(0,0);
			ij.x = Random.Range(0,grid.numRows);
			ij.y = Random.Range(0,grid.numColumns);
			Vector3 spawnPos = grid.ijToxyz(ij);
			spawnPos += new Vector3(0,0,10);
			Network.Instantiate(worm,spawnPos,Quaternion.identity,0);
		}
		TellFoxHide();
	}
	
	void TellFoxHide()
	{
		GameObject fox = (GameObject) GameObject.FindGameObjectWithTag("Player");
		if(fox==null)
		{
			Invoke("TellFoxHide",.01f);
			return;
		}
		fox.networkView.RPC("HideWorms",RPCMode.OthersBuffered); /* hide worms for the fox */
		fox.SendMessage("HideWorms");
	}
	
}