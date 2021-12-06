using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(5)]

public class GameBase : CanSave
{

    public bool HaveSpawn;
    
    [Header("For developers")]
    public bool Test;

    public bool CloseMuisc;
    public bool CloseEffectsSound;

    //Events
    public static UnityEvent SuccesefulFinishGame = new UnityEvent();
    public static UnityEvent FailGame = new UnityEvent();
    public static UnityEvent StartGame = new UnityEvent();

    [Header("Systems")] public ParticlePlaySystem ParticlePlaySystem;
    public MusicSystem MusicSystem;
    public SoundSystem SoundSystem;
    public ScoreSystem ScoreSystem;
    public MenuSystem MenuSystem;
    public ComboSystem ComboSystem;
    public SpawnObjectSystem SpawnObjectSystem;
    public SpawnerSystem SpawnerSystem;
    //public MenuSystem.WarningText WarningText;
    public TestBuild TestBuild;
    public LevelSystem LevelSystem;
    public CameraSystem CameraSystem;
    
    [Header("Spawner for Options")] public List<SpawnObjects> SpawnObjects = new List<SpawnObjects>();
    public int[] listSize;
    public List<Collider> spawnAreas = new List<Collider>();

    public static GameBase Dilaver;
    [Header("GENERALLY PARAMETRES")] 
    public int level = 1;
    public int countSpawn;

    [Header("COMBO SYSTEM")] public float MaxComboTime;
    public float AddTime;
    public float ComboTime;
    public bool Finish;
    
    private void Awake()
    {
        if (Dilaver == null)
        {
            Dilaver = this;
        }
        else Destroy(gameObject);

        SuccesefulFinishGame.AddListener(()=> Finish = true);
        FailGame.AddListener(()=> Finish = true);

        if (FindObjectOfType<SaveSystem>() == null)
        {
            Debug.LogWarning("SAVE SISTEM NULL");
        }
    
        level = 1;
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);

        ScoreSystem = gameObject.AddComponent<ScoreSystem>();
        MusicSystem = gameObject.AddComponent<MusicSystem>();
        SoundSystem = gameObject.AddComponent<SoundSystem>();
        ParticlePlaySystem = gameObject.AddComponent<ParticlePlaySystem>();
        MenuSystem = gameObject.AddComponent<MenuSystem>();
        ComboSystem = gameObject.AddComponent<ComboSystem>();
        SpawnObjectSystem = gameObject.AddComponent<SpawnObjectSystem>();
        if(HaveSpawn) SpawnerSystem = gameObject.AddComponent<SpawnerSystem>();
        //WarningText = gameObject.AddComponent<MenuSystem.WarningText>();
        LevelSystem = gameObject.AddComponent<LevelSystem>();
        CameraSystem = gameObject.AddComponent<CameraSystem>();

        if (Test) TestBuild = gameObject.AddComponent<TestBuild>();
    }
    
    public override void FileSave()
    {
        SaveSystem.Instance._SaveFile.level = level;
    }
    
    public override void SaveLoad()
    {
        if (SaveSystem.Instance.HaveSave)
        {
            var a = SaveSystem.Instance._SaveFile;
            level = a.level;
        }

        MenuSystem.LevelTextWriter.Invoke(level);
    }

    private void Start()
    {
        if (SaveSystem.Instance.HaveSave)
        {
            SaveSystem.LoadEvent.Invoke();
        }
    }
}

[System.Serializable]
public class SpawnObjects
{
    public List<GameObject> Objects = new List<GameObject>();
}

public class TestBuild : MonoBehaviour
{
    private void Start()
    {
        //Instantiate(GameBase.Dilaver.SpawnObjectSystem.GetObject(GameObjects.gameDebugConsole));

        MusicSystem.SetMusicOption.Invoke(GameBase.Dilaver.CloseMuisc);
    }

    float deltaTime = 0.0f;
    
    
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        ShowFPS();
        GUI.Label(new Rect(15, 20, 200, 40), "SCORE:" + GameBase.Dilaver.ScoreSystem.TotalScore);
    }

    private void ShowFPS()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}

