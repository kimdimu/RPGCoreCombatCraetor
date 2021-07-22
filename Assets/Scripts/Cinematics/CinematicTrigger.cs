using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool hadOn = false;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")&&!hadOn)
            {
                hadOn = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
