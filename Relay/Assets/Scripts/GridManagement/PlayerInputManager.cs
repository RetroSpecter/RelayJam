using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    public List<Unit> heroUnits;
    [HideInInspector] public HashSet<Unit> currentActiveUnits;
    private UnitManager unitManager;

    HashSet<Tile> inRangeTiles;

    private enum SelectionType { INACTIVE, ATTACK, MOVE };
    private SelectionType tileSelectionActive = SelectionType.MOVE;

    [Header("GUI Elements")]
    public GameObject cursor;
    public TileInfoUI tileInfoUI;
    public UnitInfoUI unitInfoUI;
    public CharacterOptions optionsMenu;

    public Tile selectedUnit;
    private Tile previousTile;

    public delegate void turnAction(bool yourTurn);
    public turnAction switchTurn;

    public static PlayerInputManager instance;

    void Awake() {
        instance = this;
        unitManager = GetComponent<UnitManager>();
        heroUnits = new List<Unit>(FindObjectsOfType<HeroUnit>());

        foreach (Unit u in heroUnits) {
            u.death += ((x) => heroUnits.Remove(x));
            u.death += unitManager.unitDeath;
        }

        currentActiveUnits = new HashSet<Unit>();
        optionsMenu.gm = unitManager;
        inRangeTiles = new HashSet<Tile>();
    }

    /// <summary>
    /// selects the unit on this tile. Sets the cursor inactive and
    /// moves the unit menu next to this t
    /// </summary>
    /// <param name="t">tile with unit we are selecting</param>
    public void selectUnit(Tile t) {
        if (currentActiveUnits.Contains(t.currentUnit))
        {
            tileSelectionActive = SelectionType.INACTIVE;
            cursor.SetActive(false);
            selectedUnit = t;
            optionsMenu.showMenu(t);
        } 
    }

    /// <summary>
    /// deselects a unit, undoing any movement done already
    /// and reactivates the cursor
    /// </summary>
    public void deselectUnit() {
        tileSelectionActive = SelectionType.MOVE;
        cursor.SetActive(true);

        // if we had moved the unit, we move it back to its original position
        if (previousTile != null) { 
            selectedUnit.currentUnit.moved = false;
            selectedUnit.currentUnit.moveToTile(previousTile, null);
        }

        selectedUnit = null;
        previousTile = null;
        optionsMenu.hideMenu();
    }

    /// <summary>
    /// Ends the turn of the currently selected unit
    /// </summary>
    public void endUnitTurn() {
        endUnitTurn(selectedUnit);
    }

    /// <summary>
    /// ends the turn of a selected unit and removes it from the 
    /// units the player can select. IF there are no more units that
    /// can be selected, ends the player phase
    /// </summary>
    /// <param name="selectedUnit"></param>
    public void endUnitTurn(Tile selectedUnit) {
        Unit u = selectedUnit.currentUnit;
        currentActiveUnits.Remove(u);

        previousTile = null;
        optionsMenu.hideMenu();
        inRangeTiles.Clear();
        unitManager.endUnitTurn(selectedUnit);
        deselectUnit();

        if (currentActiveUnits.Count == 0) {
            EndTurn();
        }
    }

    public void EndTurn(){
        tileSelectionActive = SelectionType.INACTIVE;
        cursor.SetActive(false);
        switchTurn.Invoke(false);
    }

    /// <summary>
    /// sets all Ally units to active and turns on player cursor
    /// </summary>
    public void setUnitsToActive() {
        tileSelectionActive = SelectionType.MOVE;
        cursor.SetActive(true);

        foreach (Unit us in heroUnits) {
            currentActiveUnits.Add(us);
            us.setActive(true);
            us.moved = false;
        }
    }

    /// <summary>
    /// tells tileInfo UI to display tile t information
    /// </summary>
    /// <param name="t">tile we want to display information about </param>
    public void tileHover(Tile t) {
        if (tileSelectionActive == SelectionType.INACTIVE) {
            return;
        }
        cursor.transform.position = t.transform.position;
        tileInfoUI.displayInformation(t);
        if (t.occupied()) {
            unitInfoUI.showStats(t.currentUnit);
        } else {
            unitInfoUI.hide();
        }
    }

    /// <summary>
    /// Actions to take when tile t is clicked on
    /// </summary>
    /// <param name="t">tile clicked on</param>
    public void tileClicked(Tile t) {
        if (tileSelectionActive == SelectionType.INACTIVE) {
            return;
        }

        if (selectedUnit == null && t.occupied())  {
            selectUnit(t);
        } else if (selectedUnit != null) {
            // if current unit is set to move
            if (tileSelectionActive == SelectionType.MOVE && inRangeTiles.Contains(t)) {
                if (unitManager.tileAvailable(selectedUnit, t)) {
                    Unit u = selectedUnit.currentUnit;
                    previousTile = selectedUnit;
                    unitManager.moveUnit(u, t, (() => selectUnit(t)));
                    tileSelectionActive = SelectionType.INACTIVE;
                }
            } else {
                if (inRangeTiles.Contains(t) && !unitManager.tileAvailable(selectedUnit, t)
                        && (t.currentUnit is EnemyUnit)) {
                    Unit u = selectedUnit.currentUnit;
                    unitManager.attackUnit(u, t.currentUnit, (() => {
                            selectUnit(t);
                            endUnitTurn();
                    }));
                    GridManager.instance.resetTiles();
                    tileSelectionActive = SelectionType.INACTIVE;
                }
            }
        }
    }

    /// <summary>
    /// Unhighlights all tiles, and resetts the menu
    /// </summary>
    public void unselectTiles() {
        if (selectedUnit == null) {
            throw new MissingReferenceException("No Unit Selected");
        }
        selectUnit(selectedUnit);
        foreach (Tile ts in inRangeTiles) {
            ts.GetComponent<SpriteRenderer>().color = ts.defaultColor;
        }
        inRangeTiles.Clear();
    }
    
    /// <summary>
    /// hides the character menu, sets the input manager to chooose a unit to attack
    /// and highlights areas this unit can attack
    /// </summary>
    public void setTilesCanAttack() {
        tileSelectionActive = SelectionType.ATTACK;
        cursor.SetActive(true);
        inRangeTiles = GridManager.instance.getTilesInRange(selectedUnit, selectedUnit.currentUnit.attackRange);
        inRangeTiles.Remove(selectedUnit);
        optionsMenu.hideMenu();
        foreach (Tile ts in inRangeTiles)
        {
            ts.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    /// <summary>
    /// hides the character menu, sets the input manager to chooose a unit to move
    /// and highlights areas this unit can move
    /// </summary>
    public void setTilesCanMove() {
        tileSelectionActive = SelectionType.MOVE;
        cursor.SetActive(true);
        inRangeTiles = GridManager.instance.getTilesInRangePassable(selectedUnit, selectedUnit.currentUnit.mobility);
        optionsMenu.hideMenu();
        foreach (Tile ts in inRangeTiles) {
            if (ts.occupied() && ts.currentUnit is EnemyUnit) {
                ts.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else {
                ts.GetComponent<SpriteRenderer>().color = ts.selectedColor;
            }
        }
    }
}
