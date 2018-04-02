using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoUI : MonoBehaviour {

    public Text nameSpace;
    public Text unitPresent;

    public void displayInformation(Tile t) {
        nameSpace.text = t.name;
        if (t.currentUnit != null) {
            //unitPresent.text = t.currentUnit.name;
        } 
    }
}
