﻿//*********************************************************
// Societe: ETML
// Auteur : Miguel Dias
// Date : 20.02.15
// But : Camera behaviour controller script
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

public class CameraController : MonoBehaviour {

	public Transform tranPlayer;

	private Transform tranCamera ;
	private int HeightDecal = 10;

	void Start () {
		tranCamera = this.transform;
	}

	// Update is called once per frame
	void Update () {
		if (tranPlayer) {
			transform.position = new Vector3(
				tranPlayer.position.x,
				tranPlayer.position.y + HeightDecal,
				tranCamera.position.z
			);	
		}

	}
}
