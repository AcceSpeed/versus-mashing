﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonFunctions : MonoBehaviour {

	public GameObject GObjRoomsContainer;
	public GameObject GObjRoomButton;
	public Text txtPlayerName;

	private GameObject roomButtonInstantiate;
	private int intButtonDecal = 10;
	private int intButtonMultiple = 0;

	public void LoadLevel (){
		Application.LoadLevel ("TestNetwork");
	}

	public void CreateGame (){
		NetworkManager.StartServer ();
	}

	public void QuitGame(){
		Application.Quit();
	}

	public void DisplayRooms (){
		NetworkManager.RefreshHostList ();


		if (NetworkManager.hostList != null) {
			foreach (var host in NetworkManager.hostList) {
				
				roomButtonInstantiate = Instantiate(
					GObjRoomButton
					) as GameObject;
				
				roomButtonInstantiate.transform.SetParent(GObjRoomsContainer.transform, false);
				roomButtonInstantiate.transform.position += Vector3.down * intButtonMultiple * intButtonDecal ;
				roomButtonInstantiate.guiText.text = host.gameName   ;
					
				intButtonMultiple++;
			}	
		}
	}

	public void ChooseName(){
		MainController.strPlayerName = txtPlayerName.text;

		DisplayRooms ();
	}
}
