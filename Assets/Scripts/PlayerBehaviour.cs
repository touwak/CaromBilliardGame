using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour {

    #region Variables
    Camera camera;
    Rigidbody rigidbody;

    float strength = 100.0f;
    #endregion

	void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rigidbody = GetComponent<Rigidbody>();
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            HitBall();
        }
    }

    void HitBall() {
        Vector3 direction = camera.transform.forward;
        direction.y = 0.0f;
        rigidbody.AddForce(direction * strength);
    }
}
