using UnityEngine;
using System.Collections;

public class PowerUpBoxScript : MonoBehaviour {

    public Transform eixoX, eixoY, eixoZ;
    private MeshRenderer[] Renders;
    private float count=10;

	// Use this for initialization
	void Start () {
        Renders = this.gameObject.GetComponentsInChildren<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (count >= 10)
        {
            foreach (MeshRenderer rend in Renders)
            {
                rend.enabled = true;
            }
        }
        else
        {
            count += Time.deltaTime;
            foreach (MeshRenderer rend in Renders)
            {
                rend.enabled = false;
            }
        }

        eixoX.Rotate(new Vector3(1, 0, 0), 30f * Time.deltaTime);
        eixoY.Rotate(new Vector3(0, 1, 0), 50f * Time.deltaTime);
        eixoZ.Rotate(new Vector3(0, 0, 1), 40f * Time.deltaTime);

	}

    
    void OnTriggerEnter(Collider Objeto)
    {
        count = 0;
    }
}
