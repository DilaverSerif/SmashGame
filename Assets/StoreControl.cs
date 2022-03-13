using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreControl : MonoBehaviour {

    public static StoreControl instance;
    public static string playerMoneyID = "money";

    public Text moneyText;

    [SerializeField] private Transform ButtonContainer;

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
        // UpdateMoneyText(GetPlayerMoney);
        UpdateUI();
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
        UpdateButtons();
    }
    
    void UpdateButtons() {
        foreach (var VARIABLE in PlayerMain.instance.UnlockablePlayers) {
            TextMeshProUGUI text = ButtonContainer.GetChild(VARIABLE.ID).GetComponentInChildren<TextMeshProUGUI>();
            bool unlocked = VARIABLE.Unlocked || VARIABLE.Price <= 0;
            text.text = unlocked ? "Unlocked" : VARIABLE.Price.ToString();
        }
        
        foreach (var VARIABLE in PlayerMain.instance.ballUnlockables) {
            TextMeshProUGUI text = ButtonContainer.GetChild(VARIABLE.ID + PlayerMain.instance.UnlockablePlayers.Length).GetComponentInChildren<TextMeshProUGUI>();
            bool unlocked = VARIABLE.Unlocked || VARIABLE.Price <= 0;
            text.text = unlocked ? "Unlocked" : VARIABLE.Price.ToString();
        }
    }
}