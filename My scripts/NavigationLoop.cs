using UnityEngine;
using UnityEngine.AI;

namespace Unity.AI.Navigation.Samples
{
    /// <summary>
    /// Use physics raycast hit from mouse click to set agent destination
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavigationLoop : MonoBehaviour
    {
        NavMeshAgent m_Agent;
        public Transform[] goals = new Transform[7];
        private int m_NextGoal = 1;
        private Animator m_Animator;

        void Start()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponent<Animator>();
        }

        void Update()
        {
            float distance = Vector3.Distance(m_Agent.transform.position, goals[m_NextGoal].position);
            if (distance < 0.5f)
            {
                m_NextGoal = m_NextGoal != 6 ? m_NextGoal + 1 : 0;
            }
            m_Agent.destination = goals[m_NextGoal].position;

            if(m_Agent.velocity.magnitude != 0f){
                m_Animator.SetBool("Running", true);
            }
            else{
                m_Animator.SetBool("Running", false);
            }
        }
    }
}