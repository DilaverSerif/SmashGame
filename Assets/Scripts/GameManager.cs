using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Text text;
    private Text CoinText;

    public Action VaseCountdown;
    private int startCount;
    private bool canCount = true;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    [SerializeField] private int sure;
    private IEnumerator Start() {
        ReklamScript.BannerGoster();

        text = GameObject.FindGameObjectWithTag("Counter").GetComponent<Text>();
        startCount = FindObjectsOfType<CanBrekable>().Length - 1;
        VaseCountdown += vaseCountDown;
        text.text = "3";
        yield return new WaitForSeconds(1f);
        text.text = "2";
        yield return new WaitForSeconds(1f);
        text.text = "1";
        yield return new WaitForSeconds(1f);
        text.text = "GO!";
        yield return new WaitForSeconds(0.55f);
        text.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -300, 0);
        GameBase.StartGame.Invoke();

        canCount = true;
        StartCoroutine("Countdown");
        // while (sure != 0)
        // {
        //     sure -= 1;
        //     text.text = sure.ToString();
        //     yield return new WaitForSeconds(1f);
        // }
        //
        // GameBase.FailGame.Invoke();
    }

    IEnumerator Countdown() {
        
        while (sure != 0)
        {
            sure -= 1;
            text.text = sure.ToString();
            yield return new WaitForSeconds(1f);
            // await Task.Delay(1000);
            if(!canCount) yield break;
        }
        GameBase.FailGame.Invoke();
    }

    async void vaseCountDown() {
        // startCount--;
        // Debug.Log(startCount);
        // if (startCount <= 0) {
        //     canCount = false;
        //     await Task.Delay(1000);
        //     GameBase.SuccesefulFinishGame.Invoke();
        // }
    }

    private void OnEnable()
    {
        GameBase.SuccesefulFinishGame.AddListener(Win);
    }

    private void OnDisable()
    {
        GameBase.SuccesefulFinishGame.RemoveListener(Win);
        VaseCountdown = null;
    }

    private void Win()
    {
        StopCoroutine("Countdown");
    }
}
