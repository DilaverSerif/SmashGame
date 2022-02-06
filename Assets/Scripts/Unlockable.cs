using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Unlockable {
    public string UnlockableName;
    public bool IsUnlocked {
        get {
            return PlayerPrefs.GetInt(UnlockableName, 0) == 1;
        }
        set {
            PlayerPrefs.SetInt(UnlockableName, value ? 1 : 0);
        }
    }

    public int ID;
    
    public int cost;
    
    public GameObject PlayerBodyPrefab;
    
}


