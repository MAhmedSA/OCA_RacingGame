using UnityEngine;
using System.Collections;

public static class CanvasGroupExtensions
{
    public static void Show(this CanvasGroup cg, float duration = 0f)
    {
        CoroutineRunner.Instance.StartCoroutine(Fade(cg, 1f, duration, true));
    }

    public static void Hide(this CanvasGroup cg, float duration = 0f)
    {
        CoroutineRunner.Instance.StartCoroutine(Fade(cg, 0f, duration, false));
    }

    private static IEnumerator Fade(CanvasGroup cg, float target, float duration, bool interactable)
    {
        float start = cg.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, target, time / duration);
            yield return null;
        }

        cg.alpha = target;
        cg.interactable = interactable;
        cg.blocksRaycasts = interactable;
    }
}