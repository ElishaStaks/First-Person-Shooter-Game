using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Vector3 m_originalPosition;
    private float m_timeAtCurrentFrame;
    private float m_timeAtLastFrame;
    private float m_fakeDelta;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // Calculate a fake delta time, so we can Shake while game is paused.
        m_timeAtCurrentFrame = Time.realtimeSinceStartup;
        m_fakeDelta = m_timeAtCurrentFrame - m_timeAtLastFrame;
        m_timeAtLastFrame = m_timeAtCurrentFrame;
    }

    public void Shake(float duration, float amount)
    {
        instance.m_originalPosition = instance.gameObject.transform.localPosition;
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.CamShake(duration, amount));
    }

    private IEnumerator CamShake(float duration, float amount)
    {
        float endTime = Time.time + duration;

        while (duration > 0)
        {
            transform.localPosition = m_originalPosition + Random.insideUnitSphere * amount;

            duration -= m_fakeDelta;

            yield return null;
        }

        transform.localPosition = m_originalPosition;
    }
}