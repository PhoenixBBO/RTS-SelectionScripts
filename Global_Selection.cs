using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Global_Selection : MonoBehaviour
{
    [SerializeField]
    SelectedDictionary selected_table;
    RaycastHit hit;
    [SerializeField]
    bool dragSelect;


    MeshCollider selectionBox;
    Mesh selectionMesh;


    Vector2 p1;
    Vector2 p2;

    // the corners of our 2d Selection box
    Vector2[] corners;
    // the vertices of our meshcollider
    Vector3[] verts;
    public LayerMask mask;
    



    private void Start()
    {
        selected_table = GetComponent<SelectedDictionary>();
        dragSelect = false;
    }

    

    private void Update()
    {
       

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            p1 = Mouse.current.position.ReadValue();
        }

        if (Mouse.current.leftButton.isPressed)
        {
            if ((p1 - Mouse.current.position.ReadValue()).magnitude > 40)
            {
                dragSelect = true;
            }

        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (dragSelect == false)
            {

                Ray ray = Camera.main.ScreenPointToRay(p1);
                if (Physics.Raycast(ray, out hit, 50000))
                {
                    if (Keyboard.current.leftShiftKey.isPressed)
                    {
                        selected_table.addSelected(hit.transform.gameObject);
                    }
                    else
                    {
                        selected_table.deselectAll();
                        selected_table.addSelected(hit.transform.gameObject);
                    }
                }
                else
                {
                    if (Keyboard.current.leftShiftKey.isPressed)
                    { }

                    else
                    {
                        selected_table.deselectAll();
                    }
                }

            }
            else
            {
                verts = new Vector3[4];
                int i = 0;
                p2 = Mouse.current.position.ReadValue();
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

                if (Mouse.current.leftButton.isPressed)
                {
                    selected_table.deselectAll();
                }

                Destroy(selectionBox, 0.02f);

            }
            dragSelect = false;
        }
       


    }

    private void OnGUI()
    {
        if (dragSelect == true)
        {
            var rect = Utils.GetScreenRect(p1,Mouse.current.position.ReadValue());
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
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
        if(other.tag == "Enemy")
        selected_table.addSelected(other.gameObject);
    }


}
