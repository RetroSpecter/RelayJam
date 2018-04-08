using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMovement : MonoBehaviour {

	public float speed = 10;
	KeyCode[] keys = new KeyCode[]{KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D};
	Vector2[] directions = new Vector2[]{Vector2.up, Vector2.left, Vector2.down, Vector2.right};

	
	// Update is called once per frame
	void Update () {
		Vector2 move = new Vector2 ();
		for (int i = 0; i < keys.Length; i++) {
			if(Input.GetKey(keys[i])) {
				move += directions [i];
			}
		}
		transform.Translate (move.normalized * speed * Time.deltaTime);


	}
}
