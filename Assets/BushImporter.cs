using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LDtkUnity;

public class BushImporter : MonoBehaviour, ILDtkImportedFields
{
    public void OnLDtkImportFields(LDtkFields fields)
    {
        if (fields.GetBool("flipX"))
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, 1);
            transform.position += Vector3.right * 2;
        }
        else
        {
            transform.localScale = new Vector3(1, transform.localScale.y, 1);
        }

        if (fields.GetBool("flipY"))
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, transform.localScale.x * -90));
            transform.position += Vector3.right * 2 * transform.localScale.x;
        }
    }
}            
