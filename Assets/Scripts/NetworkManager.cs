﻿//*********************************************************
// Societe: ETML
// Auteur : Vincent Mouquin
// Date : 09.02.15
// But : Network control script
//*********************************************************
using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	//Constants
	private const string STR_GAME_NAME = "VersusSmashingNetwork";	//Specifiy the name of the game when registering a room on the MasterServer
	private const int INT_MAX_CONNECTIONS = 2;						// maximum number of connections
	private const int INT_PORT = 26000;								// port of the game

	//Variables
	private static string strRoomComment;	//Rooms informations can contain strings, this will be used to differenciate Matchmaking v. non-matchmaking rooms

	public static HostData[] hostList;	//Used to store the informations about all the active "matches" (rooms) registered on the MasterServer for a defined game

	void Update(){
		Debug.Log("Is a Client : " + Network.isClient);
		Debug.Log("Is a Server : " + Network.isServer);
		Debug.Log("Number of connections : " + Network.connections.Length);
	}

	// *******************************************************************
	// Nom : StartServer
	// But : Registering the software on the MasterServer as an active game-hosting server
	// Retour: Void
	// Param.: None
	// *******************************************************************
	public static void StartServer(string strQueueType)
	{
		Debug.Log ("Connection as server");
		strRoomComment = strQueueType;

		//Initialize a room on the Unity Master Server, using the max. number of players, the port which will be used, the NAT options, 
		//the game name, and the name of the player (obtained on login) as the name of the room
		bool blnUseNat = !Network.HavePublicAddress ();

		Network.InitializeServer (INT_MAX_CONNECTIONS, INT_PORT, blnUseNat);
		MasterServer.RegisterHost (STR_GAME_NAME, MainController.strPlayerName, strQueueType);

		//When registreing, the software becomes a server, then loads the game
		MainController.blnIsHost = true;
		LoadLevel();
	}

	// *******************************************************************
	// Nom : RefreshHostList
	// But : Getting the newest host (rooms) list from the MasterServer
	// Retour: Void
	// Param.: None
	// *******************************************************************
	public static void RefreshHostList(){
		MasterServer.RequestHostList(STR_GAME_NAME);
	}

	// *******************************************************************
	// Nom : JoinServer
	// But : Connecting to the selected host using Unity networking processes
	// Retour: Void
	// Param.: None
	// *******************************************************************
	public static void JoinServer(HostData hostData){
		//Connect to the selected host (taken from the Hostlist)
		Debug.Log ("Connection as client");
		Network.Connect(hostData);

		//Thus the software isn't a host itself
		MainController.blnIsHost = false;
	}

	// *******************************************************************
	// Nom : FindOpponent
	// But : Searches in the Host List an available room to connect to, and if found, joins it
	// Retour: Void
	// Param.: None
	// *******************************************************************
	public static void FindOpponent (){

		RefreshHostList();

		//If the hostList actually contains something
		if (hostList != null) {
			//checks the avaliability of the hosts
			foreach (HostData host in hostList) {
				if(host.connectedPlayers == 1 && host.comment==MainController.STR_QUEUE_TYPE_MATCH){
					//joins the host and stops the search
					JoinServer(host);
					return;
				}
			}
		}

		// set self as host
		StartServer(MainController.STR_QUEUE_TYPE_MATCH);
	}

	// *******************************************************************
	// Nom : LoadLevel
	// But : When the software either becomes host or client of a room, load the arena
	// Retour: Void
	// Param.: None
	// *******************************************************************
	private static void LoadLevel (){
		Application.LoadLevel ("Arena");
	}

	// *******************************************************************
	// Nom : OnMasterServerEvent (Unity Script)
	// But : When an event occurs between the MasterServer and the software, update the displayed list based on the local list
	// Retour: Void
	// Param.: None
	// *******************************************************************
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList();
		}
	}

	// *******************************************************************
	// Nom : OnPlayerConnected (Unity Script)
	// But : When a client connects to the room (the only possible client), that means both players are connected, reset the stage
	// Retour: Void
	// Param.: None
	// *******************************************************************
	void OnPlayerConnected(NetworkPlayer player){
		GameController.blnResetStage = true;
	}

	// *******************************************************************
	// Nom : OnConnectedToServer (Unity Script)
	// But : When the software connects to a room, load the Arena
	// Retour: Void
	// Param.: None
	// *******************************************************************
	void OnConnectedToServer(){
		LoadLevel ();
	}

	// *******************************************************************
	// Nom : OnDisconnectedFromServer (Unity Script)
	// But : When the software is disconnected from the room, load the Menu
	// Retour: Void
	// Param.: None
	// *******************************************************************
	void OnDisconnectedFromServer (){
		Application.LoadLevel("MainMenu");
	}
}
