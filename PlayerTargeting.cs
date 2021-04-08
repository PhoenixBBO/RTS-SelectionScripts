using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// This is for an fps to contiue drawing colliders where the player looks
public class PlayerTargeting : MonoBehaviour
{
    [SerializeField]
    SelectedDictionary selected_table;

    Vector2 CenterScreen;
    Vector3[] verts;
    Vector2[] corners;
    RaycastHit hit;
    MeshCollider selectionBox;
    Mesh selectionMesh;


    public float Length;
    public float Height;
    [SerializeField]
    Vector2 p1;
    [SerializeField]
    Vector2 p2;
    public LayerMask Mask;


    private void Start()
    {
        selected_table = GetComponent<SelectedDictionary>();
    }

    public void FixedUpdate()
    {
        CenterScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        p1 = new Vector2(CenterScreen.x -Length, CenterScreen.y -Height);
        p2 = new Vector2(CenterScreen.x + Length, CenterScreen.y + Height);

        verts = new Vector3[4];
        int i = 0;
        corners = getBoundingBox(p1, p2);

        foreach (Vector2 corner in corners)
        {
            Ray ray = Camera.main.ScreenPointToRay(corner);

            if (Physics.Raycast(ray, out hit, 5000f, (1 << 8)))
            {
                verts[i] = new Vector3(hit.point.x, 0, hit.point.z);
                Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.red, 1.0f);
            }
            i++;
        }

        selectionMesh = generateSelectedMesh(verts);

        selectionBox = gameObject.AddComponent<MeshCollider>();
        
        selectionBox.sharedMesh = selectionMesh;
        selectionBox.convex = true;
        selectionBox.isTrigger = true;

        //if (Mouse.current.leftButton.isPressed)
        //{
        //    selected_table.deselectAll();
        //}

        Destroy(selectionBox, 0.02f);


    }

    private void OnGUI()
    {
       
            var rect = Utils.GetScreenRect(p1,p2);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        
    }


    Vector2[] getBoundingBox(Vector2 p1, Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x)
        {
            if (p1.y > p2.y)
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else
        {
            if (p1.y > p2.y)
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }

        }

        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;

    }


    Mesh generateSelectedMesh(Vector3[] corners)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; // map the tris

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + Vector3.up * 100f;
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Enemy")
        {
           
               

            RaycastHit ray2;
            Vector3 direction = other.transform.position - Camera.main.transform.position;

            if (Physics.Raycast(Camera.main.transform.position, direction, out ray2))
            {
                if (ray2.transform.tag == "Enemy")
                {
                    selected_table.addSelected(other.gameObject);
                    other.gameObject.GetComponent<Selection_Component>().Timer = 0.5f;

                   
                    Debug.DrawRay(Camera.main.transform.position, other.transform.position, Color.green);
                }
                
            
            }

           
                  
                
               
            
                   
                
                
            
        }
        
    }



  




}
