using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class npcmovement : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;        
    private Animator m_Animator;


    [SerializeField] LayerMask groundLayer, playerLayer;

    //patrol
    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        m_Animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
        m_Animator.SetBool("Running", agent.velocity.magnitude > 2f);
    }

    void Patrol(){
        if (!walkpointSet | (agent.velocity.magnitude == 0f)) SearchForDest();
        if (walkpointSet) agent.SetDestination(destPoint);
        if(Vector3.Distance(transform.position, destPoint) < 8) walkpointSet = false;
    }

    void SearchForDest(){
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer)){
            walkpointSet = true;
        }
    }
}
