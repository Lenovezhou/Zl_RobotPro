using UnityEngine;
using System.Collections;

public class ZBXController : MonoBehaviour
{

    private Transform mainCamera;
	// Use this for initialization
	void Start ()
	{
	    mainCamera = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.rotation = mainCamera.localRotation;
	}
}
