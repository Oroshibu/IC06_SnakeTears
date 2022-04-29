using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Ray_Controller : MonoBehaviour
{
    //public Material lineMat;
    //public GameObject player;
    public LayerMask mask;
    //public bool playerCast = false;
    public float startAngle;
    public float endAngle;
    public int raysCount;
    public float raysLengthMax;
    public float raysSpeed;
    public float angleSpeed;
    private float raysLength;
    private float startAngleAtm;
    private float endAngleAtm;
    private float angleSpeedAtm;
    private float fadingTime = 0f;

    private Mesh mesh;

    private MeshRenderer meshRenderer;

    public void FadeAway()
    {
        fadingTime = 0.2f;
    }

    private static Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    private void StoneEnemy(GameObject go)
    {
        if (Mathf.Sign(this.transform.parent.localScale.x) != Mathf.Sign(go.transform.parent.localScale.x))
        {
            //other.gameObject.GetComponentInParent<Enemy_Behavior>().Stone();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            StoneEnemy(other.gameObject);
        }
    }

    void Start()
    {
        mesh = new Mesh();
        mesh.Clear();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

        meshRenderer = GetComponent<MeshRenderer>();

        gameObject.SetActive(false);
    }

    private void DrawRays()
    {
        Vector3 origin = Vector3.zero;

        Vector3[] vertices = new Vector3[raysCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[raysCount * 3];

        vertices[0] = origin;
        uv[0] = origin + new Vector3(0, 0.5f, 0);

        float startAngleTmp;
        float endAngleTmp;
        if (this.transform.parent.transform.lossyScale.x == -1)
        {
            startAngleTmp = 180 + startAngleAtm;
            endAngleTmp = 180 + endAngleAtm;
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            startAngleTmp = startAngleAtm;
            endAngleTmp = endAngleAtm;
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        float step = (endAngleTmp - startAngleTmp) / raysCount;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (float i = startAngleTmp; i < endAngleTmp; i += step)
        {
            //RaycastHit2D hit = Physics2D.Raycast(origin, new Vector2(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad)), raysLength, mask);
            Vector3 vertex;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, GetVectorFromAngle(i), raysLength, mask);


            if (hit.collider == null)
            {

                vertex = origin + GetVectorFromAngle(i) * raysLength;
                //Debug.DrawLine(transform.position, vertex, Color.red);
            }
            else
            {
                //vertex = origin + GetVectorFromAngle(i) * raysLength;
                vertex = hit.point;
                //Debug.DrawLine(transform.position, vertex, Color.green);
                vertex -= transform.position;
                //vertex += origin + GetVectorFromAngle(i) * 0.5f;

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    StoneEnemy(hit.transform.gameObject);
                }

            }


            vertices[vertexIndex] = vertex;
            if (this.transform.parent.transform.lossyScale.x > 0)
            {
                uv[vertexIndex] = vertex / raysLengthMax + new Vector3(0, 0.5f, 0);
            }
            else
            {
                uv[vertexIndex] = -vertex / raysLengthMax + new Vector3(0, 0.5f, 0);
            }


            if (vertexIndex > 1)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex;
                triangles[triangleIndex + 2] = vertexIndex - 1;

                triangleIndex += 3;
            }

            vertexIndex += 1;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
    }

    private void FixedUpdate()
    {
        DrawRays();
        raysLength *= raysSpeed;
        //raysLength = Mathf.Pow(raysLength, raysSpeed);
        if (raysLength > raysLengthMax)
        {
            raysLength = raysLengthMax;
        }

        startAngleAtm *= angleSpeedAtm;
        if (startAngleAtm < startAngle)
        {
            startAngleAtm = startAngle;
        }

        endAngleAtm *= angleSpeedAtm;
        if (endAngleAtm > endAngle)
        {
            endAngleAtm = endAngle;
        }
    }

    private void OnDisable()
    {
        if (mesh != null)
        {
            mesh.Clear();
        }

        if (raysLength != 0.5f)
        {
            raysLength = 0.5f;
        }

        if (startAngleAtm != -0.5f)
        {
            startAngleAtm = -0.5f;
        }

        if (endAngleAtm != 0.5f)
        {
            endAngleAtm = 0.5f;
        }

        //this.GetComponentInParent<PlayerMovement>().StopAttacking();
    }

    private void OnEnable()
    {
        //meshRenderer = GetComponent<MeshRenderer>();
        //meshRenderer.material.SetFloat("_Opacity", 1);
        fadingTime = 10000f;
        angleSpeedAtm = angleSpeed;
    }

    private void Update()
    {
        if (fadingTime > 0)
        {
            if (fadingTime > 1)
            {
                meshRenderer.material.SetFloat("_Opacity", 1f);
                angleSpeedAtm = angleSpeed;
            }
            else
            {
                meshRenderer.material.SetFloat("_Opacity", fadingTime * 5);
                angleSpeedAtm = 0.97f;
            }

            fadingTime -= Time.deltaTime;

        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void RayShootStart()
    {
        gameObject.SetActive(true);
    }

    public void RayShootStop()
    {
        gameObject.SetActive(false);
    }

}
