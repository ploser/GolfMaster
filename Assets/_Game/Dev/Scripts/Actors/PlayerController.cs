using GolfMaster.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace GolfMaster.Actors
{
    public class PlayerController : MonoBehaviour
    {
        public Transform TargetObject;

        [SerializeField] private Rigidbody attachedRigidBody;
        [SerializeField] private Animator anim;
        [SerializeField] NavMeshAgent agent;

        private float _currentSpeed;
        private Vector3 _prePosition;

        private void Start()
        {
            _prePosition = this.transform.position;
        }

        private void Update()
        {
            var movement = Vector3.zero;

            if (Input.GetKey(KeyCode.A))
            {
                movement += Vector3.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movement += Vector3.back;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement += Vector3.right;
            }
            if (Input.GetKey(KeyCode.W))
            {
                movement += Vector3.forward;
            }

            //if (movement != Vector3.zero)
            //{
            //    movement.Normalize();
            //    attachedRigidBody.MovePosition(attachedRigidBody.position + movement * GameSettings.Instance.PlayerSettings.PlayerSpeed * Time.deltaTime);
            //    this.transform.forward = Vector3.Lerp(this.transform.forward, movement, Time.deltaTime * 10);
            //}

            Debug.DrawRay(TargetObject.position, Vector3.down * 100, Color.red);
            if (Physics.Raycast(TargetObject.position, Vector3.down, out RaycastHit hit, 100, 1 << 10))
            {
                agent.SetDestination(hit.point);
            }

           

            var dif = Vector3.Distance(this.transform.position, _prePosition);
            _currentSpeed = dif / Time.deltaTime;
            _prePosition = this.transform.position;

            print(_currentSpeed);
            anim.SetFloat("speed", Mathf.Clamp01(_currentSpeed / 3f));
        }
    }
}
