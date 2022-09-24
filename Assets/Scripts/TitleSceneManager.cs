using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingBarObject;
    
    private Slider _loadingBar;
    private TextMeshProUGUI _progressText;

    private void Awake()
    {
        _loadingBar = loadingBarObject.GetComponent<Slider>();
        _progressText = loadingBarObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingBarObject.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            _loadingBar.value = progress;
            _progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
    public void OnClickLoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
}
