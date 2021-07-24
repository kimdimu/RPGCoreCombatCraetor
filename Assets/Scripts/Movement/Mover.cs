﻿using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        NavMeshAgent NavMeshAgent;
        [SerializeField] float maxSpeed = 5f;
        private void Start()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }
        void Update()
        {
            NavMeshAgent.enabled = !GetComponent<Health>().IsDead();
            UpdateAnimator();
        }
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            //GetComponent<Fighter>().Cancel();
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            NavMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            NavMeshAgent.destination = destination;
            NavMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            NavMeshAgent.isStopped = true;
        }
        private void UpdateAnimator()
        {
            Vector3 velocity = NavMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3( transform.position);//원래 벡터3 안되는데 이건 됨
        }
        
        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
