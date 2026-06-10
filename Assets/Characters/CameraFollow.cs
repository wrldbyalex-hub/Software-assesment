using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("The target")]
    public Transform target; // who the camera is following
    
    [Header("Camera Settings")]
    public float SmoothTime = 0.08f; // how long it takes to catch up to the target
    public Vector2 Offset = new Vector2(0, 0.3f); // how far the camera is from the target

    private Vector3 velocity = Vector3.zero; 

    void LateUpdate()
    {
        if (target == null) return; // if there is no target, do nothing

        // Where the position will be with the offset
        Vector3 CameraTargetPosition = new Vector3(target.position.x + Offset.x, target.position.y + Offset.y, transform.position.z);
        // Z is not changed 

        // SmoothDamp is apparently better for camera movement than lerp
        transform.position = Vector3.SmoothDamp(transform.position, CameraTargetPosition, ref velocity, SmoothTime);
    }
}