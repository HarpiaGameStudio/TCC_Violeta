using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
Comandos:
Analógico- Direita/Esquerda
RB- Acelera
LB- Freia/Ré
X- Camera Reversa
Y- Pulo/Drift

 * Ainda não funfa:
A- Soltar Power-UP Comum
B- Soltar Power-Up Especial

Dúvidas:
1- Drift
4- Camera sem Lerp quando reverte
5- Qual a melhor maneira de fazer a AI?
6- Banco de dados pra armazenar as pistas já completadas?
 
*/

public class KartControllerScript : MonoBehaviour
{

    #region Variáveis/Atributos

    #region Rodas
    //Declaração dos colisores das rodas
    public WheelCollider RodaFDir;
    public WheelCollider RodaFEsq;
    public WheelCollider RodaTDir;
    public WheelCollider RodaTEsq;

    //Declaração das meshs das rodas
    public Transform RodaFDirMesh;
    public Transform RodaFEsqMesh;
    public Transform RodaTDirMesh;
    public Transform RodaTEsqMesh;
    #endregion

    #region Atributos Publicos
    //Centro de Massa
    public Transform CenterOfMass;

    //Tipo de personagem
    public string tamanhoPersonagem = "Grande";

    #endregion

    #region Atritutos de controle
    //Rigidbody do Kart
    private Rigidbody KartRigidbody;
    //Propriedades de controle do Kart
    private float velMax, auxVel, angAtual;
    private float aceleracao, velDesaceleracao;
    private float direcaoBaixaVel, direcaoAltaVel;
    private float direcaoBaixaVelDrift, direcaoAltaVelDrift;
    private float friccaoLateral, friccaoFrontal;
    private float friccaoLateralDrift, friccaoFrontalDrift;
    private float auxRotX, auxRotZ;
    private float vertical, horizontal;
    public bool drift = false;
    private Vector3 angDrift, impulsoDrift;
    private Quaternion rotDrift;
    private float velAtual; //Velocidade atual do kart
    #endregion

    #region Atributos para PowerUps
    //Variáveis para os Power-Ups
    private Vector3 PowerUpPosition;
    private Quaternion PowerUpRotation;
    private int powerUpTipo = 0;
    private bool boost = false;
    private int contBoost = 0;
    private bool especialDisponivel = false;
    private float countEspecial, CooldownEspecial;
    public Transform PosFrente, PosTras;
    private KartCameraScript CameraScript;
    private AranhaExplosivaScript AranhaScript;
    public Camera camKart;

    //Prefabs Power-Ups
    public Rigidbody MisselPrefab;
    public Rigidbody MisselGuiadoPrefab;
    public Object AranhaExplosivaPrefab;
    public Object OleoPrefab;
    #endregion

    #region Atributos para Interface
    //Textos de interface
    public Text contadorVoltas;
    public Text textVelocimetro;
    private int lap = 0;
    private bool jaContou;
    #endregion

    #region Checkpoint
    //Checkpoint
    public GameObject ultimoCheckpoint;
    public int contProgresso;
    private Vector3 posInicial;
    private Quaternion rotInicial;
    #endregion

    #endregion

    void Start()
    {
        KartRigidbody = GetComponent<Rigidbody>();              //Identifica e pega a referencia do rigidbody do carro 
        //Ajusta o centro de massa do Kart
        KartRigidbody.centerOfMass = new Vector3(CenterOfMass.localPosition.x * transform.localScale.x, 
                                                 CenterOfMass.localPosition.y * transform.localScale.y -0.003f, 
                                                 CenterOfMass.localPosition.z * transform.localScale.z -0.005f);

        //Atrito das rodas
        friccaoLateral = RodaTDir.sidewaysFriction.stiffness;   //Armazena a fricção lateral normal das rodas
        friccaoFrontal = RodaTDir.forwardFriction.stiffness;    //Armazena a fricção frontal normal das rodas
        friccaoLateralDrift = 0.9f;                             //Armazena a fricção lateral das rodas no drift
        friccaoFrontalDrift = 0.9f;                             //Armazena a fricção frontal das rodas no drift

        CameraScript = camKart.GetComponent<KartCameraScript>();

        PersonalizaPersonagem(); //Chama a função que muda os atributos de acordo com o personagem
        posInicial = KartRigidbody.position; //Salva sua posição inicial na corrida
        rotInicial = KartRigidbody.rotation; //Salva sua rotação inicial na corrida
        CooldownEspecial = 60; //Define o tempo de recarga do powerup especial
    }

