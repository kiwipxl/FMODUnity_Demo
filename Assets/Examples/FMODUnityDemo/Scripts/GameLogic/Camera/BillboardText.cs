using UnityEngine;
using System.Collections;

/*
** Faces the camera at all times.
** Can be applied to GUIText objects, for example
*/

public class BillboardText : MonoBehaviour {

    private void Update() {
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(0, 180, 0);
    }
}
