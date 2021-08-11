using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
  
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

       public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1)//alpha is not 1
            {
                canvasGroup.alpha += Time.deltaTime/time;
                yield return null;
                //moving alpha toward 1
            }
        }

        internal object FadeIn(object fadeInTime)
        {
            throw new NotImplementedException();
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)//alpha is not 1
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
                //moving alpha toward 1
            }
        }
    }
}
