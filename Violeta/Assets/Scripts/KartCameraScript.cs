using UnityEngine;
using System.Collections;

public class KartCameraScript : MonoBehaviour {

    public Transform Kart;
    public bool cameraReversa = false;
    private bool virada = false;
    private float distancia;
    private float altura;
    private float rotacaoDelay;
    private float alturaDelay;
    private float anguloCamera, anguloKart;
    private float alturaCamera, alturaKart;
    private float rotacao;
    private Quaternion rot;
    KartControllerScript KartScript;

	// Use this for initialization
	void Start () {
        KartScript = Kart.GetComponent<KartControllerScript>();
	}
	
	//Late update eh executado depois do update, e é chamado uma vez por frame
	void LateUpdate () {
        anguloKart = rotacao;
        alturaKart = Kart.position.y + altura;
        anguloCamera = transform.eulerAngles.y;
        alturaCamera = transform.position.y;
        anguloCamera = Mathf.LerpAngle(anguloCamera, anguloKart, rotacaoDelay * Time.deltaTime);
        alturaCamera = Mathf.Lerp(alturaCamera, alturaKart, alturaDelay * Time.deltaTime);
        rot = Quaternion.Euler(0, anguloCamera, 0);
        transform.position = Kart.position;
        transform.position -= rot * Vector3.forward * distancia;
        transform.position = new Vector3(transform.position.x, alturaCamera, transform.position.z);
        transform.LookAt(new Vector3(Kart.position.x,Kart.position.y+2f,Kart.position.z));
	}

    void FixedUpdate(){
        if (Input.GetButton("Camera"))
        {
            cameraReversa = true;
        }
        else
        {
            cameraReversa = false;
        }

        if (!KartScript.drift)
        {
            if (cameraReversa)
            {
                rotacao = Kart.eulerAngles.y + 180;
                altura = 2f;
                distancia = 5f;
                rotacaoDelay = 30f;
                alturaDelay = 5.0f;
                virada = true;
            }
            else
            {
                rotacao = Kart.eulerAngles.y;
                altura = 2f;
                distancia = 5f;
                alturaDelay = 5.0f;
                rotacaoDelay = 1.0f;
                if (virada)
                {
                    rotacaoDelay = 30f;
                    virada = false;
                }

            }
        }

    }
}
