using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheelBehavior : MachineUnit {

	public float rotationSpeed = 5;
	public float rotationAmount = 45;
	GameObject field;
	Vector2 pivot;

	public void Awake() {
		base.Awake ();
		GridEditor ge = FindObjectOfType<GridEditor> ();
		field = ge.gameObject;
		pivot = ge.centerOfGrid;
	}

	public override IEnumerator MachineEffect() {
		ShooterAI[] cannons = FindObjectsOfType<ShooterAI> ();
		for (int i = 0; i < cannons.Length; i++) {
			cannons [i].gameObject.transform.Rotate (0, 0, 90);
		}
		/*
		float currentZ = field.transform.localEulerAngles.z;
		float targetZ = currentZ + rotationAmount;
		while (currentZ < targetZ) {
			field.transform.RotateAround (pivot, Vector3.forward, Time.deltaTime * rotationSpeed);
			currentZ = field.transform.localEulerAngles.z;
			if (currentZ > targetZ) {
			}
			yield return null;
		}*/
		yield return null;
		//For One Shot Actions
		ClearMachineStatus ();
	}

}
