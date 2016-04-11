using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSeleçãoModoJogoScript : MonoBehaviour {

    public Button botaoCarreira, botaoCustom;

	// Use this for initialization
	void Start () {

        botaoCarreira.onClick = new Button.ButtonClickedEvent();
        botaoCustom.onClick = new Button.ButtonClickedEvent();
        botaoCarreira.onClick.AddListener(() => Carreira());
        botaoCustom.onClick.AddListener(() => Custom());
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void Carreira()
    {
        SceneManager.LoadScene("Menu Modo Carreira");
    }

    private void Custom()
    {
        SceneManager.LoadScene("Menu Modo Custom");
    }
}