public class SpawnerSystem : MonoBehaviour
//OBJE HAVUZLARINI, SPAWN NOKTALARINI VE SPAWNERLERI TUTAN VE YARATN SINIF
{
    public List<ObjectPool> Pools = new List<ObjectPool>();
    public List<Collider> SpawnAreas = new List<Collider>();
    public List<Spawner> Spawners = new List<Spawner>();

    private void Awake()
    {
        SpawnAreas = GameBase.Dilaver.spawnAreas; //SPAWN NOKTALARINI AL

        CreateNewPool();
    }

    public Spawner NewSpawnerCreate()
    {
        var newSpawner = gameObject.AddComponent<Spawner>();
        Spawners.Add(newSpawner);
        return newSpawner;
    }

    private void CreateNewPool()
    {
        for (var index = 0; index < GameBase.Dilaver.SpawnObjects.Count; index++)
        {
            var newPool = new ObjectPool();
            newPool.SetSize(GameBase.Dilaver.listSize[index]).SetObject(GameBase.Dilaver.SpawnObjects[index].Objects)
                .CreatePool();
            Pools.Add(newPool);
        }
    }

    public class Spawner : MonoBehaviour //DÜSMAN SPAWNLAMAK ICIN
    {
        public bool Finish
        {
            get { return finish; }
        }

        private bool finish;
        private int limit = 9999999;
        private Collider area;
        private float delay = 2f;
        private int seletecedPool = -1;
        private int min = 1, max = 1;
        private List<GameObject> spawns;
        public int spawnCount;

        public Spawner SetRange(int min, int max)
        {
            this.min = min;
            this.max = max;
            return this;
        }

        public Spawner SetDelay(float _delay)
        {
            delay = _delay;
            return this;
        }

        public Spawner SetArea(Collider _area)
        {
            area = _area;
            return this;
        }

        public Spawner SetSpawns(List<GameObject> _spawns)
        {
            spawns = _spawns;
            return this;
        }

        public Spawner SetPool(int pool)
        {
            seletecedPool = pool;
            return this;
        }

        public Spawner SetLimit(int _limit)
        {
            limit = _limit;
            return this;
        }

        public void StartSpawn()
        {
            finish = false;
            StartCoroutine("SpawnObject");
        }

        public void HardStopSpawn()
        {
            StopCoroutine("SpawnObject");
        }

        public Spawner AddLimit(int value)
        {
            limit = value;
            return this;
        }

        public IEnumerator SpawnObject()
        {
            int checkSpawn = 0;

            if (seletecedPool == -1)
            {
                Debug.LogError("YOU MUST ENTER A POOL");
                yield break;
            }

            while (limit > checkSpawn)
            {
                for (int i = 0; i < Random.Range(min, max); i++) //RANDOM AYNI ANDA DÜŞMAN ÇIKARTMA
                {
                    GameBase.Dilaver.SpawnerSystem.Pools[seletecedPool]
                        .GetPoolObstacle().SetPosition(area);
                    spawnCount++; //sistem kasmasin diye 100 den fazla enemy basmamasi için
                    checkSpawn++; //yeterince spawn olunca spawneri durdur.
                }

                while (spawnCount > 100)
                {
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(delay);
            }

            finish = true;
        }
    }

    public class ObjectPool
    {
        private GameObject selectObject;
        public List<GameObject> obstacle = new List<GameObject>();
        private List<GameObject> obstacleList = new List<GameObject>();
        private int poolSize;

        public void AddObject(GameObject _object)
        {
            obstacleList.Add(_object);
            _object.SetActive(false);
        }

        public ObjectPool SetSize(int _size)
        {
            poolSize = _size;
            return this;
        }

        public ObjectPool SetObject(List<GameObject> _gameObject)
        {
            obstacle = _gameObject;
            return this;
        }

        public void CreatePool(bool value = false)
        {
            for (int i = 0; i < poolSize; i++)
            {
                var spawn = Instantiate(obstacle[Random.Range(0, obstacle.Count)]);
                spawn.SetActive(value);
                obstacleList.Add(spawn);
            }
        }

        public ObjectPool SetRotation(Quaternion rot)
        {
            selectObject.transform.rotation = rot;
            return this;
        }

        public ObjectPool SetPosition(Collider area)
        {
            selectObject.transform.position = General.RandomPointInArea(area);
            return this;
        }


        public ObjectPool GetPoolObstacle()
        {
            if (obstacleList.Count > 0)
            {
                selectObject = obstacleList[0];
                selectObject.SetActive(true);
                obstacleList.Remove(selectObject);
                return this;
            }
            else
            {
                poolSize = 1;
                CreatePool();
                return GetPoolObstacle();
            }
        }
    }
}


public class ScoreSystem : CanSave
{
    private int totalScore;
    private int episodeScore;
    public static AddScoreClass AddScoreEvent = new AddScoreClass();
    public class AddScoreClass : UnityEvent<int> { }
    private void Start()
    {
        SaveLoad();
        SaveSystem.SaveEvent.AddListener(FileSave);

        if (PlayerPrefs.HasKey("prize"))
        {
            AddScore(PlayerPrefs.GetInt("prize"));
        }
    }

    public int TotalScore
    {
        get { return totalScore; }
    }
    
    public int EpisodeScore
    {
        get
        {
            return episodeScore;
        }
    }

    public int FullScore
    {
        get
        {
            return episodeScore + totalScore;
        }
    }

    private void AddScore(int score)
    {
        if (GameBase.Dilaver.ComboSystem.TotalCombo > 0 & score > 0)
        {
            score += GameBase.Dilaver.ComboSystem.TotalCombo;
        }

        episodeScore += score;

        if (episodeScore < 0)
        {
            episodeScore = 0;
        }

        MenuSystem.ScoreTextWriter.Invoke(episodeScore);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        AddScoreEvent.AddListener(AddScore);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        AddScoreEvent.RemoveListener(AddScore);
    }

    public override void FileSave()
    {
        var a = SaveSystem.Instance._SaveFile;
        a.score = totalScore + episodeScore;
    }

    public override void SaveLoad()
    {
        if (SaveSystem.Instance.HaveSave)
        {
            var a = SaveSystem.Instance._SaveFile;
            totalScore = a.score;
            MenuSystem.ScoreTextWriter.Invoke(totalScore);
        }
    }
}


public class AddScore : UnityEvent<int>
{
}

public class ShowScore : UnityEvent<int>
{
}

public class LoadScene : UnityEvent<string>
{
}

public class MenuSystem : MonoBehaviour
{
    public MenuSet mainMenu
    {
        get
        {
            if (mainMenu == null)
                mainMenu = FindObjectOfType<MenuSet>();
            return mainMenu;
        }

        set => mainMenu = value;
    }

    public static UnityEvent OpenDead = new UnityEvent(); //ÖLCEĞİ ZAMAN BU DİNLET
    public static UnityEvent CloseDead = new UnityEvent(); //TEKRAR HAYATA DÖNDÜ BUNA DİNLET

    public static ScoreTextWriter ScoreTextWriter = new ScoreTextWriter();
    public static ComboTextWriter ComboTextWriter = new ComboTextWriter();
    public static LevelTextWriter LevelTextWriter = new LevelTextWriter();

    public static UnityEvent CloseWarning = new UnityEvent();
    public static OpenWarning OpenWarning = new OpenWarning();


    public class WarningText : MonoBehaviour
    {
        private TextMeshProUGUI warningText
        {
            get
            {
                if (_warningText == null)
                {
                    _warningText = FindObjectOfType<UIWarningText>().TextMeshProUGUI();
                }

                return _warningText;
            }
        }

        private TextMeshProUGUI _warningText;

        private float showTime = 1, openTime = 0.25F, dieTime = 0.55f;
        private Color color = Color.white;

        public void ShowText(string _text)
        {
            warningText.color = General.TextFade(color);

            warningText.text = _text;

            DOTween.Kill("warningText");

            warningText.DOFade(1, openTime).SetId("warningText").SetUpdate(true).OnComplete(
                () => warningText.DOFade(0, dieTime).SetUpdate(true).SetDelay(showTime).SetId("warningText")
            );
        }

        public WarningText SetColor(Color _color)
        {
            color = _color;
            return this;
        }

        public WarningText SetOpen(float _time)
        {
            openTime = _time;
            return this;
        }

        public WarningText SetDie(float _time)
        {
            dieTime = _time;
            return this;
        }

        public WarningText SetTime(float _time)
        {
            showTime = _time;
            return this;
        }
    }
}

public class ScoreTextWriter : UnityEvent<int>
{
}

public class ComboTextWriter : UnityEvent<int>
{
}

public class LevelTextWriter : UnityEvent<int>
{
}

public class OpenWarning : UnityEvent<string>
{
}

public class OpenWarningText : UnityEvent<string, float>
{
}

public class MusicSystem : MonoBehaviour
{
    private AudioSource musicSource;
    public static MusicOption SetMusicOption = new MusicOption();
    private List<AudioClip> musics = new List<AudioClip>();
    private float time = 1f;
    private float lowerValue = 0.5f;

    public class MusicOption : UnityEvent<bool> { }
    
    private void Awake()
    {
        SetMusicOption.AddListener(OpenCloseMusic);

        Load();
        musicSource = gameObject.AddComponent<AudioSource>();

        if(musics.Count == 0) return;
        musicSource.loop = true;
        musicSource.volume = 0.75f;
        
         musicSource.clip = musics[0];
        musicSource.Play();
    }

    private void OnEnable()
    {
        GameBase.SuccesefulFinishGame.AddListener(()=> LowerAnim(0));
        GameBase.FailGame.AddListener(()=>LowerAnim(0));
    }

    private void OnDisable()
    {
        GameBase.SuccesefulFinishGame.RemoveListener(()=> LowerAnim(0));
        GameBase.FailGame.RemoveListener(()=>LowerAnim(0));
    }

    public void OpenCloseMusic(bool value)
    {
        musicSource.mute = value;
    }

    public MusicSystem SetLowerTime(float _time)
    {
        time = _time;
        return this;
    }

    public void LowerAnim(float _lower,float time = 0.25f)
    {
        musicSource.DOFade(_lower, time);
    }

    public MusicSystem SetLower(float _lower)
    {
        lowerValue = _lower;
        return this;
    }

    private void Load()
    {
        foreach (AudioClip g in Resources.LoadAll("Musics", typeof(AudioClip)))
        {
            musics.Add(g);
        }
    }

    public void LowerMusic()
    {
        StartCoroutine(_LowerMusic());
    }

    private IEnumerator _LowerMusic()
    {
        musicSource.volume = lowerValue;
        yield return new WaitForSeconds(time);
        musicSource.volume = 0.75f;
    }
}

public class SoundSystem : MonoBehaviour
{
    private List<AudioClip> audioClips = new List<AudioClip>();
    private AudioSource source;
    private MusicSystem musicSystem;
    private float volume = 1f;
    private bool lowerMusic;
    public static SoundOption SetSoundOption = new SoundOption();
    public class SoundOption : UnityEvent<bool> { }
    private void Awake()
    {
        SetSoundOption.AddListener(OpenCloseEffect);
        Load();
        source = gameObject.AddComponent<AudioSource>();
        musicSystem = GetComponent<MusicSystem>();
    }

    private void OnEnable()
    {
        GameBase.SuccesefulFinishGame.AddListener(()=> PlaySound(Sounds.Win));
        GameBase.FailGame.AddListener(()=> PlaySound(Sounds.Fail));
    }

    private void OnDisable()
    {
        GameBase.SuccesefulFinishGame.RemoveListener(()=> PlaySound(Sounds.Win));
        GameBase.FailGame.RemoveListener(()=> PlaySound(Sounds.Fail));
    }

    private void OpenCloseEffect(bool value)
    {
        source.mute = value;
    }

    private void Load()
    {
        foreach (AudioClip g in Resources.LoadAll("Sounds", typeof(AudioClip)))
        {
            audioClips.Add(g);
        }
    }

    public SoundSystem SetLowerMusic(bool value)
    {
        lowerMusic = value;
        return this;
    }

    public SoundSystem SetPitch(float value = 20)
    {
        source.pitch *= 1 + Random.Range(-value / 100, value / 100);
        return this;
    }

    public void PlaySound(Sounds sound)
    {
        if (lowerMusic)
        {
            musicSystem.LowerMusic();
        }

        foreach (var ss in audioClips)
        {
            if (ss.name.ToLower() == sound.ToString().ToLower())
            {
                source.PlayOneShot(ss, volume);
            }
        }

        source.pitch = 1;
        lowerMusic = false;
    }

    public SoundSystem OverrideVolume(float vol)
    {
        volume = vol;
        return this;
    }
}

public class SpawnObjectSystem : MonoBehaviour
{
    private List<GameObject> gameObjects = new List<GameObject>();

    private void Awake()
    {
        Load();
    }

    private void Load()
    {
        foreach (GameObject g in Resources.LoadAll("GameObjects", typeof(GameObject)))
        {
            gameObjects.Add(g);
        }
    }

    public GameObject GetObject(GameObjects obje, int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            foreach (var g in gameObjects)
            {
                if (obje.ToString().ToLower().Equals(g.name.ToLower()))
                {
                    return g;
                }
            }
        }

        Debug.LogError("NOT FOUND GAMEOBJECTS");
        return null;
    }
}

