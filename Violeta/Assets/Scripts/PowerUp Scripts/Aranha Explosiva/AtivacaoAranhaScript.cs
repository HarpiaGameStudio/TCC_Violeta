using UnityEngine;
using System.Collections;

public class AtivacaoAranhaScript : MonoBehaviour {

    private AranhaExplosivaScript script;
    public float count = 0;

	// Use this for initialization
	void Start () {

        script = this.gameObject.transform.parent.gameObject.GetComponent<AranhaExplosivaScript>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        ativar();
    }

    private void ativar()
    {
        if (count >= 2.1f)
        {
            script.Ativado = true;
        }
        else
        {
            count += Time.deltaTime;
        }
    }

}
