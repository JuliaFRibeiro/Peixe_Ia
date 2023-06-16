using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // Vari�vel para o gerenciador e a velocidade
    public FlockManager myManager;
    float speed;
    bool turning = false;

    private void Start()
    {
        // Definir a velocidade do peixe com base nos valores m�nimos e m�ximos
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    private void Update()
    {
        // Criar limites para delimitar uma �rea
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);

        // Criar um raycast e um vetor de dire��o do gerenciador para o peixe
        RaycastHit hit = new RaycastHit();
        Vector3 direction = myManager.transform.position - transform.position;

        // Se o peixe n�o estiver dentro da �rea dos limites
        if (!b.Contains(transform.position))
        {
            // Definir "turning" como verdadeiro e o vetor "direction" como a dire��o do gerenciador para o peixe
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        // Se o peixe detectar uma colis�o � sua frente
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            // Definir "turning" como verdadeiro e a dire��o como a reflex�o do vetor "forward" em rela��o � normal da colis�o
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        // Caso contrario
        else
        {
            turning = false;
        }

        // Definir "turning" como verdadeiro e a dire��o como a reflex�o do vetor "forward" em rela��o � normal da colis�o
        if (turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }
        // Caso contr�rio, deixar a velocidade aleat�ria e chamar a fun��o "ApplyRules"
        else
        {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            if (Random.Range(0, 100) < 20)
                ApplyRules();
        }

        // Movimenta o peixe para frente
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    // Aplica regras de comportamento do cardume
    private void ApplyRules()
    {
        // Array de todos os peixes
        GameObject[] gos;
        gos = myManager.allFish;

        // Variaveis que calculam a rota��o do peixe
        Vector3 vcentre = Vector3.zero;
        Vector3 avoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            // Se o item atual do loop n�o for este peixe
            if (go != this.gameObject)
            {
                // Calcular a dist�ncia do item atual em rela��o a este peixe
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);

                // Se a dist�ncia for menor ou igual � dist�ncia de vizinhan�a definida no gerenciador
                if (nDistance <= myManager.neighbourDistance)
                {
                    // Somar a posi��o do item atual ao vetor de centro
                    vcentre += go.transform.position;
                    groupSize++;

                    // Se a dist�ncia for menor que 1, calcular a dire��o para evitar colis�es
                    if (nDistance < 1.0f)
                    {
                        avoid = avoid + (this.transform.position - go.transform.position);
                    }

                    // Obter o componente Flock do peixe para calcular a velocidade do grupo
                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        // Se o tamanho do grupo for maior que 0
        if (groupSize > 0)
        {
            // Calcular a posi��o do centro e a velocidade do cardume
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;
            speed = Mathf.Clamp(speed, myManager.minSpeed, myManager.maxSpeed);

            // Calcular a dire��o para a qual o peixe deve se orientar
            Vector3 direction = (vcentre + avoid) - transform.position;

            // Se a dire��o n�o for igual a zero, definir a rota��o do peixe
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
