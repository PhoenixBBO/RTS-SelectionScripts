using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDictionary : MonoBehaviour
{
    public static SelectedDictionary _SelectedDictionary;
    [SerializeField]
    public Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();


    private void Start()
    {
        _SelectedDictionary = this;
    }
    public void addSelected(GameObject go)
    {

        int id = go.GetInstanceID();

        if (!(selectedTable.ContainsKey(id)))
        {
            selectedTable.Add(id, go);
            go.AddComponent<Selection_Component>();

        }

    }

    public void deselect(int id)
    {
        selectedTable.Remove(id);
    }

    public void deselectAll()
    {
        foreach (KeyValuePair<int, GameObject> pair in selectedTable)
        {
            if (pair.Value != null)
            {
                Destroy(selectedTable[pair.Key].GetComponent<Selection_Component>());
            }
        }
        selectedTable.Clear();
    }

    private void FixedUpdate()
    {
        //foreach (KeyValuePair<int, GameObject> pair in selectedTable)
        //{
        //    if (pair.Value != null)
        //    {
        //        Destroy(selectedTable[pair.Key].GetComponent<Selection_Component>());
        //    }
        //}
    }

}
