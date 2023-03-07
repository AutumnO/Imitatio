using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CASCarosel : MonoBehaviour
{
    // Prefab for each item in the carosel
    [SerializeField] protected GameObject _caroselItem;

    [SerializeField] protected CharacterCreationUIManager _uiManager;

    private void Awake()
    {
        _uiManager = GameObject.Find("SceneManager").GetComponent<CharacterCreationUIManager>();
    }
}
