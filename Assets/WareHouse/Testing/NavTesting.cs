using UnityEngine;
using UnityEngine.AI;

public class NavTesting : MonoBehaviour
{
    public Transform target;

    NavMeshAgent agent;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
    }
}
