using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOptions : MonoBehaviour {

    [HideInInspector] public UnitManager gm;
    public GameObject MoveButton, DoneButton, CancelButton, EndTurnButton;

    public void showMenu(Tile t) {
        MoveButton.SetActive(!t.currentUnit.moved);
        DoneButton.SetActive(t.currentUnit.moved);
        transform.position = t.transform.position + Vector3.right * 1.25f;
    }

    public void hideMenu() {
        transform.position = Vector3.one * -1000;
    }

    void Update(){
        if (CancelButton){
            CancelButton.SetActive(PlayerInputManager.instance.selectedUnit);
        }
        if (EndTurnButton){
            EndTurnButton.SetActive(!PlayerInputManager.instance.selectedUnit);
        }
    }
}