    void FixedUpdate()
    {
        //FixedUpdate no lugar de Update por que o update só é chamado uma vez por frame enquanto 
        //o fixed pode ser chamado várias, logo, é mais usado para simulação.
        Controle();    //Chama a função com as mecanicas básicas do kart
        Drift();       //Chama a função com a mecanica do drift
    }

    void Update()
    {
        InserirInterface(); //coloca informações na tela (velocimetro, contagem de voltas...)
        UsarPowerUp(); //Chama função com a mecanica de powerup

        #region Movimento das meshs das rodas

        //Faz as rodas rodarem conforme o carro anda
        RodaFDirMesh.Rotate(RodaFDir.rpm / (60f * 360f * Time.deltaTime), 0, 0);
        RodaFEsqMesh.Rotate(RodaFEsq.rpm / (60f * 360f * Time.deltaTime), 0, 0);
        RodaTDirMesh.Rotate(RodaTDir.rpm / (60f * 360f * Time.deltaTime), 0, 0);
        RodaTEsqMesh.Rotate(RodaTEsq.rpm / (60f * 360f * Time.deltaTime), 0, 0);

        //Faz as rodas dianteiras virarem quando o volante vira
        auxRotX = RodaFDirMesh.localEulerAngles.x;
        auxRotZ = RodaFDirMesh.localEulerAngles.z;
        RodaFDirMesh.localEulerAngles = new Vector3(auxRotX, RodaFDir.steerAngle, auxRotZ);
        auxRotX = RodaFEsqMesh.localEulerAngles.x;
        auxRotZ = RodaFEsqMesh.localEulerAngles.z;
        RodaFEsqMesh.localEulerAngles = new Vector3(auxRotX, RodaFEsq.steerAngle, auxRotZ);

        //Faz as rodas traseiras virarem quando o volante vira      
        auxRotX = RodaTDirMesh.localEulerAngles.x;
        auxRotZ = RodaTDirMesh.localEulerAngles.z;
        RodaTDirMesh.localEulerAngles = new Vector3(auxRotX, RodaTDir.steerAngle, auxRotZ);
        auxRotX = RodaTEsqMesh.localEulerAngles.x;
        auxRotZ = RodaTEsqMesh.localEulerAngles.z;
        RodaTEsqMesh.localEulerAngles = new Vector3(auxRotX, RodaTEsq.steerAngle, auxRotZ);
        
        #endregion
    }

