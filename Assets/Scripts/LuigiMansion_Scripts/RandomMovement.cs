using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [Header("Controllers")]
    public CharacterController CharacterController;

    [Header("Movement Settings")]
    public float capturingSpeed = 3.0f;

    [Header("Random Rotate Settings")]
    public float rotateSpeed = 10;
    public AnimationCurve lerpEase = default;
    public float yRot;

    private bool right;

    private void OnEnable()
    {
        StartCoroutine(RotateTo());
        StartCoroutine(ChooseDir());
    }

    private void Update()
    {
        CharacterController.Move(transform.forward * capturingSpeed * Time.deltaTime);
    }

    private void OnDisable()
    {
        StopCoroutine("RotateTo");
        StopCoroutine("ChooseDir");
        StopAllCoroutines();
    }

    IEnumerator RotateTo()
    {
        yRot += Random.Range(15, 45) * (right ? 1 : -1);
        float distance = Mathf.Abs(Mathf.DeltaAngle(transform.localEulerAngles.y, yRot));

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(0, yRot, 0);

        float animateTime = 0;
        float animationLength = distance / rotateSpeed;

        while (animateTime < animationLength)
        {
            animateTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRot, endRot, lerpEase.Evaluate(animateTime / (animationLength)));
            yield return null;
        }

        StartCoroutine(RotateTo());
    }

    IEnumerator ChooseDir()
    {
        yield return new WaitForSeconds(Random.Range(1, 3));
        right = (Random.value > 0.5f);
        StartCoroutine(ChooseDir());
    }
}
