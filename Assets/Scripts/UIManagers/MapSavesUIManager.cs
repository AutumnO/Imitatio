using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSavesUIManager : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    public void OnClickBack()
    {
        StartCoroutine(_sceneLoader.LoadSceneAsync(0));
    }
    public void OnClickNewMap()
    {
        StartCoroutine(_sceneLoader.LoadSceneAsync(2));
    }
    public void OnClickArchives()
    {
        Debug.Log("Archives");
    }
    public void OnClickChangeName()
    {
        Debug.Log("ChangeName");
    }
    public void OnClickOpenScroll(int scrollNumber)
    {
        Debug.Log("Open Scroll " + scrollNumber);
    }
}
