using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreControl : MonoBehaviour {

    public static StoreControl instance;
    public static string playerMoneyID = "money";

    public Text moneyText;

    public int GetPlayerMoney {
        get => PlayerPrefs.GetInt(playerMoneyID);
        set => PlayerPrefs.SetInt(playerMoneyID, value);
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    private void OnDisable() {
        instance = null;
    }

    private void Start() {
        if (!PlayerPrefs.HasKey(playerMoneyID)) {
            PlayerPrefs.SetInt(playerMoneyID, 0);
        }

        moneyText ??= transform.Find("PlayerCointxt").GetComponent<Text>();
        UpdateMoneyText(GetPlayerMoney);

    }

    void OnApplicationQuit() {
        SavePlayerMoney();
    }

    public void AddPlayerMoney(int value) {
        GetPlayerMoney += value;
        UpdateMoneyText(GetPlayerMoney);
    }
    
    public void SubPlayerMoney(int value) {
        GetPlayerMoney -= value;
        UpdateMoneyText(GetPlayerMoney);
    }

    public void ResetPlayerMoney() {
        GetPlayerMoney = 0;
        UpdateMoneyText(GetPlayerMoney);
    }

    public void SavePlayerMoney() {
        PlayerPrefs.Save();
    }

    public void UpdateMoneyText(int value) {
        moneyText.text = "COIN:"+ value.ToString();
    }

    public void UpdateUI() {
        UpdateMoneyText(GetPlayerMoney);
    }
}