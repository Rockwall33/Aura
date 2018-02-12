using UnityEngine;

public class SinRotationOffset : MonoBehaviour
{
    private Quaternion _initialRotation;
    public float sinAmplitude = 15;
    public Vector3 sinDirection = Vector3.up;
    public float sinOffset;
    public float sinSpeed = 1;
    public Space space = Space.Self;

    private void Start()
    {
        _initialRotation = space == Space.Self ? transform.localRotation : transform.rotation;
    }

    private void Update()
    {
        float angle = sinAmplitude * Mathf.Sin(Time.time * sinSpeed + sinOffset);
        Quaternion rotationOffset = Quaternion.AngleAxis(angle, sinDirection);

        if(space == Space.Self)
        {
            transform.localRotation = _initialRotation * rotationOffset;
        }
        else
        {
            transform.rotation = _initialRotation * rotationOffset;
        }
    }
}
