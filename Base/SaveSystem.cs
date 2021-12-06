using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(1)]
public class SaveSystem : MonoBehaviour
{
    public static UnityEvent SaveEvent = new UnityEvent();
    public static UnityEvent LoadEvent = new UnityEvent();
    public SaveFile _SaveFile;
    
    private string path;

    private static SaveSystem _instance;

    public static SaveSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveSystem>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("SaveSystem");
                    _instance = container.AddComponent<SaveSystem>();
                }
            }

            return _instance;
        }
    }


    private void Awake()
    {
        path = Application.persistentDataPath + "\\save.json";
        
        if (File.Exists(path))
        {
            // JSON'u dosyadan oku
            string readJson = File.ReadAllText(path);
            // Okunan JSON'u objeye çevir
            _SaveFile = JsonUtility.FromJson<SaveFile>(readJson);

            LoadEvent.Invoke();
        }
        else
        {
            _SaveFile = new SaveFile();
        }
        
        DontDestroyOnLoad(gameObject);
    }
    
    public bool HaveSave
    {
        get
        {
            if (File.Exists(path))
            {
                return true;
            }
       
            return false;
        }
        
    }


    private void OnApplicationPause(bool pauseStatus) //OYUNU KAYDEDIYOR
    {
        if (pauseStatus)
        {
            Debug.Log("OYUN KAYDEDILDI" + path);
            SaveEvent.Invoke();
            string json = JsonUtility.ToJson(_SaveFile, true);
            File.WriteAllText(path, json);
        }
    }

    public void SaveNow()
    {
        Debug.Log("OYUN KAYDEDILDI" + path);
        SaveEvent.Invoke();
        string json = JsonUtility.ToJson(_SaveFile, true);
        File.WriteAllText(path, json);
    }

    // private void OnApplicationQuit()
    // {
    //     SaveEvent.Invoke();
    //     if (GameBase.Dilaver.level != 1)
    //     {
    //         string json = JsonUtility.ToJson(_saveFile, true);
    //         File.WriteAllText(path, json);
    //     }
    // }
}

public abstract class CanSave : MonoBehaviour, ISave
{
    public virtual void OnEnable()
    {
        SaveSystem.LoadEvent.AddListener(SaveLoad);
        SaveSystem.SaveEvent.AddListener(FileSave);
    }

    public virtual void OnDisable()
    {
        SaveSystem.LoadEvent.RemoveListener(SaveLoad);
        SaveSystem.SaveEvent.RemoveListener(FileSave);
    }

    public abstract void FileSave();
    public abstract void SaveLoad();
}

[Serializable]
public partial class SaveFile
{
    public int level = 1;
    public int score;
}