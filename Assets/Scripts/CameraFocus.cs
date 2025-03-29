using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    private Vector3 wantedPos;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private GameObject target;
    private void FixedUpdate()
    {
        wantedPos = target.transform.position;
        wantedPos.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, wantedPos, smoothSpeed * Time.fixedDeltaTime);
    }
}
