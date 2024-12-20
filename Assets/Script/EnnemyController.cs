using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyController : MonoBehaviour
{
    public int Hp = 3;
    public NavMeshAgent agent;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
    }
    private void OnCollisonEnter(Collision collision)
    {
        Hp--;
        if(Hp<=0)
        {
            Destroy(this.gameObject);
        }
    }
}