    private void Controle()
    {
        #region Velocimetro

        //Armazena a velocidade atual.
        auxVel = KartRigidbody.velocity.magnitude / (2*velMax) ;
        velAtual = 2 * 22 / 7 * RodaFDir.radius * RodaFDir.rpm * 60 / 100000;
        velAtual = Mathf.Round(velAtual);

        #endregion

        #region Aceleração do Kart

        //Se a vel está abaixo da máxima, pode acelerar. Senão, para de acelerar.
        if (velAtual <= velMax)
        {
            if (boost)
            {
                vertical = Input.GetAxis("Vertical");
                RodaTDir.motorTorque = aceleracao * vertical * 500f;
                RodaTEsq.motorTorque = aceleracao * vertical * 500f;
                RodaFDir.motorTorque = aceleracao * vertical * 500f;
                RodaFEsq.motorTorque = aceleracao * vertical * 500f;
                if (contBoost <= 25 )
                {
                    contBoost++;
                }
                else 
                {
                    boost = false;
                    contBoost = 0;
                }
            }
            else
            {
                vertical = Input.GetAxis("Vertical");
                RodaTDir.motorTorque = aceleracao * vertical;
                RodaTEsq.motorTorque = aceleracao * vertical;
                RodaFDir.motorTorque = aceleracao * vertical;
                RodaFEsq.motorTorque = aceleracao * vertical;
            }
        }
        else // Velocidade acima do permitido, zera a aceleração do motor.
        {
                RodaTDir.motorTorque = 0;
                RodaTEsq.motorTorque = 0;
                RodaFDir.motorTorque = 0;
                RodaFEsq.motorTorque = 0;
        }

        #endregion

        #region Desaceleração do kart

        //Desaceleração - Quando não estão apertadas as setinhas pra cima e pra baixo
        if (Input.GetButton("Vertical") == false) //Se a tecla de aceleração não está pressionada, aciona o brake
        {
            RodaTDir.brakeTorque = velDesaceleracao;
            RodaTEsq.brakeTorque = velDesaceleracao;
            RodaFDir.brakeTorque = velDesaceleracao;
            RodaFEsq.brakeTorque = velDesaceleracao;
        }
        else //Se a tecla de aceleração está pressionada, zera o brake
        {
            RodaTDir.brakeTorque = 0;
            RodaTEsq.brakeTorque = 0;
            RodaFDir.brakeTorque = 0;
            RodaFEsq.brakeTorque = 0;
        }

        #endregion

        #region Direção do kart

            horizontal = Input.GetAxis("Horizontal");
            if (drift)
            {
                angAtual = Mathf.Lerp(direcaoBaixaVelDrift, direcaoAltaVelDrift, auxVel);
                RodaFDir.steerAngle = -angAtual;
                RodaFEsq.steerAngle = -angAtual;
                RodaTDir.steerAngle = angAtual*0.2f;
                RodaTEsq.steerAngle = angAtual*0.2f;
            }
            else
            {
                angAtual = Mathf.Lerp(direcaoBaixaVel, direcaoAltaVel, auxVel);
                angAtual *= horizontal;
                RodaFDir.steerAngle = angAtual;
                RodaFEsq.steerAngle = angAtual;
                RodaTDir.steerAngle = 0f;
                RodaTEsq.steerAngle = 0f;
            }

        #endregion  

        #region Verifica Fim do Drift

        //Fim do drift - Retira o efeito do drift
        if (Input.GetButtonUp("Jump")) //Verifica se o drift terminou
        {
            if (drift)
            {
                drift = false;                                       //Marca o drift como inativo
                ConfigurarFriccao(friccaoFrontal, friccaoLateral,1); //Volta com os valores padrões de friccao nas dianteiras
                ConfigurarFriccao(friccaoFrontal, friccaoLateral,1); //Volta com os valores padrões de friccao nas traseiras
            }
        }

        #endregion
    }

    private void Drift()
    {
        #region Botão do drift está pressionado?

        if (Input.GetButtonDown("Jump")) //Pressionado
            drift = true;

        if (Input.GetButtonUp("Jump")) //Solto
            drift = false;

        #endregion

        #region Execução do drift

        if (drift) //Está no drift
        {
            if (velAtual > 0)
            {
                ConfigurarFriccao(friccaoFrontalDrift, friccaoLateralDrift, 1);
                ConfigurarFriccao(friccaoFrontalDrift, friccaoLateralDrift, 2);
            }
            else
            {
                ConfigurarFriccao(friccaoFrontal, friccaoLateral,1); //Volta o atrito pro normal no eixo dianteiro
                ConfigurarFriccao(friccaoFrontal, friccaoLateral,2); //Volta o atrito pro normal no eixo traseiro
            }
        }
        else //Não tá no drift
        {
            ConfigurarFriccao(friccaoFrontal, friccaoLateral,1); //Volta o atrito pro normal no eixo dianteiro
            ConfigurarFriccao(friccaoFrontal, friccaoLateral,2); //Volta o atrito pro normal no eixo traseiro           
        }

        #endregion
    }

