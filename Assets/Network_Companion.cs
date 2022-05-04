using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network_Companion : MonoBehaviour
{
    public class Network
    {
        public GameObject left;
        public GameObject right;
        public GameObject up;
    }

    public enum Direction { Left, Right, Up}

    public Network network;
    
    void Start()
    {
        network = new Network();
    }

    public List<string> ProjectNetwork(Direction direction)
    {
        var retList = new List<string>();
        retList.Add(network.left.tag);
        if (network.left != null)
        {
            if (network.left.CompareTag("Stone"))
            {
                retList.AddRange(network.left.GetComponent<StoneComponent>().network.ProjectNetwork(direction));
            }
        }
        return retList;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("Stone"))
        //{
        print(GetVectorDirection(collision.transform.position - transform.position));
        //}
    }

    private Direction GetVectorDirection(Vector2 vector)
    {
        //right = -90
        //left = 90
        //top = 0
        var angle = Vector2.SignedAngle(Vector2.up, vector);
        if (angle < - 45)
        {
            return Direction.Right;
        } else if (angle > 45)
        {
            return Direction.Left;
        } else
        {
            return Direction.Up;
        }
    }
}
