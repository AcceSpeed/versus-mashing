﻿//*********************************************************
// Societe: ETML
// Auteur : Vincent Mouquin
// Date : 09.02.15
// But : Network control script
//*********************************************************
// Modifications:
// Date :
// Auteur :
// Raison :
//*********************************************************
// Date :
// Auteur :
// Raison :
//*********************************************************

using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string STR_GAME_NAME = "VersusSmashingNetwork";
	private string strRoomComment;
	
	public static HostData[] hostList;

	public static void StartServer()
	{
		Network.InitializeServer (2, 25000, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (STR_GAME_NAME, MainController.strPlayerName);

		MainController.blnIsHost = true;
		LoadLevel();
	}

	public static void RefreshHostList()
	{
		MasterServer.RequestHostList(STR_GAME_NAME);
	}

	public static void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
		MainController.blnIsHost = false;
	}

	public static void FindOpponent (){
		if (hostList != null) {
			// check the avaliability of the hosts
			foreach (HostData host in hostList) {
				if(host.connectedPlayers == 1){
					// join the host and stop the search
					JoinServer(host);
					return;
				}
			}
		}

		// set self as host
		StartServer();
	}

	private static void LoadLevel (){
		Application.LoadLevel ("Arena");
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList();
		}
	}

	void OnPlayerConnected(NetworkPlayer player){
		GameLoader.blnResetStage = true;
	}

	void OnConnectedToServer(){
		LoadLevel ();
	}

	void OnDisconnectedFromServer ()
	{
		Application.LoadLevel("MainMenu");
	}
	
}
