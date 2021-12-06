using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DragAndShoot : MonoBehaviour {

    private Vector3 mousePressDown;
    private Vector3 mouseRelease;

    public bool Reverse;
    public float ForceMulti;

    private Rigidbody rb;

    private bool shot = false;


    private void Start() {
        rb = GetComponent<Rigidbody>();
    }


    private void OnMouseDown() {
        mousePressDown = Input.mousePosition;
    }

    private void OnMouseDrag() {
        Vector3 forceInit = ForceInit();
        Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y) * ForceMulti);
        if (!shot) {
            DrawTrajectory.instance.UpdateTrajectory(forceV, rb, transform.position);
        }
    }

    private void OnMouseUp() {
        DrawTrajectory.instance.HideLine();
        mouseRelease = Input.mousePosition;
        Shoot(Direction());
    }

    Vector3 Direction() => Reverse ? mousePressDown - mouseRelease : mouseRelease - mousePressDown;
    Vector3 ForceInit() => Reverse ? (mousePressDown - Input.mousePosition) : (Input.mousePosition - mousePressDown);

    async void Shoot(Vector3 force) {
        if (shot) 
            return;
        rb.AddForce(new Vector3(force.x, force.y, force.y)* this.ForceMulti);
        shot = true;
        
        //Silinebilir
        await Task.Delay(3000);
        if(!Application.isPlaying) return;
        rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
        shot = false;
        await Task.Delay(1000);
        rb.velocity = Vector3.zero;
    }

}
