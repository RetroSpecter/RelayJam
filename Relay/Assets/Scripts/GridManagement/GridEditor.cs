using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridManager))]
// GridEditor builds a grid of tiles based off of a map texture
// and a list of color mappings. 
public class GridEditor : MonoBehaviour {

    public Tile tileObject;
    public Texture2D map;
    public ColorToTile[] colorMappings;

    // For a GridEditor ge
    // ge.tileObject holds the tile gameObject to be spawned
    // ge.map is the 2d texture use to construct the map
    // colorMappings is the list of pixel colors to mappings used to determine tile information in the map

    /// <summary>
    /// Takes in a 2d texutre and builds a tile map based on its dimesnions and the color
    /// of each pixel. Deletes any previous tile maps created.
    /// </summary>
    /// <param name="map">The texture we are construting the tilemap from</param>
    public void constructGrid(Texture2D map) {
        destroyAllChildren();

        Vector2Int gridDim = new Vector2Int(map.width, map.height);

        Vector3 offset = Vector3.zero;
        offset.x = gridDim.x / 4.0f;
        offset.y = -gridDim.y / 4.0f;
        GetComponent<GridManager>().gridDim = gridDim; //updates the dimensions of the gridManager

        for (int i = 0; i < gridDim.x; i++) {
            for (int j = 0; j < gridDim.y; j++) {
                Vector3 targetPosition = transform.position;
                targetPosition += Vector3.right * i;
                targetPosition -= Vector3.down * j;
                Color pixelColor = map.GetPixel(i, j);
                generateTile(targetPosition - offset, pixelColor);
            }
        }
    }

    /// <summary>
    /// Instantiates a tileat a given position, and sets up its propreties based on
    /// the pixel color.
    /// </summary>
    /// <param name="position">position to spawn tile in</param>
    /// <param name="pixelColor">color of pixel at that position.</param>
    public void generateTile(Vector3 position, Color pixelColor) {

        Tile tile = Instantiate(tileObject, position, Quaternion.identity);
        tile.transform.parent = this.transform;

        bool found = false;
        if (pixelColor.a == 0) {
            tile.active = false;
            found = true;
        } else {
            tile.active = true;
        }

        foreach (ColorToTile colorMapping in colorMappings) {
            if (colorMapping.color.Equals(pixelColor)) {
                tile.setTileSettings(colorMapping);
                found = true;
            }
        }

        if(!found)
            print("No mapping for color: RGB (" + (int)(255 * pixelColor.r) + "," + (int)(255 * pixelColor.g) + "," + (int)(255 * pixelColor.b) + ")");

        tile.unHighlightTile();
    }

    /// <summary>
    /// Destroys all children objects of this gameObject
    /// </summary>
    public void destroyAllChildren() {
        int i = transform.childCount - 1;
        while (i >= 0) {
            DestroyImmediate(transform.GetChild(i).gameObject);
            i--;
        }
    }
}

[System.Serializable]
// represets a pixel color and the tile information it corresponds to
public class ColorToTile {
    public Color color;
    [Header("Tile Info")]
    public Color defaultColor;
    public Color selectedColor;
    public Color unmovableColor = Color.black;
    public Unit startingUnit;
}