using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelButton : MonoBehaviour {
	private Text text;
	private bool endTurn;
	// Use this for initialization
	void Awake(){
		text = GetComponentInChildren<Text>();
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		endTurn = PlayerInputManager.instance.selectedUnit != null;
	}
}
