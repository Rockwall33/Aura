using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//[ExecuteInEditMode]
public class LightFlicker : MonoBehaviour
{
    private float _currentFactor = 1.0f;
    private Vector3 _currentPos;
    private float _deltaTime;
    private Vector3 _initPos;
    private float _targetFactor;
    private Vector3 _targetPos;

    private float _time;
    private float _timeLeft;
    public Color baseColor;
    public float maxFactor = 1.2f;
    public float minFactor = 1.0f;
    public float moveRange = 0.1f;
    public float speed = 0.1f;

    private void Start()
    {
        Random.InitState((int)transform.position.x + (int)transform.position.y);
    }

    //

    private void OnEnable()
    {
        _initPos = transform.localPosition;
        _currentPos = _initPos;
    }

    //

    private void OnDisable()
    {
        transform.localPosition = _initPos;
    }

    //

#if !UNITY_EDITOR
    private void Update()
    {
        _deltaTime = Time.deltaTime;
#else
    void OnRenderObject()
    {
        float currentTime = (float)EditorApplication.timeSinceStartup;
        _deltaTime = currentTime - _time;
        _time = currentTime;
#endif

        if(_timeLeft <= _deltaTime)
        {
            _targetFactor = Random.Range(minFactor, maxFactor);
            _targetPos = _initPos + Random.insideUnitSphere * moveRange;
            _timeLeft = speed;
        }
        else
        {
            float weight = _deltaTime / _timeLeft;
            _currentFactor = Mathf.Lerp(_currentFactor, _targetFactor, weight);

            //GetComponent<AuraLight>().overridingColor = baseColor * _currentFactor;
            GetComponent<Light>().color = baseColor * _currentFactor;
            _currentPos = Vector3.Lerp(_currentPos, _targetPos, weight);
            transform.localPosition = _currentPos;
            _timeLeft -= _deltaTime;
        }
    }
}
