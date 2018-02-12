using UnityEngine;

public class MoveSinCos : MonoBehaviour
{
    private Vector3 _initialPosition;
    public float cosAmplitude; // = Random.Range(0.5f, 2.0f);

    public Vector3 cosDirection = Vector3.right;
    public float cosOffset; // = Random.Range(-Mathf.PI, Mathf.PI);
    public float cosSpeed; // = Random.Range(2.0f, 3.5f);
    public float sinAmplitude; // = Random.Range(0.5f, 2.0f);
    public Vector3 sinDirection = Vector3.up;
    public float sinOffset; // = Random.Range(-Mathf.PI, Mathf.PI);
    public float sinSpeed; // = Random.Range(2.0f, 3.5f);

    public Space space = Space.Self;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        Vector3 sinVector = sinDirection.normalized * Mathf.Sin(Time.time * sinSpeed + sinOffset) * sinAmplitude;
        Vector3 cosVector = cosDirection.normalized * Mathf.Cos(Time.time * cosSpeed + cosOffset) * cosAmplitude;

        sinVector = space == Space.World ? sinVector : transform.localToWorldMatrix.MultiplyVector(sinVector);
        cosVector = space == Space.World ? cosVector : transform.localToWorldMatrix.MultiplyVector(cosVector);

        transform.position = _initialPosition + sinVector + cosVector;
    }
}
