using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform _target;

    public float _smoothSpeed = 0.125f;
    public Vector3 _offset;

    void LateUpdate()
    {
        if(_target == null) return;
        
        Vector3 _desiredPosition = _target.position + _offset;
        Vector3 _smoothPosition = Vector3.Lerp(transform.position, _desiredPosition, _smoothSpeed);
        transform.position = _smoothPosition;

        transform.LookAt(_target);
    }
}
