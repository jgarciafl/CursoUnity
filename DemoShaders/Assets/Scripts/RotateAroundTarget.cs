using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RotateAroundTarget : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform _target;
    [SerializeField, Range(-10f, 10f)] private float _targetYOffset;
    [Space]
    [SerializeField, Range(0f, 10f)] private float _rotationDistance = 3.5f;
    [SerializeField, Range(-1f, 5f)] private float _rotationOffset = 0.75f;
    [SerializeField, Range(0f, 2f)] private float _rotationSpeed = 0.25f;

    private void Update()
    {
        float time = Time.time * Mathf.PI * _rotationSpeed;

        float xPos = _target.position.x + Mathf.Sin(time) * _rotationDistance;
        float yPos = _target.position.y + _rotationOffset;
        float zPos = _target.position.z + Mathf.Cos(time) * _rotationDistance;

        transform.position = new Vector3(xPos, yPos, zPos);
        transform.LookAt(_target.position + new Vector3(0, _targetYOffset, 0));
    }
}