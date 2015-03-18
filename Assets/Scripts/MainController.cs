﻿//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 18.02.2015
// But : Main Controller class file who manage the entire fight
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

public class MainController : MonoBehaviour {

	//Variables

	public static bool blnMatchOver;			//Used when a round is over
	public static string strPlayerName = "";	//Player's name, entered at the launch and stored for use in the Display Rooms

	public static bool blnIsHost;				//Used to know if the current software is a server or a client for that game

	// *******************************************************************
	// Function called at the instantiation of the class
	// *******************************************************************
	void Start () {

		//To avoid loosing informations (like the player's name), the class isn't destroyed on load of another scene
		DontDestroyOnLoad (this);

		blnMatchOver = false;

		//Sets the gravity that will be used for the game
		Physics.gravity = new Vector3(0f,-50f,0f);
	}
}
