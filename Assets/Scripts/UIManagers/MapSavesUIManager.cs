using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MapSavesUIManager : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private PopUpManager _popUpManager;

    private GameObject _confirmationPopUp;
    private GameObject _nameChangePopUp;

    public void GetInput()
    {
        Debug.Log("suck");
    }
    public void OnClickBack()
    {
        StartCoroutine(_sceneLoader.LoadSceneAsync(0));
    }
    public void OnClickNewMap()
    {
        _nameChangePopUp = _popUpManager.ActivatePopUp("Input", "Enter the name of your new map...");
        Button button = _nameChangePopUp.GetComponentsInChildren<Button>()[1];
        button.onClick.AddListener(CreateNewMap);
    }
    public void CreateNewMap()
    {
        TMP_InputField field = _nameChangePopUp.GetComponentInChildren<TMP_InputField>();
        if (field.text.Length > 0)
        {
            Debug.Log(field.text);
            _popUpManager.ClosePopUp();
            StartCoroutine(_sceneLoader.LoadSceneAsync(2));
        }
    }
    public void OnClickArchives()
    {
        Debug.Log("Archives");
    }
    public void OnClickChangeName()
    {
        _nameChangePopUp = _popUpManager.ActivatePopUp("Input", "Change the name of your map here...");
        Button button = _nameChangePopUp.GetComponentsInChildren<Button>()[1];
        button.onClick.AddListener(ChangeMapName);
    }
    public void ChangeMapName()
    {
        TMP_InputField field = _nameChangePopUp.GetComponentInChildren<TMP_InputField>();
        Debug.Log(field.text);
        _popUpManager.ClosePopUp();
    }
    public void OnClickOpenScroll(int scrollNumber)
    {
        Debug.Log("Open Scroll " + scrollNumber);
    }
}
