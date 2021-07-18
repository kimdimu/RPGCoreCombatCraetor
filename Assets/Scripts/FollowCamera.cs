using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        //[SerializeField] Vector3 relationship;
        //[SerializeField] float dist;
        //[SerializeField] float height;


        private void LateUpdate() //애니메이션 업데이트가 후순위기 때문에 카메라가 먼저 움직이고 캐릭터가 움직이게 된다. 반대로 만들어주기위해 Late 적용
        {
            transform.position = target.position;
        }

        //void Update()
        //{
        //transform.LookAt(target);

        //transform.position = target.position + relationship;

        //if ((target.position - transform.position).magnitude < dist)
        //{
        //    transform.position += -(target.position - transform.position)*Time.deltaTime;
        //}
        //else if((target.position - transform.position).magnitude > dist)
        //{
        //    transform.position += (target.position - transform.position) * Time.deltaTime;
        //}
        //else
        //{
        //    transform.position = target.position;
        //}
        //}
    }
}