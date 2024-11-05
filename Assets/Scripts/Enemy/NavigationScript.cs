using UnityEngine;
using UnityEngine.AI;


public class NavigationScript : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameManager.instanse.transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = player.position;
    }
}
