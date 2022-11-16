using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PikoruaTest
{
    public class Respondent : MonoBehaviour
    {
        public enum Status { Idle, Running }
        public Status status = Status.Idle;
        public Animator respondentAnim;
        public NavMeshAgent respondentNavAgent;
        public Grid.GridPoint initPoint;
        public Vector3 destination;
        [Range(0, 1f)]public float destinationOffset = 0.5f;

        private void Update()
        {
            if (status == Status.Idle)
                return;

            if (ArrivedAtDestination())
            {
                SetStop();
            }
        }

        bool ArrivedAtDestination()
        {
            if (transform.position.x <= destination.x + destinationOffset && transform.position.x >= destination.x - destinationOffset
                &&
                transform.position.z <= destination.z + destinationOffset && transform.position.z >= destination.z - destinationOffset)
                return true;
            else
                return false;
        }

        public void SetStop()
        {
            status = Status.Idle;
            //respondentNavAgent.isStopped = true;
            respondentNavAgent.enabled = false;            
            destination = initPoint.position;
            respondentAnim.SetBool("Run", false);
        }

        public void SetGoToPoll(Vector3 _destination)
        {
            status = Status.Running;
            respondentNavAgent.enabled = true;
            //respondentNavAgent.isStopped = false;
            respondentAnim.SetBool("Run", true);
            destination = _destination;
            respondentNavAgent.SetDestination(destination);
        }

        public void Reset()
        {
            transform.position = initPoint.position;
            SetStop();
        }
    }
}

