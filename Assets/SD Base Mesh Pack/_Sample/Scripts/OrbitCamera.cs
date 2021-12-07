using UnityEngine; 
public class OrbitCamera : MonoBehaviour {
    // The target of the camera. The camera will always point to this object.
    public Transform target;
    // The default distance of the camera from the target.
    public float distance = 20.0f;
    // Control the speed of zooming and dezooming.
    public float zoomStep = 1.0f;
    // The speed of the camera. Control how fast the camera will rotate.
    public float xSpeed = 1f;
    public float ySpeed = 1f;
    // The position of the cursor on the screen. Used to rotate the camera.
    private float m_X = 0.0f;
    private float m_Y = 0.0f;
    // Distance vector. 
    private Vector3 m_DistanceVector;
    void Start () {
        m_DistanceVector = new Vector3(0.0f,0.0f,-distance);
        Vector2 angles = this.transform.localEulerAngles;
        m_X = angles.x;
        m_Y = angles.y;
        this.Rotate(m_X, m_Y);
    }
    void LateUpdate () {
    if ( target )
    {
        this.RotateControls();
        this.Zoom();
    }
 }
    void RotateControls () {
    if (Input.GetButton("Fire2"))
    {
        m_X += Input.GetAxis("Mouse X") * xSpeed;
        m_Y += -Input.GetAxis("Mouse Y")* ySpeed;
        this.Rotate(m_X,m_Y);
    }
 }
    void Rotate (float x, float y) {
        // Transform angle in degree in quaternion form used by Unity for rotation.
        Quaternion rotation = Quaternion.Euler(y,x,0.0f);
        // The new position is the target position + the distance vector of the camera
        // rotated at the specified angle.
        Vector3 position = rotation * m_DistanceVector + target.position;
        // Update the rotation and position of the camera.
        transform.rotation = rotation;
        transform.position = position;
    }
    void Zoom () {
        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f) {
            this.ZoomOut();
        }
        else
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) {
            this.ZoomIn();
        }
    }
    void ZoomIn () {
        distance -= zoomStep;
        m_DistanceVector = new Vector3(0.0f,0.0f,-distance);
        this.Rotate(m_X,m_Y);
    }
    void ZoomOut () {
        distance += zoomStep;
        m_DistanceVector = new Vector3(0.0f,0.0f,-distance);
        this.Rotate(m_X,m_Y);
    }
}