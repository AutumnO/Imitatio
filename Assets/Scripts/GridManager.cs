using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // width and height of visible grid
    [SerializeField] private int _width, _height;

    // empty tile prefab
    [SerializeField] private Tile _emptyTilePrefab;

    // the game camera
    [SerializeField] private Transform _camera;

    // the dictionary containing all visible tiles
    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // create dictionary to hold spawned tiles
        _tiles = new Dictionary<Vector2, Tile>();

        // create "empty tile" objects
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var tileSpawn = Instantiate(_emptyTilePrefab, new Vector3(x, y), Quaternion.identity);
                tileSpawn.name = $"Tile {x} {y}";

                _tiles[new Vector2(x, y)] = tileSpawn;
            }
        }

        //_camera.transform.position = new Vector3((float))
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out Tile returnedTile))
            return returnedTile;
        return null;
    }
}
