using UnityEngine;
using System.Collections;

public class MisselComumScript : MonoBehaviour {

    float velocidade = 10f;
    Rigidbody MisselRigidbody;

	// Use this for initialization
	void Start () {
        MisselRigidbody = GetComponent<Rigidbody>();
        MisselRigidbody.velocity = transform.TransformDirection(Vector3.forward * velocidade);
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
