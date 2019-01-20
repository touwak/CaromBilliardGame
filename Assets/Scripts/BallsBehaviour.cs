using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsBehaviour : MonoBehaviour {

    #region Variables
    AudioSource hitSound;
    #endregion

    void Start () {
        hitSound = GetComponent<AudioSource>();
	}

    private void OnCollisionEnter(Collision collision) {
        if (!collision.collider.CompareTag("Player") &&
            !collision.collider.CompareTag("Surface")) {

            hitSound.Play();
        }
    }

}
