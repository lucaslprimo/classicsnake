using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachBoard : MonoBehaviour
{
    public GameObject boardPosition;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = Camera.main.WorldToScreenPoint(boardPosition.transform.position) + offset;
    }
}
