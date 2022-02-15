using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FormerPlayerSpawnPosition : MonoBehaviour
{
    private void Start() {
        ClearVisuals();
        PlayerMain.instance.OnNewLevelLoaded(transform);
    }

    void ClearVisuals() {
        GetVisuals();
        for (var i = visuals.Count - 1; i >= 0; i--) {
            visuals[i].gameObject.SetActive(false);
        }
    }
    
    List<GameObject> visuals = new List<GameObject>();
    //Get all the transforms of the children of this object
    void GetVisuals() {
        visuals.Clear();
        foreach (Transform child in transform) {
            visuals.Add(child.gameObject);
        }
    }

}