public class ComboSystem : MonoBehaviour
{
    private int totalCombo;
    private bool spawnText = true;
    private GameObject comboText;

    private float time;

    public int TotalCombo
    {
        get { return totalCombo; }
    }

    public ComboSystem SpawnText(Vector3 pos) //DİKKAT BU FONKSIYON ADDCOMBODAN SONRA CALISTIRLMALI
    {
        comboText = Instantiate(GameBase.Dilaver.SpawnObjectSystem.GetObject(GameObjects.ComboText));
        comboText.transform.position = pos;
        return this;
    }

    public ComboSystem AddCombo(int value)
    {
        if (value == 0) return this;
        totalCombo += value;

        if (spawnText)
        {
            StartComboCounter();
        }

        MenuSystem.ComboTextWriter.Invoke(totalCombo);
        return this;
    }

    private IEnumerator ComboCounter()
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        totalCombo = 0;
        MenuSystem.ComboTextWriter.Invoke(totalCombo);
        if (time < 0)
        {
            time = 0;
        }
    }

    private void HardResetCombo()
    {
        StopCoroutine("ComboCounter");
        totalCombo = 0;
    }

    private void StartComboCounter()
    {
        if (time == 0)
        {
            time += GameBase.Dilaver.ComboTime;
            StartCoroutine("ComboCounter");
        }
        else
        {
            time += GameBase.Dilaver.AddTime;

            if (time > GameBase.Dilaver.MaxComboTime)
            {
                time = GameBase.Dilaver.MaxComboTime;
            }
        }
    }

    public ComboSystem ChanceColor(Color color)
    {
        if (comboText == null)
        {
            Debug.LogWarning("ERROR! FIRST USE FUNC ADDCOMBO");
        }
        // else comboText.GetComponent<ComboText>().textMesh.color = color;

        return this;
    }

    public ComboSystem FailCombo()
    {
        totalCombo = 0;
        return this;
    }

    public ComboSystem SpawnText(bool value)
    {
        spawnText = value;
        return this;
    }
}


