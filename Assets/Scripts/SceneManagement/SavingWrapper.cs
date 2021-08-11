﻿using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime=0.3f;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }
        IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
            Debug.Log("SavingWrapper LoadLastScene");
        }
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        private void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }

        public void Load()
        {
            //call to saving system load
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
        public void Save()
        {
            //call to saving system load
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
    }
}