using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection_Component : MonoBehaviour
{
    public float Timer;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        GetComponent<Renderer>().material.color = Color.blue;
    }

    private void FixedUpdate()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
           
            SelectedDictionary._SelectedDictionary.deselect(gameObject.GetInstanceID());
            Destroy(gameObject.GetComponent<Selection_Component>());
        }
    }

    //public void TargetCheck()
    //{

    //}
}
