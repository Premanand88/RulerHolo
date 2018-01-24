using UnityEngine;
using System.Collections;
public class CursorPrem : MonoBehaviour
{
    [Tooltip("The gameobject to be used as cursor.")]
    public Transform cursorPrem;
    [Tooltip("The position the cursor is placed at when nothing is hit.")]
    public float maxDistance = 5.0f;
    // Use this for initialization   
    void Start () {   }  
    // Update is called once per frame   
    void Update () {
        if (cursorPrem == null) return;
        var camTrans = Camera.main.transform;
        RaycastHit raycastHit;
        if (Physics.Raycast(new Ray(camTrans.position, camTrans.forward), out raycastHit))
        {
            cursorPrem.position = raycastHit.point;
        }
        else
        {
            cursorPrem.position = camTrans.position + camTrans.forward * maxDistance;
            cursorPrem.up = camTrans.up;
        }
        
    }
} 