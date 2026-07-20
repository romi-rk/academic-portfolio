using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace Unity.AI.Navigation.Samples
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavigationLoop1 : MonoBehaviour
    {
        private NavMeshAgent m_Agent;
        private Animator m_Animator;

        public Transform waypointParent; // Assign the parent GameObject here
        private List<Transform> goals = new List<Transform>();
        private int m_NextGoal = 0;

        void Start()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponent<Animator>();

            // Automatically gather all child waypoints
            if (waypointParent != null)
            {
                foreach (Transform child in waypointParent)
                {
                    goals.Add(child);
                }
            }
        }

        void Update()
        {
            if (goals == null || goals.Count == 0)
                return;

            float distance = Vector3.Distance(m_Agent.transform.position, goals[m_NextGoal].position);
            if (distance < 0.5f)
            {
                m_NextGoal = (m_NextGoal + 1) % goals.Count;
            }

            m_Agent.destination = goals[m_NextGoal].position;

            m_Animator.SetBool("Running", m_Agent.velocity.magnitude != 0f);
        }
    }
}
