using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSet : MonoBehaviour
{
    private NextLevelMeu nextLevelMeu;
    private FailMenu failMenu;

    private void Awake()
    {
        nextLevelMeu = FindObjectOfType<NextLevelMeu>();
        failMenu = FindObjectOfType<FailMenu>();
    }

    private void OnEnable()
    {
        GameBase.SuccesefulFinishGame.AddListener(Play);
        GameBase.FailGame.AddListener(GameOver);
    }

    private void OnDisable()
    {
        GameBase.SuccesefulFinishGame.RemoveListener(Play);
        GameBase.FailGame.RemoveListener(GameOver);
    }

    private void Play()
    {
        nextLevelMeu.gameObject.SetActive(true);
        failMenu.gameObject.SetActive(false);
    }

    private void GameOver()
    {
        failMenu.gameObject.SetActive(true);
        nextLevelMeu.gameObject.SetActive(false);
    }
}