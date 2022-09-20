using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Sprite _unselected;
    [SerializeField] private Sprite _selected;

    void OnMouseEnter()
    {
        _renderer.sprite = _selected;
    }
    void OnMouseExit()
    {
        _renderer.sprite = _unselected;
    }
}
