using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineExampleTemplate : MachineUnit {
	
	public void Awake() {
		base.Awake ();
		//Any Initializers Here
	}

	public override IEnumerator MachineEffect() {
		while (true) {
			//Action to Repeat while active here
			//ex. fire bullet while on
			yield return null;
		}
		//If the Machine does not have a continous action, make sure to ClearMachineStatus()
		//when Coroutine ends
		//ClearMachineStatus ();
	}

}
