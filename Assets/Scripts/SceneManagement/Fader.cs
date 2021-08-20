using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine curActiveFade = null;
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

       public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately( canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
        public Coroutine Fade(float target, float time)
        {
            if (curActiveFade != null)
            {
                StopCoroutine(curActiveFade);
            }
            curActiveFade = StartCoroutine(FadeRoutine(target, time));
            return curActiveFade;
        }
    }
}
