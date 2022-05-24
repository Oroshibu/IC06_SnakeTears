using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LDtkUnity;

public class PayerSpawner : MonoBehaviour, ILDtkImportedFields
{
    public GameObject playerPrefab;

    private void Start()
    {
        SpawnPlayer();
    }
    
    public void OnLDtkImportFields(LDtkFields fields)
    {
        if (fields.GetBool("flip"))
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void SpawnPlayer()
    {
        Transform player = Instantiate(playerPrefab, transform.position, transform.rotation).transform;
        player.localScale = transform.localScale;
        player.parent = transform.parent;
        Game_Manager.i.StartGameLevel();
        
        Destroy(gameObject);
    }
}
