using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI text;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    [SerializeField] private int sure;
    private IEnumerator Start()
    {
        text.text = "3";
        yield return new WaitForSeconds(1f);
        text.text = "2";
        yield return new WaitForSeconds(1f);
        text.text = "1";
        yield return new WaitForSeconds(1f);
        text.text = "GO!";
        yield return new WaitForSeconds(0.55f);
        text.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -100, 0);
        GameBase.StartGame.Invoke();

        while (sure != 0)
        {
            sure -= 1;
            text.text = sure.ToString();
            yield return new WaitForSeconds(1f);
        }
        
        GameBase.FailGame.Invoke();
    }

    private void OnEnable()
    {
        GameBase.SuccesefulFinishGame.AddListener(Win);
    }

    private void OnDisable()
    {
        GameBase.SuccesefulFinishGame.RemoveListener(Win);
    }

    private void Win()
    {
        StopCoroutine("Start");
    }
}
