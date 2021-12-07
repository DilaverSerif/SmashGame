using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanBrekable : MonoBehaviour {

    private Outline outline;
    private void Start() {
        outline = GetComponent<Outline>();
        DOTween.To(() => outline.OutlineWidth, x => outline.OutlineWidth = x, 1, 2f).SetLoops(-1, LoopType.Yoyo);
        // float myFloat = 1;
        // DOTween.To(()=> myFloat, x=> myFloat = x, 52, 1);
    }
    private void OnTriggerEnter(Collider other)
    {
        var check = FindObjectOfType<DragAndShoot>();
        
        if (check != null)
        {
            GameBase.Dilaver.ParticlePlaySystem.SetScale(Vector3.one * 2).PlayParticle(Particles.smoke,transform.position);
            Finisher.Check.Invoke(transform);
            GameManager.Instance.VaseCountdown?.Invoke();
            Destroy(gameObject);
        }
    }
}
