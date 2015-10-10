using UnityEngine;
using System.Collections;

public class RenderProbe : MonoBehaviour {

    ReflectionProbe probe;

	void Start () {
        probe = GetComponent<ReflectionProbe>();
	}

    void Update () {
        probe.transform.position = new Vector3(
            Camera.main.transform.position.x,
            -Camera.main.transform.position.y,
            Camera.main.transform.position.z
        );

        probe.RenderProbe();
	}
}
