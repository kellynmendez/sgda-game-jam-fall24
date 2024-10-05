using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Floor : MonoBehaviour
{
    [Header("Fall Settings")]
    [SerializeField] float secondsUntilFall = 3f;
    [SerializeField] float fallTime = 1.5f;
    [SerializeField] float fallDistance = 20f;
    [SerializeField] float waitToRespawn = 3f;

    [Header("Shake Settings")]
    [SerializeField] float shakeSpeed = 100f;
    [SerializeField] float shakeAmount = 0.02f;

    [Header("Miscellaneous")]
    [SerializeField] GameObject artToDisable;

    private bool _fallTriggered = false;
    private float _timerToFall;
    private Vector3 _startPos;

    private void Awake()
    {
        _timerToFall = secondsUntilFall;
        _startPos = transform.position;
    }

    void Update()
    {
        if (_fallTriggered == true)
        {
            _timerToFall -= Time.deltaTime;
            // Shake platform
            this.transform.position = new Vector3(
                _startPos.x + (Mathf.Sin(_timerToFall * shakeSpeed) * shakeAmount),
                _startPos.y + (Mathf.Sin(_timerToFall * shakeSpeed) * shakeAmount),
                _startPos.z + (Mathf.Sin(_timerToFall * shakeSpeed) * shakeAmount));
        }

        // Once timer is finished, platform falls
        if (_fallTriggered == true && _timerToFall <= 0f)
        {
            this.transform.position = _startPos;
            // Making platform fall
            Vector3 toPos = new Vector3(
                _startPos.x, 
                _startPos.y - fallDistance, 
                _startPos.z);
            StartCoroutine(LerpFloor(this.transform, this.transform.position, 
                toPos, fallTime, waitToRespawn, DisableVisuals, ResetPlatform));
            // Resetting timer and trigger
            _fallTriggered = false;
            _timerToFall = secondsUntilFall;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _fallTriggered = true;
        }
    }

    private void DisableVisuals()
    {
        artToDisable.SetActive(false);
    }

    private void ResetPlatform()
    {
        this.transform.position = _startPos;
        artToDisable.SetActive(true);
    }

    private static IEnumerator LerpFloor(Transform target, Vector3 from, Vector3 to, 
        float duration, float respawnDuration, System.Action OnLerpComplete = null, System.Action OnRespawnComplete = null)
    {
        // initial value
        target.position = from;

        // animate value
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            target.position = Vector3.Lerp(from, to, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // final value
        target.position = to;

        if (OnLerpComplete != null) { OnLerpComplete(); }

        yield return new WaitForSeconds(respawnDuration);

        if (OnRespawnComplete != null) { OnRespawnComplete(); }
        yield break;
    }
}
