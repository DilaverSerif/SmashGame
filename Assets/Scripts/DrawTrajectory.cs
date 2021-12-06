using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour {

    public static DrawTrajectory instance;

    [SerializeField] LineRenderer _lineRenderer;
    [Range(5, 100)]
    [SerializeField] int _lineSegmentCount = 30;

    [Range(1, 100)]
    [SerializeField] private int _showPercentage = 50;

    [SerializeField] private int _linePointCount;

    private List<Vector3> _linePoints = new List<Vector3>();

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }


    public void UpdateTrajectory(Vector3 forceVector, Rigidbody rb, Vector3 startPos) {
        
        Vector3 velocity = (forceVector / rb.mass) * Time.fixedDeltaTime;
        
        float flightDuration = (2 * velocity.y) / Physics.gravity.y;
        
        float stepTime = flightDuration / _lineSegmentCount;
        
        _linePoints.Clear();    
        _linePoints.Add(startPos);
        for (int i = 1; i < _linePointCount; i++)
        {
            
            float stepTimePassed = stepTime * i;
            
            Vector3 newMovementVector = new Vector3(
                velocity.x * stepTimePassed,
                velocity.y * stepTimePassed - .5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                velocity.z * stepTimePassed
            );

            Vector3 newPointInLine = -newMovementVector + startPos;

            RaycastHit hit;
            if (Physics.Raycast(_linePoints[i - 1], newPointInLine - _linePoints[i - 1], out hit, (newPointInLine - _linePoints[i - 1]).magnitude)) {
                _linePoints.Add(hit.point);
                //Top denk gelecekse
                _lineRenderer.startColor = Color.green;
                _lineRenderer.endColor = Color.green;
                break;
            }
            else {
                
                //Top Iskalayacaksa
                _lineRenderer.startColor = Color.red;
                _lineRenderer.endColor = Color.red;
            }
            _linePoints.Add(newPointInLine);
        }
        
        _lineRenderer.positionCount = _linePoints.Count;
        _lineRenderer.SetPositions(_linePoints.ToArray());
    }

    public void HideLine() {
        _lineRenderer.positionCount = 0;
    }
    
    private void OnDisable() {
        instance = null;
    }
}
