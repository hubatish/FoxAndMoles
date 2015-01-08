﻿using UnityEngine;
using System.Collections;

public class MainMenuNetwork : MonoBehaviour {

	private const string typeName = "FoxGame2-25";
	private const string gameName = "RoomName";

	public GameObject mainMenuPanel;
	public GameObject waitingPanel;
	
	public GameObject readyButton;
	public GameObject waitingLabel;

	public int numReady = 0;
	public bool imReady = false;
	
	bool levelStarted = false;

	void Start()
	{
	//	DontDestroyOnLoad(this);
		//InvokeRepeating ("RefreshHostList",1f,3f);
	}
	
	//Called by Button
	public void StartServer()
	{
		Network.InitializeServer(2, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		WaitButtons();
		//ButtonsSetWaiting(true);
	}
	
	void OnServerInitialized()
	{
		Debug.Log("Server initialized!");
	}
	
	private HostData[] hostList;
	
	private void RefreshHostList()
	{
		if(!levelStarted)
			MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
		if(hostList!=null)
		{
			foreach(HostData host in hostList)
				Debug.Log("the host "+host+host.connectedPlayers+host.ip);
		}
	}
	
	//Called by button
	public IEnumerator JoinServer()
	{
		Debug.Log("try to connect!");
		RefreshHostList();
		//wait for a bit cause refreshing takes a second
		for (float timer = .8f; timer >= 0; timer -= Time.deltaTime)
            yield return 0;
		if(hostList!=null && hostList.Length>0)
		{
			Debug.Log("we should be connected."+hostList[0]);
			Network.Connect(hostList[0]);
			//ButtonsSetWaiting(true);
			WaitButtons();
		}
		
		//else Debug.Log("no connection yet!");
	}
	
	void OnFailedToConnect(NetworkConnectionError error) {
        Debug.Log("Could not connect to server: " + error);
    }
	
	//called directly by button
	public void ImReady()
	{
	//	ConnectionTesterStatus connected = Network.TestConnection();
		readyButton.SetActive(false);
		gameObject.SendMessage("BeReady",SendMessageOptions.DontRequireReceiver);
	}

	public void Act()
	{
		/*if(Network.isServer)
		{
			Invoke("StartLevel",4f);
		}
		else StartLevel(); commented out waiting stuff cause next scene should delay itself */
		StartLevel();
	}
	
	//Called by network
	[RPC]
	public void StartLevel()
	{
		Debug.Log("someone's strying to start level!");
		Application.LoadLevel(1);
		levelStarted = true;
	}
	
	//Enable waiting button, disable start/join server
	private void WaitButtons()
	{
		if(!waitingPanel.activeSelf)
		{
			waitingPanel.SetActive(true);
			mainMenuPanel.SetActive(false);
			readyButton.SetActive(false);
			waitingLabel.SetActive(true);
		}
		if (Network.connections.Length==0)//(connected == ConnectionTesterStatus.Undetermined)
		{
		//	Debug.Log("not found yet");
            Invoke("WaitButtons",.1f);
			return;
		}
		//Found network
		readyButton.SetActive(true);
		waitingLabel.SetActive(false);
	}
	
	//Enable/disable buttons
	private void ButtonsSetWaiting(bool waiting)
	{
		waitingPanel.SetActive(waiting);
		mainMenuPanel.SetActive(!waiting);
		readyButton.SetActive(waiting);
	}
	
	//Called by button
	public void ICancel()
	{
		networkView.RPC("AnyCancel",RPCMode.OthersBuffered);
		ButtonsSetWaiting(false);
		numReady = 0;
		imReady = false;
		Network.Disconnect();
	}
	
	//Called by Network
	[RPC]
	public void AnyCancel()
	{
		numReady -= 1;
	}
}


