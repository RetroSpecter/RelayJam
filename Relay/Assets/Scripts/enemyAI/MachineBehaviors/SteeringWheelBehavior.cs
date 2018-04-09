using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheelBehavior : MachineUnit {

	public float rotationSpeed = 90;
	GameObject field;

	private void Awake() {
		base.Awake ();
		field = FindObjectOfType<GridEditor> ().gameObject;
	}

	public override IEnumerator MachineEffect() {
		while (true) {
			field.transform.Rotate (0, 0, rotationSpeed * Time.deltaTime);
			yield return null;
		}
	}

}