public class ParticlePlaySystem : MonoBehaviour
{
    private int amount;
    private Particles particle;
    private Transform parent = null;
    private Vector3 scale;
    private Vector3 position, offSet;
    private bool loop;
    private bool destory = true;
    private float time;

    private GameObject spawnParticle;
    private List<GameObject> particles = new List<GameObject>();


    private void Awake()
    {
        Load();
    }

    private void Load()
    {
        foreach (GameObject g in Resources.LoadAll("Particles", typeof(GameObject)))
        {
            particles.Add(g);
        }
    }

    public ParticlePlaySystem SetParent(Transform parent)
    {
        this.parent = parent;
        return this;
    }

    public void PlayParticle(Particles particle, Vector3 pos, [Optional] Quaternion rot)
    {
        foreach (var pp in particles)
        {
            if (pp.name.ToLower() == particle.ToString().ToLower())
            {
                if (rot == Quaternion.Euler(0, 0, 0)) rot = Quaternion.identity;
                spawnParticle = Instantiate(pp, pos + offSet, rot);

                if (scale != Vector3.zero)
                {
                    spawnParticle.transform.localScale = scale;
                }

                if (destory)
                {
                    spawnParticle.gameObject.AddComponent<ParticleDestory>().lifeTime = time;
                }

                if (parent != null)
                {
                    spawnParticle.transform.SetParent(parent);
                    spawnParticle.transform.position = pos;
                }
            }
        }

        if (spawnParticle == null) Debug.LogWarning("NO FINDING PARTICLE");
    }

