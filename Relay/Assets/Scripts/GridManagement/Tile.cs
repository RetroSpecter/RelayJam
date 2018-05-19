using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a Tile represents a grid tile. Has coordinates, properties, and 
// is possibly occupied by a Unit
public class Tile : MonoBehaviour, IComparable<Tile> {

    public Vector2Int tileCoordinate;

    public delegate void tileAction(Tile t);
    public tileAction hover;
    public tileAction selected;

    [Header("Attributes")]
    public bool active;
    public Unit currentUnit;

    [Header("GUI")]
    public Color defaultColor;
    public Color selectedColor;
    public Color unmovableColor = Color.black;

    private void OnMouseEnter()  {
        if (hover != null){
            hover.Invoke(this);
        }
    }

    private void OnMouseDown() {
        if (selected != null){
            selected.Invoke(this);
        }
    }

    /// <summary>
    /// Sets tile's settings to match that of the settings in tileInfo
    /// </summary>
    /// <param name="tileInfo">tile information of this tile</param>
    public void setTileSettings(ColorToTile tileInfo) {
        defaultColor = tileInfo.defaultColor;
        selectedColor = tileInfo.selectedColor;
        if (tileInfo.startingUnit != null) {
            Unit u = Instantiate(tileInfo.startingUnit, transform.position, transform.rotation);
            currentUnit = u;
            u.transform.parent = this.transform;
        }
    }

    /// <summary>
    /// starts a tile, making it so it proprely registers 
    /// if it has a unit occupying it
    /// </summary>
    public void tileStart() {
        if(currentUnit != null) {
            currentUnit.move += unsetUnit;
            currentUnit.moveToTile(this, null);
        } 
    }

    /// <summary>
    /// highlights the tile Color
    /// </summary>
    /// <param name="color">the color we want to highlight the tile</param>
    public void highlightTile(Color color) {
        GetComponent<SpriteRenderer>().color = color;
    }

    /// <summary>
    /// sets the tile back to its original color
    /// </summary>
    public void unHighlightTile() {
            GetComponent<SpriteRenderer>().enabled = active;
            GetComponent<SpriteRenderer>().color = defaultColor;
    }

    /// <summary>
    /// sets this unit to occupy this tile
    /// </summary>
    /// <param name="u">unit being set</param>
    public void setUnit(Unit u) {
        currentUnit = u;
    }

    /// <summary>
    /// unset's this unit from this tile
    /// </summary>
    /// <param name="u">unit being unset</param>
    public void unsetUnit(Unit u) {
        u.move -= unsetUnit;
        currentUnit = null;
    }

    /// <summary>
    /// returns whether or not a unit is occupying this tile
    /// </summary>
    /// <returns></returns>
    public bool occupied() {
        return currentUnit != null;
    }

    /// <summary>
    /// Comparison function for tiles
    /// </summary>
    /// <param name="other">tile being compared to</param>
    /// <returns></returns>
    public int CompareTo(Tile other) {
        float myPos = -transform.position.x + transform.position.y * GridManager.instance.gridDim.x;
        float otherPos = -other.transform.position.x + other.transform.position.y * GridManager.instance.gridDim.x;
        return (int)Mathf.Sign(otherPos - myPos);
    }
}