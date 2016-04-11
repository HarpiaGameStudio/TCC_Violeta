using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Linq;

public class GerenciadorScript : MonoBehaviour
{
    public GameObject[] Karts;
    public int progTotal;
    public int Laps;
    private KartControllerScript script;
    private int CheckpointsNum;

    // Use this for initialization
    void Start()
    {
        Laps = 5;
        Karts = GameObject.FindGameObjectsWithTag("Player");
        CheckpointsNum = GameObject.FindGameObjectsWithTag("Checkpoint").Length;
    }

    // Update is called once per frame
    void Update()
    {
        Karts = Karts.OrderBy(go => go.GetComponent<KartControllerScript>().contProgresso).ToArray();

        foreach (GameObject kart in Karts)
        {
            script = kart.GetComponent<KartControllerScript>();
            if (script.contProgresso >= progTotal && script.lap >= Laps)
                script.Terminou = true;
        }
    }
}