    private void ConfigurarFriccao(float friccaoFrontalAtual, float friccaoLateralAtual, int eixo)
    {
        WheelFrictionCurve auxFoward, auxSideway;
        //Eixo = 1 é o da frente
        //Eixo = 2 é o de trás

        if (eixo == 1)
        {
            //Roda Dianteira Direita
            auxFoward = RodaFDir.forwardFriction;
            auxSideway = RodaFDir.sidewaysFriction;
            auxFoward.stiffness = friccaoFrontalAtual;
            auxSideway.stiffness = friccaoLateralAtual;
            RodaFDir.forwardFriction = auxFoward;
            RodaFDir.sidewaysFriction = auxSideway;

            //Roda Dianteira Esquerda
            auxFoward = RodaFEsq.forwardFriction;
            auxSideway = RodaFEsq.sidewaysFriction;
            auxFoward.stiffness = friccaoFrontalAtual;
            auxSideway.stiffness = friccaoLateralAtual;
            RodaFEsq.forwardFriction = auxFoward;
            RodaFEsq.sidewaysFriction = auxSideway;
        }

        if (eixo == 2)
        {
            //Roda Traseira Direita
            auxFoward = RodaTDir.forwardFriction;
            auxSideway = RodaTDir.sidewaysFriction;
            auxFoward.stiffness = friccaoFrontalAtual;
            auxSideway.stiffness = friccaoLateralAtual;
            RodaTDir.forwardFriction = auxFoward;
            RodaTDir.sidewaysFriction = auxSideway;

            //Roda Traseira Esquerda
            auxFoward = RodaTEsq.forwardFriction;
            auxSideway = RodaTEsq.sidewaysFriction;
            auxFoward.stiffness = friccaoFrontalAtual;
            auxSideway.stiffness = friccaoLateralAtual;
            RodaTEsq.forwardFriction = auxFoward;
            RodaTEsq.sidewaysFriction = auxSideway;
        }

    }

    private void PersonalizaPersonagem()
    {
        switch (tamanhoPersonagem) //Personalização de acordo com o tamanho do personagem
        {
            case "Pequeno":
                {
                    velMax = 80;
                    aceleracao = 40;
                    direcaoBaixaVel = 60f;
                    direcaoAltaVel = 55f;
                }
                break;
            case "Médio":
                {
                    velMax = 70;
                    aceleracao = 50;
                    direcaoBaixaVel = 55f;
                    direcaoAltaVel = 50f;
                }
                break;
            case "Grande":
                {
                    velMax = 10000000000000000;
                    aceleracao = 2000;
                    direcaoBaixaVel = 50f;
                    direcaoAltaVel = 45f;
                }
                break;
        }

        //Repassa a personalização do tamanho pra outras propriedades
        velDesaceleracao = aceleracao * 0.2f;                   //Configura o valor da vel de desaceleração
        direcaoBaixaVelDrift = direcaoBaixaVel * 1.25f;         //Configura o valor sensibilidade da direção no drift
        direcaoAltaVelDrift = direcaoAltaVel * 1.25f;           //Configura o valor sensibilidade da direção no drift
    }
    
    private IEnumerator Delay(int segundos, string tipo)
    {
        yield return new WaitForSeconds(segundos);
        switch (tipo)
        {
            case "colisão":
                {
                    jaContou = false;
                }
                break;
    }
    }

    private void InserirInterface()
    {
        contadorVoltas.text = "Lap: " + lap.ToString();
        textVelocimetro.text = "Velocidade: " + velAtual.ToString();
    }

    public void voltarNoCheckpoint()
    {
        if (ultimoCheckpoint != null)
        {
            KartRigidbody.position = ultimoCheckpoint.transform.position;
            KartRigidbody.rotation = ultimoCheckpoint.transform.rotation;
        }
        else
        {
            KartRigidbody.position = posInicial;
            KartRigidbody.rotation = rotInicial;
        }
    }

    public void Rodar()
    {
        Destroy(this.gameObject);
    }

