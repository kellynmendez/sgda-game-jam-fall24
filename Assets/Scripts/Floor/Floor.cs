using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Floor : MonoBehaviour
{
    [Header("Fall Settings")]
    [SerializeField] float secondsUntilFall = 3f;
    [SerializeField] float fallTime = 1.5f;
    [SerializeField] float fallDistance = 20f;
    [SerializeField] float waitBeforeRespawn = 2.5f;
    [SerializeField] float respawnTime = 0.2f;

    [Header("Shake Settings")]
    [SerializeField] float shakeSpeed = 100f;
    [SerializeField] float shakeAmount = 0.02f;

    [Header("Miscellaneous")]
    [SerializeField] GameObject artToDisable;

    private bool _fallTriggered = false;
    private float _timerToFall;
    private Vector3 _startPos;
    private Vector3 _startScale;

    private void Awake()
    {
        _timerToFall = secondsUntilFall;
        _startPos = transform.position;
        _startScale = transform.localScale;
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
                toPos, fallTime, waitBeforeRespawn, respawnTime, _startScale, DisableVisuals, ResetPlatform));
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
        this.transform.localScale = _startScale;
        artToDisable.SetActive(true);
    }

    private static IEnumerator LerpFloor(Transform target, Vector3 from, Vector3 to,
        float fallDuration, float respawnWait, float respawnDuration, Vector3 startScale,
        System.Action OnLerpComplete = null, System.Action OnRespawn = null)
    {
        // Fall
        target.position = from;
        float elapsedTime = 0;
        while (elapsedTime < fallDuration)
        {
            target.position = Vector3.Lerp(from, to, elapsedTime / fallDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        target.position = to;

        if (OnLerpComplete != null) { OnLerpComplete(); }

        yield return new WaitForSeconds(respawnWait);
        if (OnRespawn != null) 
        { 
            OnRespawn(); 
        }

        // Respawn
        elapsedTime = 0;
        Vector3 smallScale = new Vector3(0.001f, 0.001f, 0.001f);
        target.localScale = smallScale;
        while (elapsedTime < respawnDuration)
        {
            target.localScale = Vector3.Lerp(smallScale, startScale, elapsedTime / respawnDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        target.localScale = startScale;

        yield break;
    }

    public static IEnumerator LerpColor(MaskableGraphic graphic, Color from, Color to, float duration, System.Action OnComplete = null)
    {
        // initial value
        graphic.color = from;

        // animate value
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            graphic.color = Color.Lerp(from, to, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // final value
        graphic.color = to;
        OnComplete?.Invoke();
        yield break;
    }
}
