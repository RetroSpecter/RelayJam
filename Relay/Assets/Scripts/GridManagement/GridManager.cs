using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GridManager manages the tile Grid itself
public class GridManager : MonoBehaviour {

    public static GridManager instance; //TODO: eventually make this a non-singleton
    [HideInInspector] public Tile[,] tileArray;

    public delegate bool tileValid(Tile a, Tile b);

    [Header("TileParameters")]
    public Vector2Int gridDim;

    /// <summary>
    /// Sets up grid, sorting and storing tile coordinates and tying onclick/onhover obsevers
    /// </summary>
    /// <param name="manager">the PlayerInputManager that will listen for tile clicked/hover</param>
    public void buildGrid(PlayerInputManager manager) {
        GridManager.instance = this;
        Tile[] tiles = GetComponentsInChildren<Tile>();
        Array.Sort(tiles);

        //puts tiles in a 2d array and ties events to it
        tileArray = new Tile[gridDim.x, gridDim.y];
        for (int i = 0; i < tiles.Length; i++) {
            tiles[i].hover += manager.tileHover;
            tiles[i].selected += manager.tileClicked;
            tiles[i].tileCoordinate = new Vector2Int(i % gridDim.x, i / gridDim.x);
            tiles[i].tileStart();

            tileArray[i % gridDim.x, i / gridDim.x] = tiles[i];
        }
    }

    /// <summary>
    /// Takes in a Tile t and a range and returns a list of all the tiles within that range
    /// </summary>
    /// <param name="t">tile we are starting from</param>
    /// <param name="range">range we are covering</param>
    /// <returns>returns a set of all the tiles within a given range with t as the center</returns>
    public HashSet<Tile> getTilesInRange(Tile t, int range) {
        return getTilesInRange(t,range, ((Tile a, Tile b) => true));
    }

    /// <summary>
    /// Takes in a Tile t and a range and returns a list of all the tiles within that range
    /// </summary>
    /// <param name="t">tile we are starting from</param>
    /// <param name="range">range we are covering</param>
    /// <returns>returns a set of all the tiles within a given range with t as the center</returns>
    public HashSet<Tile> getTilesInRangePassable(Tile t, int range) {
        tileValid tileValid;
        tileValid = sameUnitType;
        return getTilesInRange(t, range, tileValid);
    }

    /// <summary>
    /// Takes in a Tile t and a range and returns a list of all the tiles within that range
    /// </summary>
    /// <param name="t">tile we are starting from</param>
    /// <param name="range">range we are covering</param>
    /// <param name="tileValid">delegate that holds special check on tile condition </param>
    /// <returns>returns a set of all the tiles within a given range with t as the center</returns>
    private HashSet<Tile> getTilesInRange(Tile t, int range, tileValid tileValid) {
        HashSet<Tile> tiles = new HashSet<Tile>();
        getTilesInRange(t, t, tiles, range, tileValid);
        return tiles;
    }

    private void getTilesInRange(Tile source, Tile t, HashSet<Tile> tiles, int range, tileValid tileValid) {
        tiles.Add(t);
        List<Tile> neighbors = getTileNeighbors(t);
        foreach (Tile n in neighbors) {
            if (range > 1 && n.active) {
                if(tileValid != null && tileValid.Invoke(source, t))
                    getTilesInRange(source, n, tiles, range - 1, tileValid);
            }
        }
    }

    /// <summary>
    /// Compares two tiles, If either one is unoccupied returns true.
    /// If the units occupied are the same type, returns true
    /// </summary>
    /// <param name="t1">tile 1</param>
    /// <param name="t2">tile 2</param>
    /// <returns>if one of the tiles is unoccupied, 
    /// or if the units occupying them are the same type</returns>
    public bool sameUnitType(Tile t1, Tile t2) {
        if (!t1.occupied() || !t2.occupied())
            return true;

        return t1.currentUnit.GetType() == t2.currentUnit.GetType();
    }

    /// <summary>
    /// Gets the veritcal and horizontal neighbors of tile t
    /// </summary>
    /// <param name="t">the starting tile</param>
    /// <returns>a list of the veritval and horizontal neighbors of t</returns>
    public List<Tile> getTileNeighbors(Tile t) {
        List<Tile> neighbors = new List<Tile>();
        for (int i = -1; i <= 1; i += 2) {
            if (t.tileCoordinate.x + i < gridDim.x && t.tileCoordinate.x + i >= 0) {
                neighbors.Add(tileArray[t.tileCoordinate.x + i, t.tileCoordinate.y]);
            }

            if (t.tileCoordinate.y + i < gridDim.y && t.tileCoordinate.y + i >= 0) {
                neighbors.Add(tileArray[t.tileCoordinate.x, t.tileCoordinate.y + i]);
            }
        }
        return neighbors;
    }

    /// <summary>
    /// Resets all tiles to there original unhighlighted color state
    /// </summary>
    public void resetTiles() {
        for (int i = 0; i < gridDim.x; i++) {
            for (int j = 0; j < gridDim.y; j++) {
                tileArray[i, j].unHighlightTile();
            }
        }
    }

    /// <summary>
    /// Returns the cooridnate distance between tiles a and b
    /// </summary>
    /// <param name="a">tile we start from</param>
    /// <param name="b">tilewe go to</param>
    /// <returns>Returns the cooridnate distance between tiles a and b</returns>
    public float getTileDistances(Tile a, Tile b) {
        return Vector2.Distance(a.tileCoordinate, b.tileCoordinate);
    }
}
