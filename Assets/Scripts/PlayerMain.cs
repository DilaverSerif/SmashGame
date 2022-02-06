using System;
using UnityEngine;
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
    
    public void ChangePlayerBody(int id) {
        Destroy(currentBody);
        currentBody = Instantiate(UnlockablePlayers[id].PlayerBodyPrefab, SpawnPos);
        currentBody.transform.localPosition = Vector3.zero;
        currentBody.transform.localRotation = Quaternion.identity;
        CurrentPlayerBodyID = id;
    }
    
    public void PlayAnimation(string animationName) {
        currentBody.GetComponent<Animator>().SetTrigger(animationName);
    }
}