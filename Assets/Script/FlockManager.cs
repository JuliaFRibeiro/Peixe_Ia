using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    // Vari�vel para o prefab do peixe, n�mero de peixes para instanciar, array para manter todos os peixes instanciados e limite de dist�ncia entre os peixes
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;

    // Velocidade minima e maxima do peixe
    [Header("Configura��es do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 100.0f)]
    public float neighbourDistance;
    [Range(1.0f, 5.0f)]
    public float rotationSpeed;

    void Start()
    {
        allFish = new GameObject[numFish];

        // Loop for para instanciar todos os peixes em uma posi��o aleat�ria, mas dentro dos limites definidos, e definir o gerenciador dos peixes como este
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position = new Vector3(Random.Range(-swinLimits.x, swinLimits.x), 
                                                                Random.Range(-swinLimits.y, swinLimits.y), 
                                                                Random.Range(-swinLimits.z, swinLimits.z));

            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        goalPos = this.transform.position;
    }

    private void Update()
    {
        // Definir o goalPos como a posi��o deste objeto
        goalPos = this.transform.position;

        // Mudar a posi��o do goalPos de forma aleat�ria
        if (Random.Range(0, 100) < 10)
        {
            goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                            Random.Range(-swinLimits.y, swinLimits.y), 
                                                            Random.Range(-swinLimits.z, swinLimits.z));
        }  
    }
}
