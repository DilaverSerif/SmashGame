using System;
using UnityEngine;
using random = UnityEngine.Random;
public class PlayerMain : MonoBehaviour {
    public static PlayerMain instance;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }
    
    // public static Action<string> PlayBodyAnimation;

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

    public GameObject currentBody;
    private void Start() {
        
        if (CurrentPlayerBodyID >= UnlockablePlayers.Length) {
            CurrentPlayerBodyID = 0;
        }

        CurrentPlayerBodyID = 0;


        currentBody = Instantiate(CurrentPlayerBody.PlayerBodyPrefab, SpawnPos);
        currentBody.transform.localPosition = Vector3.zero;
        currentBody.transform.localRotation = Quaternion.identity;

        // PlayBodyAnimation += PlayAnimation;
        // MinorSaveSystem.instance.OnLevelLoaded += OnNewLevelLoaded;
    }

    public void OnNewLevelLoaded(Transform t) {
        // Transform t = FindObjectOfType<FormerPlayerSpawnPosition>().transform;
        transform.position = t.position;
        transform.rotation = t.rotation;
        transform.localScale = t.localScale;
    }

    public void UnlockNewBody(int id) {
        if (id >= UnlockablePlayers.Length) {
            id = 0;
        }
        if (id < 0) {
            id = UnlockablePlayers.Length - 1;
        }
        if (UnlockablePlayers[id].Unlocked) {
            CurrentPlayerBodyID = id;
            ChangePlayerBody(id);
            return;
        }
        //Get player money from store, check if the price of the new body is less than the player money
        //if it is, then unlock the new body and set the current body to the new body
        //if it is not, then do nothing
        int currentMoney = PlayerPrefs.GetInt("money", 0);
        if (currentMoney >= UnlockablePlayers[id].Price) {
            PlayerPrefs.SetInt("money", currentMoney - UnlockablePlayers[id].Price);
            UnlockablePlayers[id].Unlocked = true;
            CurrentPlayerBodyID = id;
            ChangePlayerBody(id);
        }
        else {
            Debug.Log("Not enough money");
        }
        StoreControl.instance.UpdateUI();
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

    private void OnDisable() {
        instance = null;
        // PlayBodyAnimation = null;
        // MinorSaveSystem.instance.OnLevelLoaded = null;
    }
}