    private void UsarPowerUp()
    {
        #region Power-Up Comum

        if (CameraScript.cameraReversa)
        {
            PowerUpPosition = PosTras.position;
            PowerUpRotation = PosTras.rotation;
        }
        else
        {
            PowerUpPosition = PosFrente.position;
            PowerUpRotation = KartRigidbody.rotation;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            switch (powerUpTipo)
            {
                case 1: //Missel Comum
                    {
                        Instantiate(MisselPrefab, PowerUpPosition, PowerUpRotation);
                        powerUpTipo = 0;
                    }
                    break;
                case 2: //Boost de Velocidade
                    {
                        boost = true;
                        powerUpTipo = 0;
                    }
                    break;
                case 3: //Missel Teleguiado
                    {
                        Instantiate(MisselGuiadoPrefab, PowerUpPosition, PowerUpRotation);
                        powerUpTipo = 0;
                    }
                    break;
                case 4:  //Oleo
                    {
                        Instantiate(OleoPrefab, PosTras.position, PosTras.rotation);
                        powerUpTipo = 0;
                    }
                    break;
                case 5: //Aranha Explosiva
                    {
                        Instantiate(AranhaExplosivaPrefab, PosTras.position, PosTras.rotation);
                        powerUpTipo = 0;
                    }
                    break;
            }
        }
        #endregion

        #region Power-Up Especial

        #region Tempo de Recarga PowerUp Especial
        if (countEspecial >= CooldownEspecial)
        {
            especialDisponivel = true;
        }
        else
        {
            countEspecial += Time.deltaTime;
            especialDisponivel = false;
        }
        #endregion

        #region Soltar PowerUp Especial
        if (Input.GetButtonDown("Fire2"))
        {
            if (especialDisponivel)
            {
                switch (this.gameObject.name)
                {
                    case "Violeta":
                        {

                        }
                        break;
                    case "Kruchula":
                        {

                        }
                        break;
                    case "Jeshi":
                        {

                        }
                        break;
                    case "Momoto":
                        {

                        }
                        break;
                    case "Atawe":
                        {

                        }
                        break;
                    case "Rangitoto":
                        {

                        }
                        break;
                    case "Ayah":
                        {

                        }
                        break;
                }
                countEspecial = 0; //Inicia novamente a contagem do tempo de recarga
            }
        }
        #endregion

        #endregion

    }

    private void OnTriggerExit(Collider Objeto)
    {
        if (Objeto.gameObject.CompareTag("AranhaAoE"))
        {
            AranhaScript = Objeto.transform.parent.gameObject.GetComponent<AranhaExplosivaScript>();
            AranhaScript.removeAlvo = this.gameObject;
        }
    }

    private void OnTriggerEnter(Collider Objeto)
    {
        if (Objeto.gameObject.CompareTag("AranhaAoE"))
        {
            AranhaScript = Objeto.transform.parent.gameObject.GetComponent<AranhaExplosivaScript>();
            AranhaScript.addAlvo = this.gameObject;
        }

        if (Objeto.gameObject.CompareTag("Limite"))
        {
            voltarNoCheckpoint();
        }

        #region Checkpoints
        if (Objeto.gameObject.CompareTag("Checkpoint"))
        {
            if (ultimoCheckpoint == null || ultimoCheckpoint != Objeto.gameObject)
            {
                ultimoCheckpoint = Objeto.gameObject;
                contProgresso++;
            }
        }
        #endregion

        #region Contador de Voltas
        if (Objeto.gameObject.CompareTag("LapCounter"))
        {
            if (jaContou)
            {
                StartCoroutine(Delay(30, "colisão"));
            }
            else
            {
                lap++;
                jaContou = true;
            }
        }
        #endregion

        #region Randomização de PowerUps Comuns

        if (Objeto.gameObject.CompareTag("PowerUpBox"))
        {
            powerUpTipo = Random.Range(1, 6);
           // powerUpTipo = 2;

        }

        #endregion

    }

}
