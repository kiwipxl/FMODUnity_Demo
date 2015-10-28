using UnityEngine;
using System.Collections;

public class BillboardText : MonoBehaviour {

	void Start() {
	
	}

    void Update() {
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(0, 180, 0);
    }
}
