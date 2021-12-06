using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI loadingText;
    private static string sceneName;

    private AsyncOperation loadingAsync;

    public static void LoadScene(string _sceneName)
    {
        sceneName = _sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private void Awake()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }


        if (!SaveSystem.Instance.HaveSave) sceneName = "1";
        else sceneName = SaveSystem.Instance._SaveFile.level.ToString();


        loadingSlider = FindObjectOfType<Slider>();
    }

    private void Start()
    {
        if (loadingSlider != null) loadingSlider.value = 0;

        loadingAsync = SceneManager.LoadSceneAsync(sceneName);

        loadingAsync.allowSceneActivation = false;

        StartCoroutine("Loading");
    }


    private IEnumerator Loading()
    {
        loadingText.text = "Loading %0";

        while (loadingAsync.progress < 0.9f)
        {
            loadingText.text = "Loading %" + loadingAsync.progress * 100;
            if (loadingSlider != null) loadingSlider.DOValue(loadingAsync.progress * 100, 0.15F);
            yield return new WaitForEndOfFrame();
        }

        if (loadingSlider != null) loadingSlider.DOValue(100, 0.15F);
        loadingText.text = "Loading %100";

        yield return new WaitForSeconds(1.5f);
        loadingAsync.allowSceneActivation = true;
    }
}