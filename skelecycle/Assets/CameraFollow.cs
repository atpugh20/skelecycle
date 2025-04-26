using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;

    public float CameraOffsetX = 2.0f;
    public float CameraOffsetY = 1.0f;

    void Update() {
        // Update is called once per frame
        if (target != null) {
            transform.position = new Vector3(
                target.position.x + CameraOffsetX, 
                target.position.y + CameraOffsetY, 
                transform.position.z);
        } 
    }
}
