using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network_Interface : MonoBehaviour
{
    public enum Direction { Left, Right, Up }

    [Header("Network")]
    public Network_Companion networkLeft;
    public Network_Companion networkRight;
    public Network_Companion networkUp;

    public void OnEnable()
    {
        if (Mathf.Sign(transform.lossyScale.x) != Mathf.Sign(transform.localScale.x))
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public GameObject GetNetwork(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return networkLeft.networkObject;
            case Direction.Right:
                return networkRight.networkObject;
            case Direction.Up:
                return networkUp.networkObject;
            default:
                return networkUp.networkObject;
        }
    }

    public List<StoneComponent> GetExtendedStoneNetwork(Direction direction)
    {
        var retList = new List<StoneComponent>();
        var obj = GetNetwork(direction);
        if (obj != null && obj.CompareTag("Stone"))
        {
            var stoneComponent = obj.GetComponent<StoneComponent>();
            retList.Add(stoneComponent);
            retList.AddRange(stoneComponent.network.GetExtendedStoneNetwork(direction));
        }
        return retList;
    }

    public List<string> GetExtendedNetwork(Direction direction)
    {
        var retList = new List<string>();
        var obj = GetNetwork(direction);
        if (obj != null)
        {
            retList.Add(obj.tag);
            if (obj.CompareTag("Stone"))
            {
                retList.AddRange(obj.GetComponent<StoneComponent>().network.GetExtendedNetwork(direction));
            }
        }
        return retList;
    }

    public bool IsWallInNetworkDirection(Direction direction)
    {
        var obj = GetNetwork(direction);
        if (obj != null)
        {
            if (obj.CompareTag("Stone"))
            {
                return obj.GetComponent<StoneComponent>().network.IsWallInNetworkDirection(direction);
            } else if (obj.CompareTag("Ground") || obj.CompareTag("Enemy"))
            {
                return true;
            } else
            {
                return false;
            }
        } else
        {
            return false;
        }

    }
}
