using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NM_Controller : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    public GameObject path;
    private Transform[] PathPoints;

    public int index = 0;

    public float minDist = 1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        PathPoints = new Transform[path.transform.childCount];
        for(int i = 0; i< PathPoints.Length; i++){
            PathPoints[i] = path.transform.GetChild(i);
        }
    }

    void Update()
    {
        roam();
    }

    void roam(){
        if(Vector3.Distance(transform.position , PathPoints[index].position) < minDist){
            if(index >= 0 && index < PathPoints.Length){
                index += 1;
            }
            if(index + 1 != PathPoints.Length){
                index = 0;
            }
        }

        agent.SetDestination(PathPoints[index].position); 
        animator.SetFloat("vertical", !agent.isStopped ? 1 : 0); 
    }
}
