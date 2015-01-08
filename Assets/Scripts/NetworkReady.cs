using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkReady : MonoBehaviour {
	//Attach this script to a gameobject 
	//  that has another script with an Act function
	//Another script will also call the BeReady function
	//  and the server will call SendAct for all classes
		//called directly by button
		
	public static int numReady = 0;
	public int intToSend = -1;
	public Vector3 vectorToSend = default(Vector3);
	public bool imReady
	{
		get { 
			return _imReady;
		}
		set { 
			_imReady = value;
			if(readyLabel!=null)
				readyLabel.SetActive(value);
		}
	}
	private bool _imReady;
	
	public GameObject readyLabel = null;
	
	/* list of all networkReady scripts in scene so players can communicate with each other */
	public List<NetworkReady> listeners = new List<NetworkReady>();
	
	public void Start()
	{
		imReady = false;
		Invoke("DelayedStart",0.5f);
	}
	
	public void DelayedStart()
	{
		if(gameObject.tag!="Mole"&&gameObject.tag!="Player")
		{
			Debug.Log("did you add self "+listeners.Count + " my name "+gameObject.name);
			listeners.Add(this);
			return;
		}
		StartCoroutine(AddObjectsWithTag("Player",1));
		StartCoroutine(AddObjectsWithTag("Mole",Network.connections.Length));
		Debug.Log("who's listening to NetworkReady? "+listeners.Count + " my name "+gameObject.name);
	}
	public IEnumerator AddObjectsWithTag(string tag,int min)
	{
		GameObject[] objects = new GameObject[0];
		while(objects.Length<min)
		{
			objects = GameObject.FindGameObjectsWithTag(tag);
			foreach(GameObject go in objects)
			{
				Debug.Log("did you add "+tag+listeners.Count + " my name "+gameObject.name);
				listeners.Add(go.GetComponent<NetworkReady>());
			}
			yield return 0;
		}
	}
	
	public void BeReady()
	{
		if(imReady)
			return;
		Debug.Log("beReady imReady?"+imReady+"abstract network is ready: "+ numReady+" to others "+Network.connections.Length);
		imReady = true;
		networkView.RPC("SetMove",RPCMode.OthersBuffered,vectorToSend);
		if(!Network.isServer)
			networkView.RPC ("AnyReady",RPCMode.Server);
		else AnyReady();
	}
	
	[RPC]
	public void SetMove(Vector3 pos)
	{
		Debug.Log("move set"+gameObject.name + " is mine" + networkView.isMine + " numReady"+numReady+"imReady"+imReady);
		vectorToSend = pos;
	}
	
	//Called by network
	[RPC]
	public void AnyReady()
	{
		//Each client and server calls BeReady on RPC which increments server's numReady variable
		//When server receives enough/correct number of numReady calls it starts the game
		//An additional complication is that each player has a different networkView and networkReady script attached
		//Current solution is to make numReady a static variable that each networkView/player increments by self
		//When numReady reaches correct number, this networkView/player alerts all the other networkViews/players that they should sendAct
		if(!Network.isServer)
			return;
		numReady+=1;
		int numConnected = Network.connections.Length;
		Debug.Log("any ready hit from "+gameObject.name+" with ready "+numReady);
		//Network.connections.Length is the number of clients connected to the server (doesn't count the server itself
		if(Network.isServer && numConnected>0 && numReady==numConnected+1)
		{
			Debug.Log("server et all are any ready and sending.");
			foreach(NetworkReady net in listeners)
			{
				Debug.Log("I'm serving all the listeners"+listeners.Count);
				net.BroadcastActs();
			}
		}
	}//*/
	
	public void BroadcastActs()
	{
		numReady = 0;
		Debug.Log("broadcasting acts i "+intToSend + " or v "+vectorToSend + " from "+gameObject.name);
		if(Network.isServer)
			networkView.RPC("SendAct",RPCMode.OthersBuffered,intToSend,vectorToSend);
		SendAct(intToSend,vectorToSend);
	}
	
	[RPC]
	public void SendAct(int i=-1,Vector3 v = default(Vector3))
	{	
		imReady = false;
		//send optional integer and vector arguments
		object value = null;
		if(i!=-1)
		{
			value = i as object;
			i = -1; //reset back to default for next time
		}
		else if(v!=default(Vector3))
		{
			value = v as object;
			v = default(Vector3); //reset back to default for next time
		}
		gameObject.SendMessage("Act",value,SendMessageOptions.DontRequireReceiver);
	}

}