using System;
using UnityEngine;
using random = UnityEngine.Random;
public class PlayerMain : MonoBehaviour {

    public static Action<string> PlayBodyAnimation;

    public Unlockable[] UnlockablePlayers;
    public int CurrentPlayerBodyID {
        get {
            return PlayerPrefs.GetInt("CurrentPlayerBodyID", 0);
        }
        set {
            PlayerPrefs.SetInt("CurrentPlayerBodyID", value);
        }
    }
    public Unlockable CurrentPlayerBody {
        get {
            return UnlockablePlayers[CurrentPlayerBodyID];
        }
    }

    public Transform SpawnPos;

    private GameObject currentBody;
    private void Start() {
        
        if (CurrentPlayerBodyID >= UnlockablePlayers.Length) {
            CurrentPlayerBodyID = 0;
        }

        CurrentPlayerBodyID = 0;
        
        PlayBodyAnimation += PlayAnimation;
        
        currentBody = Instantiate(CurrentPlayerBody.PlayerBodyPrefab, SpawnPos);
        currentBody.transform.localPosition = Vector3.zero;
        currentBody.transform.localRotation = Quaternion.identity;
    }
    
    public void UnlockNextBody(int id) {
        
        int currentMoney = PlayerPrefs.GetInt("money", 0);
        if (currentMoney >= UnlockablePlayers[id].Price) {
            PlayerPrefs.SetInt("money", currentMoney - UnlockablePlayers[id].Price);
            UnlockablePlayers[id].Unlocked = true;
            CurrentPlayerBodyID = id;
            Destroy(currentBody);
            currentBody = Instantiate(CurrentPlayerBody.PlayerBodyPrefab, SpawnPos);
            currentBody.transform.localPosition = Vector3.zero;
            currentBody.transform.localRotation = Quaternion.identity;
        }
    }
    
    public void ChangePlayerBody(int id) {
        if (id >= UnlockablePlayers.Length) {
            id = 0;
        }
        if (id < 0) {
            id = UnlockablePlayers.Length - 1;
        }
        if (UnlockablePlayers[id].Unlocked) {
            CurrentPlayerBodyID = id;
            Destroy(currentBody);
            currentBody = Instantiate(CurrentPlayerBody.PlayerBodyPrefab, SpawnPos);
            currentBody.transform.localPosition = Vector3.zero;
            currentBody.transform.localRotation = Quaternion.identity;
        }
    }
    
    public void PlayAnimation(string animationName) {
        currentBody.GetComponent<Animator>().SetTrigger(animationName);
    }
}