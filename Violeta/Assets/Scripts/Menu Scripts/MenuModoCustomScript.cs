using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuModoCustomScript : MonoBehaviour {

    public Button botaoSinglePlayer, botaoMultiPlayer;

    // Use this for initialization
    void Start()
    {

        botaoSinglePlayer.onClick = new Button.ButtonClickedEvent();
        botaoMultiPlayer.onClick = new Button.ButtonClickedEvent();
        botaoSinglePlayer.onClick.AddListener(() => Single());
        botaoMultiPlayer.onClick.AddListener(() => Multi());

    }

    private void Single()
    {
        SceneManager.LoadScene("Seleção de Personagem (Custom Single Player)");
    }

    private void Multi()
    {
        SceneManager.LoadScene("Menu Modo Custom");
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
