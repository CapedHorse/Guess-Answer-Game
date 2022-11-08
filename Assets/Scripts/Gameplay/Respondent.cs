using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PikoruaTest
{
    public class Respondent : MonoBehaviour
    {
        public enum Status { Idle, Running }
        public Status status;
        public Animator respondentAnim;
        public NavMeshAgent respondentNavAgent;

        public void SetStop()
        {
            status = Status.Idle;
            respondentNavAgent.isStopped = true;
            respondentAnim.SetBool("Run", false);
        }

        public void SetGoToPoll(Vector3 destination)
        {
            respondentNavAgent.isStopped = false;
            respondentAnim.SetBool("Run", true);
            respondentNavAgent.SetDestination(destination);
        }
    }
}

