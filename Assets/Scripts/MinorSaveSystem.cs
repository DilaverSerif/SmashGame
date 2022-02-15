using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
public class MinorSaveSystem : MonoBehaviour {
    public static MinorSaveSystem instance;
    public static Action<bool> EndLevelAction;
    public Action OnLevelEnded;
    public Action OnLevelEndedSuccess;
    public Action OnLevelEndedFail;
    public Action OnLevelLoaded;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
        EndLevelAction += EndLevel;
    }

    private void Start() {
        LoadLevel(LastOpenedLevel);
    }


    void OnDisable() {
        instance = null;
    }
    public int LastOpenedLevel {
        get => PlayerPrefs.GetInt("LastOpenedLevel", 1);
        set => PlayerPrefs.SetInt("LastOpenedLevel", value);
    }
    
    public bool GameEnded {
        get => PlayerPrefs.GetInt("GameEnded", 0) == 1;
        set => PlayerPrefs.SetInt("GameEnded", value ? 1 : 0);
    }
    
    //If game ended, return a random level int from scenes in the build settings, else return the last opened level
    public int GetNextLevel() {
        if (GameEnded) {
            return Random.Range(1, Application.levelCount);
        }
        return LastOpenedLevel + 1;
    }
    
    void OnLevelWasLoaded(int level) {
        if (level == 0) return;
        LastOpenedLevel = level;
    }
    
    public void EndLevel(bool success) {
        if (success) {
            OnLevelEndedSuccess?.Invoke();
        } else {
            OnLevelEndedFail?.Invoke();
        }
    }
    
    //Test function to load a level
    public void LoadLevel(int level) {
        

        if(level > 0 && level < Application.levelCount) {
            SceneManager.LoadScene(level);
            OnLevelLoaded?.Invoke();
        }
        else {
            GameEnded = true;
            SceneManager.LoadScene(GetNextLevel());
        }
        // OnLevelLoaded?.Invoke();
    }

    public  void LoadNextLevel() {

        LoadLevel(GetNextLevel());
    }
    
    
}