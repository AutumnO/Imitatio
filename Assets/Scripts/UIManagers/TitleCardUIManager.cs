using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleCardUIManager : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    public void OnClickLoadScene(int sceneIndex)
    {
        StartCoroutine(_sceneLoader.LoadSceneAsync(sceneIndex));
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
}
