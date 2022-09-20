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

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var tileSpawn = Instantiate(_emptyTilePrefab, new Vector3(x, y), Quaternion.identity);
                tileSpawn.name = $"Tile {x} {y}";
            }
        }

        //_camera.transform.position = new Vector3((float))
    }
}
