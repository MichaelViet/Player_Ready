using System.Collections;
using UnityEngine;
public class TrainAnimation : MonoBehaviour
{
    public SpriteRenderer leftBarrier;
    public SpriteRenderer rightBarrier;
    public SpriteRenderer trainSignRight;
    public SpriteRenderer trainSignLeft;
    public Transform train;
    public float signFadeDuration = 1.0f;
    public float barrierRotationDuration = 2.0f;
    public float trainMoveForwardDuration = 5.0f;
    public float trainMoveBackwardDuration = 5.0f;
    public float waitDuration = 60.0f;
    private Coroutine fadeSignsCoroutine;
    public AudioSource railroadPoezdVlevoShortSound;
    public AudioSource railroadSignalSound;


    private void Start()
    {
        SetSignsAlpha(0);
        StartCoroutine(AnimateTrain());
    }

    private IEnumerator AnimateTrain()
    {
        while (true)
        {
            fadeSignsCoroutine = StartCoroutine(FadeSignsInOut());

            yield return StartCoroutine(RotateBarriers(-90, 90));
            yield return StartCoroutine(RotateBarriers(-90, 90));
            yield return StartCoroutine(MoveTrain(9.15f, -26, trainMoveForwardDuration));
            yield return StartCoroutine(RotateBarriers(0, 0));
            StopCoroutine(fadeSignsCoroutine);
            SetSignsAlpha(0);

            yield return new WaitForSeconds(waitDuration);

            fadeSignsCoroutine = StartCoroutine(FadeSignsInOut());

            yield return StartCoroutine(RotateBarriers(-90, 90));
            yield return StartCoroutine(MoveTrain(-26, 8, trainMoveBackwardDuration));
            yield return StartCoroutine(RotateBarriers(0, 0));
            StopCoroutine(fadeSignsCoroutine);
            SetSignsAlpha(0);

            yield return new WaitForSeconds(waitDuration);
        }
    }

    private IEnumerator FadeSignsInOut()
    {
        while (true)
        {
            // Змінюємо альфу лівої лампи від 0 до 1
            for (float t = 0; t < signFadeDuration; t += Time.deltaTime)
            {
                Color colorLeft = trainSignLeft.color;
                colorLeft.a = Mathf.Lerp(0, 1, t / signFadeDuration);
                trainSignLeft.color = colorLeft;
                yield return null;
            }

            // Змінюємо альфу лівої лампи від 1 до 0
            for (float t = 0; t < signFadeDuration; t += Time.deltaTime)
            {
                Color colorLeft = trainSignLeft.color;
                colorLeft.a = Mathf.Lerp(1, 0, t / signFadeDuration);
                trainSignLeft.color = colorLeft;
                yield return null;
            }

            // Змінюємо альфу правої лампи від 0 до 1
            for (float t = 0; t < signFadeDuration; t += Time.deltaTime)
            {
                Color colorRight = trainSignRight.color;
                colorRight.a = Mathf.Lerp(0, 1, t / signFadeDuration);
                trainSignRight.color = colorRight;
                yield return null;
            }

            // Змінюємо альфу правої лампи від 1 до 0
            for (float t = 0; t < signFadeDuration; t += Time.deltaTime)
            {
                Color colorRight = trainSignRight.color;
                colorRight.a = Mathf.Lerp(1, 0, t / signFadeDuration);
                trainSignRight.color = colorRight;
                yield return null;
            }
        }
    }

    private IEnumerator MoveTrain(float startPositionX, float endPositionX, float duration)
    {
        railroadPoezdVlevoShortSound.volume = 1.0f;
        railroadPoezdVlevoShortSound.Play();
        Vector3 startPosition = train.position;
        Vector3 endPosition = new Vector3(endPositionX, train.position.y, train.position.z);

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            train.position = Vector3.Lerp(startPosition, endPosition, t / duration);
            if (t >= duration * 0.8f)
            {
                railroadPoezdVlevoShortSound.volume = Mathf.Lerp(1, 0, (t - (duration * 0.8f)) / (duration * 0.2f));
            }
            yield return null;
        }
        railroadPoezdVlevoShortSound.Stop();
    }

    private IEnumerator RotateBarriers(float leftTargetZ, float rightTargetZ)
    {
        railroadSignalSound.volume = 1.0f;
        railroadSignalSound.Play();
        Quaternion leftStartRotation = leftBarrier.transform.rotation;
        Quaternion rightStartRotation = rightBarrier.transform.rotation;

        Quaternion leftTargetRotation = Quaternion.Euler(0, 0, leftTargetZ);
        Quaternion rightTargetRotation = Quaternion.Euler(0, 0, rightTargetZ);

        for (float t = 0; t < barrierRotationDuration; t += Time.deltaTime)
        {
            float progress = Mathf.SmoothStep(0, 1, t / barrierRotationDuration);

            leftBarrier.transform.rotation = Quaternion.Slerp(leftStartRotation, leftTargetRotation, progress);
            rightBarrier.transform.rotation = Quaternion.Slerp(rightStartRotation, rightTargetRotation, progress);

            if (t >= barrierRotationDuration * 0.8f)
            {
                railroadSignalSound.volume = Mathf.Lerp(1, 0, (t - (barrierRotationDuration * 0.8f)) / (barrierRotationDuration * 0.2f));
            }

            yield return null;
        }
        railroadSignalSound.Stop();
    }

    private void SetSignsAlpha(float alpha)
    {
        Color colorRight = trainSignRight.color;
        Color colorLeft = trainSignLeft.color;
        colorRight.a = alpha;
        colorLeft.a = alpha;
        trainSignRight.color = colorRight;
        trainSignLeft.color = colorLeft;
    }
    public void StopTrainAnimation()
    {
        if (fadeSignsCoroutine != null)
        {
            StopCoroutine(fadeSignsCoroutine);
        }
        StopAllCoroutines();
        SetSignsAlpha(0);
        leftBarrier.transform.rotation = Quaternion.Euler(0, 0, 0);
        rightBarrier.transform.rotation = Quaternion.Euler(0, 0, 0);
        railroadPoezdVlevoShortSound.Stop();
        railroadSignalSound.Stop();
    }
}