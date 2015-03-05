﻿//*********************************************************
// Societe: ETML
// Auteur : Vincent Mouquin
// Date : 16.02.15
// But : Network control test script
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

	public static GameObject playerPrefabSusan;
	public static HostData[] hostList;

	public static void StartServer()
	{
		Network.InitializeServer (2, 25000, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (STR_GAME_NAME, MainController.strPlayerName);

		MainController.blnIsHost = true;
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

	void OnServerInitialized()
	{
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList();
		}
	}

	void OnConnectedToServer()
	{
	}
}
