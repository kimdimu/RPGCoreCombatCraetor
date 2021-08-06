using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed=1;
    [SerializeField] bool isHoming = true;
    [SerializeField] GameObject hitEffect = null;
    Health target=null;
    float damage = 0;
    private void Start()
    {
        transform.LookAt(GetAimLocation());
    }
    void Update()
    {
        if (target == null) return;

        if (isHoming && !target.IsDead())
        {
            transform.LookAt(GetAimLocation());
        }
        transform.Translate(Vector3.forward*speed*Time.deltaTime);
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private Vector3 GetAimLocation()//캡슐콜라이더!
    {
        CapsuleCollider targetCC = target.GetComponent<CapsuleCollider>();
        //여기에 isDead면 return으로 해놨었다.
        if (targetCC==null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCC.height / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != target) return; //지정한 상대가 아니라면
        {
            if (target.IsDead()) return;
            target.TakeDamage(damage);
            if (hitEffect != null)
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            Destroy(gameObject);
        }
    }
}
