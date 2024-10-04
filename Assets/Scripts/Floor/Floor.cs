using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Floor : MonoBehaviour
{
    [Header("Fall Settings")]
    [SerializeField] float secondsUntilFall = 3f;
    [SerializeField] float fallTime = 1.5f;
    [SerializeField] float fallDistance = 20f;
    [SerializeField] float waitToRespawn = 3f;

    [Header("Miscellaneous")]
    [SerializeField] GameObject artToDiasble;

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
        }

        // Once timer is finished, platform falls
        if (_fallTriggered == true && _timerToFall <= 0f)
        {
            // Making platform fall
            Debug.Log("falling");
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

    public void TriggerFloor()
    {
        Debug.Log("floor triggered");
        _fallTriggered = true;
    }

    private void DisableVisuals()
    {
        artToDiasble.SetActive(false);
    }

    private void ResetPlatform()
    {
        this.transform.position = _startPos;
        artToDiasble.SetActive(true);
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
