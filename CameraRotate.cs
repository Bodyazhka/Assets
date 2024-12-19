using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraRotate : MonoBehaviour
{
   public float rc = 2;
   public Transform cL1;
   public Transform cL2;
   void Update(){
    cL1.transform.Rotate(0, Input.GetAxis("Horizontal") * -rc, 0);
    //cL1.transform.Rotate(Input.GetAxis("Vertical") * rc, 0, 0);
   }
}
