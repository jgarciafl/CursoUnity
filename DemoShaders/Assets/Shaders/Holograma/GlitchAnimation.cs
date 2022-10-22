using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GlitchAnimation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 _timeRange;
    [SerializeField] private float _timeWait = 0.2f;

    private Material _material;
    private int _hash_useGlith = Shader.PropertyToID("_Use_Glitch");

    private void Start()
    {
        _material = GetComponent<Renderer>().material;

        StartCoroutine(StartGlitch());
    }

    private IEnumerator StartGlitch()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_timeRange.x, _timeRange.y));

            _material.SetFloat(_hash_useGlith, 1);

            yield return new WaitForSeconds(_timeWait);

            _material.SetFloat(_hash_useGlith, 0);
        }
    }

}
