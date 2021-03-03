using System.Collections;
using UnityEngine;

// Allows child-classes to safely fade a value in and out over animation curves.
public abstract class Fader : MonoBehaviour
{
    public AnimationCurve     fadeInCurve = AnimationCurve.Linear(0, 0, 3, 1);
    public AnimationCurve     fadeOutCurve = AnimationCurve.Linear(0, 1, 3, 0);

    protected abstract float  Value { get; set; }
    private Coroutine         CurrentCoroutine { get; set; } = null;

    public void Fade(FadeType type)
    {
        if (CurrentCoroutine != null) // If this component is already fading, cancel the previous fade.
            StopCoroutine(CurrentCoroutine);

        AnimationCurve curve = type == FadeType.In ? fadeInCurve : fadeOutCurve;
        CurrentCoroutine = StartCoroutine(FadeCoroutine(curve));
    }

    private IEnumerator FadeCoroutine(AnimationCurve curve)
    {
        float endTime = AnimationCurveHelper.LastKey(curve).time;
        float elapsedTime = 0;

        AnimationCurveHelper.ChangeFirstKeyframe(curve, Value);

        while (elapsedTime <= endTime)
        {
            Value = curve.Evaluate(elapsedTime);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Value = AnimationCurveHelper.LastKey(curve).value;
    }
}

public enum FadeType
{
    In,
    Out
}