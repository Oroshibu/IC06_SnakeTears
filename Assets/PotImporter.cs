using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LDtkUnity;

public class PotImporter : MonoBehaviour, ILDtkImportedFields
{
    [SerializeField] List<Sprite> sprites;

    public void OnLDtkImportFields(LDtkFields fields)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = sprites[fields.GetInt("spriteID")];
    }
}
