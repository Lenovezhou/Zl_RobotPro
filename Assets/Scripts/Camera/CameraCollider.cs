using UnityEngine;
using System.Collections;

public class CameraCollider : MonoBehaviour {



    void OnTriggerEnter(Collider co)
    {
        co.GetComponent<MeshRenderer>().enabled = false;
        
    }

    void OnTriggerExit(Collider co)
    {
        co.GetComponent<MeshRenderer>().enabled = true;
    }


}
