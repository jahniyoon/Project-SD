using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBullet : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {

            agent.SetDestination(target.position);
        }
        
    }
}