    public ParticlePlaySystem ChangeScale(Vector3 scale)
    {
        if (spawnParticle == null)
        {
            Debug.LogError("PARTICLE IS NULL FIRST YOU MUST USE FUNC PLAYPARTICLE!");
        }
        else spawnParticle.transform.localScale = scale;

        return this;
    }

    public ParticlePlaySystem SetDestoryDelay(float value)
    {
        time = value;
        return this;
    }

    public ParticlePlaySystem SetDestory(bool value)
    {
        destory = value;
        return this;
    }

    public ParticlePlaySystem SetScale(Vector3 value)
    {
        scale = value;
        return this;
    }

    public ParticlePlaySystem OffSet(Vector3 pos)
    {
        offSet = pos;
        return this;
    }
}

public class AdsSystem : MonoBehaviour
{
    public static UnityEvent ShowRewardEvent = new UnityEvent();
    public static UnityEvent ShowFScreenAdsEvent = new UnityEvent();
    public static UnityEvent BannerEvent = new UnityEvent();

    public static UnityEvent AdsResumeGame = new UnityEvent();
    public static UnityEvent AdsPrize = new UnityEvent();


    private int time = 0;

    private IEnumerator Start()
    {
        while (true)
        {
            time += 1;
            yield return new WaitForSeconds(1);

            if (time % 60 == 0)
            {
                ShowFScreenAdsEvent.Invoke();
            }
        }
    }
}
public class GenericUnityEvent<T>:UnityEvent<T>{}

