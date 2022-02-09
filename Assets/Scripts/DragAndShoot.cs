using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndShoot : MonoBehaviour {

    private Vector3 mousePressDown;
    private Vector3 mouseRelease;

    public bool Reverse;
    public float ForceMulti;

    private Rigidbody rb;

    private bool shot = false;
    private bool play;
    private Transform player;

    [Range(0.1f, 1f)]
    public float Sensitivty = 1;

    private Vector3 spawnPos;

    private void Start()
    {
        player = FindObjectOfType<PlayerMain>().transform;
        rb = GetComponent<Rigidbody>();
        spawnPos = transform.position;
    }
    
    
    

    private void Update() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        if (Input.GetMouseButtonDown(0)) {
            mousePressDown = Input.mousePosition;
        }
        if (Input.GetMouseButton(0)) {
            if (!play) return;
        
            Vector3 forceInit = ForceInit();
            Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y) * ForceMulti);
            if (!shot) {
                DrawTrajectory.instance.UpdateTrajectory(forceV, rb, transform.position);
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            DrawTrajectory.instance.HideLine();
            mouseRelease = Input.mousePosition;
            Shoot(Direction());
        }
    }

    Vector3 Direction() => Reverse ? (mousePressDown - mouseRelease) * Sensitivty : (mouseRelease - mousePressDown) * Sensitivty;
    Vector3 ForceInit() => Reverse ? (mousePressDown - Input.mousePosition) * Sensitivty : (Input.mousePosition - mousePressDown) * Sensitivty;

    async void Shoot(Vector3 force) {
        if (shot | !play) 
            return;
        rb.isKinematic = false;
        
        PlayerMain.PlayBodyAnimation?.Invoke("Kick");
        // FormerPlayerSpawnPosition.SutGol.Invoke();
        
        await Task.Delay(500);
        rb.AddForce(new Vector3(force.x, force.y, force.y)* this.ForceMulti);
        shot = true;
        CameraSystem.ChangeFocus(transform);
        //Silinebilir
        await Task.Delay(3000);
        CameraSystem.ChangeFocus(player);
        
        if(!Application.isPlaying) return;
        rb.velocity = Vector3.zero;
        transform.position = spawnPos;
        shot = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private void OnEnable()
    {
        GameBase.StartGame.AddListener(Play);
        GameBase.FailGame.AddListener(GameOver);
    }

    private void OnDisable()
    {
        GameBase.StartGame.RemoveListener(Play);
        GameBase.FailGame.RemoveListener(GameOver);
    }

    private void Play()
    {
        play = true;
    }

    private void GameOver()
    {
        play = false;
    }

}
