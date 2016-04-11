using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SeleçãoPersonagensScript : MonoBehaviour
{

    public Transform Violeta;
    public Transform Ayah;
    public Transform Kruchulla;
    public Transform Momoto;
    public Transform Jeshi;
    public Transform Rangitoto;
    public Transform Atawe;
    public string proximaCena;
    public bool Loading = false;

    private Button botVioleta;
    private float velocidade = 10f;
    private bool CarregarCena = false;

	private void Start ()
    {
	}

	private void Update ()
    {
        #region Carregar a próxima Cena + Loading
        if (Loading)
        {
            if (Input.GetButtonDown("Fire1") && CarregarCena == false) // Se o jogador já selecionou a pista/personagem e ainda não entrou no loading
            {
                CarregarCena = true; // Load scene é true para previnir que inicie o loading de mais de uma cena de uma vez
                StartCoroutine(CarregarNovaCena()); //Inicia uma coroutine que carrega a cena desejada
            }

            if (CarregarCena) //Se o loading já foi iniciado 
            {
                // Pulsa a transparencia do texto de Loading pro jogador saber que o jogo não travou
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1")) //Botão A
            {
                SceneManager.LoadScene(proximaCena);
            }
        }
        #endregion

        #region Muda o modelo de acordo com o botão selecionado
        GameObject botAtual = EventSystem.current.currentSelectedGameObject;
        string nomeBotao = botAtual.name;
        switch (nomeBotao)
        {
            case "Violeta":
                {
                    Violeta.gameObject.SetActive(true);
                    Violeta.Rotate(new Vector3(0f, 1f, 0f), velocidade * Time.deltaTime);
                    Ayah.gameObject.SetActive(false);
                    Kruchulla.gameObject.SetActive(false);
                    Momoto.gameObject.SetActive(false);
                    Jeshi.gameObject.SetActive(false);
                    Atawe.gameObject.SetActive(false);
                    Rangitoto.gameObject.SetActive(false);
                    if (Loading)
                    {
                        proximaCena = "Pista 7 - Doceria da Violeta";
                    }
                }
                break;
            case "Atawe":
                {
                    Atawe.gameObject.SetActive(true);
                    Atawe.Rotate(new Vector3(0f, 1f, 0f), velocidade * Time.deltaTime);
                    Violeta.gameObject.SetActive(false);
                    Ayah.gameObject.SetActive(false);
                    Kruchulla.gameObject.SetActive(false);
                    Momoto.gameObject.SetActive(false);
                    Jeshi.gameObject.SetActive(false);
                    Rangitoto.gameObject.SetActive(false);
                    if (Loading)
                    {
                        proximaCena = "Pista 2 - Ruínas de Atawe";
                    }
                }
                break;
            case "Ayah":
                {
                    Ayah.gameObject.SetActive(true);
                    Ayah.Rotate(new Vector3(0f, 1f, 0f), velocidade * Time.deltaTime);
                    Violeta.gameObject.SetActive(false);
                    Kruchulla.gameObject.SetActive(false);
                    Momoto.gameObject.SetActive(false);
                    Jeshi.gameObject.SetActive(false);
                    Atawe.gameObject.SetActive(false);
                    Rangitoto.gameObject.SetActive(false);
                    if (Loading)
                    {
                        proximaCena = "Pista 6 - Noite da Ayah";
                    }
                }
                break;
            case "Rangitoto":
                {
                    Rangitoto.gameObject.SetActive(true);
                    Rangitoto.Rotate(new Vector3(0f, 1f, 0f), velocidade * Time.deltaTime);
                    Violeta.gameObject.SetActive(false);
                    Ayah.gameObject.SetActive(false);
                    Kruchulla.gameObject.SetActive(false);
                    Momoto.gameObject.SetActive(false);
                    Jeshi.gameObject.SetActive(false);
                    Atawe.gameObject.SetActive(false);
                    if (Loading)
                    {
                        proximaCena = "Pista 5 - Vulcão do Rangitoto";
                    }
                }
                break;
            case "Momoto":
                {
                    Momoto.gameObject.SetActive(true);
                    Momoto.Rotate(new Vector3(0f, 1f, 0f), velocidade * Time.deltaTime);
                    Violeta.gameObject.SetActive(false);
                    Ayah.gameObject.SetActive(false);
                    Kruchulla.gameObject.SetActive(false);
                    Jeshi.gameObject.SetActive(false);
                    Atawe.gameObject.SetActive(false);
                    Rangitoto.gameObject.SetActive(false);
                    if (Loading)
                    {
                        proximaCena = "Pista 4 - Mina do Momoto";
                    }
                }
                break;
            case "Kruchulla":
                {
                    Kruchulla.gameObject.SetActive(true);
                    Kruchulla.Rotate(new Vector3(0f, 1f, 0f), velocidade * Time.deltaTime);
                    Violeta.gameObject.SetActive(false);
                    Ayah.gameObject.SetActive(false);
                    Momoto.gameObject.SetActive(false);
                    Jeshi.gameObject.SetActive(false);
                    Atawe.gameObject.SetActive(false);
                    Rangitoto.gameObject.SetActive(false);
                    if (Loading)
                    {
                        proximaCena = "Pista 1 - Enseada da Kruchula";
                    }
                }
                break;
            case "Jeshi":
                {
                    Jeshi.gameObject.SetActive(true);
                    Jeshi.Rotate(new Vector3(0f, 1f, 0f), velocidade * Time.deltaTime);
                    Violeta.gameObject.SetActive(false);
                    Ayah.gameObject.SetActive(false);
                    Kruchulla.gameObject.SetActive(false);
                    Momoto.gameObject.SetActive(false);
                    Atawe.gameObject.SetActive(false);
                    Rangitoto.gameObject.SetActive(false);
                    if (Loading)
                    {
                        proximaCena = "Pista 3 - Geleiras de Jeshi";
                    }
                }
                break;
        }
        #endregion
    }

    IEnumerator CarregarNovaCena()
    {
        //espera 3 segundos
        yield return new WaitForSeconds(5);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(proximaCena);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }

}