public class LevelSystem: MonoBehaviour
{
    public static UnityEvent LevelChange = new UnityEvent();
    public static UnityEvent RestartLevel = new UnityEvent();
    private void OnEnable()
    {
         LevelChange.AddListener(LevelUp);
         RestartLevel.AddListener(Again);
    }

    private void OnDisable()
    {
        LevelChange.RemoveListener(LevelUp);
        RestartLevel.RemoveListener(Again);
    }

    private void Again()
    {
        LoadingScreen.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private void LevelUp()
    {
        GameBase.Dilaver.level += 1;
        SaveSystem.Instance.SaveNow();
        LoadingScreen.LoadScene(GameBase.Dilaver.level.ToString());
    }
}

public class CameraSystem: MonoBehaviour
{
    private CinemachineStateDrivenCamera mainCamera;

    private void Awake()
    {
        mainCamera = FindObjectOfType<CinemachineStateDrivenCamera>();
    }

    private void DontFollow()
    {
        mainCamera.m_Follow = null;
        mainCamera.LookAt = null;
    }
    
    private void OnEnable()
    {
        GameBase.FailGame.AddListener(DontFollow);
    }

    private void OnDisable()
    {
        GameBase.FailGame.RemoveListener(DontFollow);
    }
}

public class General
{
    public static Vector3 RandomPointInArea(Collider collider)
    {
        var bounds = collider.bounds;

        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    public static Color TextFade(Color a)
    {
        return new Color(a.r, a.g, a.b, 0);
    }
}


public enum Particles
{
    StarExplosion,
    DollarbillBlast,
    Crash,
    DustDirtyPoof,
    EmojiCool,
    EmojiCry
}

public enum Sounds //Ses dosyasiyla enum ismi ayni olmak zorunda (kücük büyük harf gereksiz)
{
    loot,
    GateLoot,
    FailGateLoot,
    MetalCrash,
    PizzaThrow,
    Win,
    Fail,
    Taxi,
    Motor,
    Smash
}

public enum GameObjects
{
    Money,
    ComboText
}


