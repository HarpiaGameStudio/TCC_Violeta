using UnityEngine;
using System.Collections;

public class AtivacaoAranhaScript : MonoBehaviour {

    AranhaExplosivaScript script;

	// Use this for initialization
	void Start () {

        script = this.gameObject.transform.parent.gameObject.GetComponent<AranhaExplosivaScript>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        script.Ativado = true;
    }

}
