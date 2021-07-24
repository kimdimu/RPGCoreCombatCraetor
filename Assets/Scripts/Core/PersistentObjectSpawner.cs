﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjFab;
        static bool hasSpawned = false;
        void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObjects();
            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObj = Instantiate(persistentObjFab);
            DontDestroyOnLoad(persistentObj);
        }
    }
}
