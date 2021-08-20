using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    enum DestinationIdentifier
    {
        A,B,C,D,E
    }
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime;
        [SerializeField] float fadeInTime;
        [SerializeField] float fadeWaitTime;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }
        private IEnumerator Transition()
        {
            if(sceneToLoad <0)
            {
                Debug.Log("sceneToLoad fail");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            player.enabled=false;

            yield return fader.FadeOut(fadeOutTime);

            //Save cur lev
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();//바뀌기전

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            PlayerController newplayer = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newplayer.enabled = false;

            //Load cur lev
            savingWrapper.Load();//바뀐후


            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            savingWrapper.Save();//바뀐후 씬을 다시 저장

            yield return new WaitForSeconds(fadeWaitTime);

            fader.FadeIn(fadeInTime);//안기다릴래!
            //yield return fader.FadeIn(fadeInTime); 이 초를 기다린다는 뜻.
            newplayer.enabled = true;
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            //player.GetComponent<NavMeshAgent>().enabled = false;
            //player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            //player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }

            return null;
        }
    }
}