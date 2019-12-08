using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        try
        {
            this.transform.position = new Vector3(target.transform.position.x, (target.transform.position.y) + 2, this.transform.position.z);
        }
        catch { }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
