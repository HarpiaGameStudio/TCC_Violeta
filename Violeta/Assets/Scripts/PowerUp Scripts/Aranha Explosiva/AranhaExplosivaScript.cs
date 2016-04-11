using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AranhaExplosivaScript : MonoBehaviour {

    public Transform Circulo;
    public bool Ativado;
    private List<GameObject> Alvos;
    public GameObject addAlvo, removeAlvo;

	// Use this for initialization
	void Start () {
        
        Circulo.localScale = new Vector3(0, 0, 0);
        Alvos = new List<GameObject>();
        Ativado = false;
        addAlvo = null;
        removeAlvo = null;
	}
	
	// Update is called once per frame
	void Update () {

        if (addAlvo != null)
        {
            if (!Alvos.Contains(addAlvo))
            {
                Alvos.Add(addAlvo);
            }
            addAlvo = null;
        }

        if (removeAlvo != null)
        {
                Alvos.Remove(removeAlvo);
                removeAlvo = null;
        }

        if (Ativado)
            Ativar();

       
	}

    void Ativar()
    {
        //Play na Animação **************

        if (Circulo.localScale.x < 4.8f) //Exibir Range 
        {
            Circulo.localScale += new Vector3(1.2f, 1.2f, 1.2f) * Time.deltaTime * 4;
        }
        else //Explode
        {
            //Play nas Particulas ***************

            foreach (GameObject alvo in Alvos)
            {
                alvo.GetComponent<KartControllerScript>().Rodar();
            }
            Destroy(this.gameObject);

        }


    }

}
