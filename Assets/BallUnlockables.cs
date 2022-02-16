using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BallUnlockables 
{

    public string unlockableName;
    public bool Unlocked {
        get => PlayerPrefs.GetInt(unlockableName, 0) == 1;
        set => PlayerPrefs.SetInt(unlockableName, value ? 1 : 0);
    }

    public int ID;
    public int Price;
    public GameObject BallPrefab;

}
