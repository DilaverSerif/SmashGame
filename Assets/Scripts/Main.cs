using System;
using UnityEngine;
namespace DefaultNamespace {
    public class Main : MonoBehaviour {
        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
    }
}