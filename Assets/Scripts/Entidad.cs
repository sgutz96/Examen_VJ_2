using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entidad : MonoBehaviour
{
    public Transform[] destino; // Los destinos hacia los que se moverá el agente
    private NavMeshAgent agente;

    public int pos = 0;

    private bool esperando = false; // Flag para saber si estamos esperando

    private string estado = null;

    private Transform player; // Transform del jugador (el objeto con el tag "Player")

    void Start()
    {
        // Obtén el componente NavMeshAgent
        agente = GetComponent<NavMeshAgent>();
        estado = "Patrulla";

        // Busca al jugador con el tag "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (estado == "Patrulla")
        {
            // Verifica si el array de destinos no está vacío
            if (destino != null && destino.Length > 0)
            {
                // Mueve el agente hacia el destino actual
                agente.SetDestination(destino[pos].position);
            }
        }
        if (estado == "Perseguir" && player != null)
        {
            // El agente sigue al jugador
            agente.SetDestination(player.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entro algo");

        // Verifica si la entidad colisionó con un objeto que tenga el tag "Waypoint" y no estamos esperando
        if (other.CompareTag("Waypoint") && !esperando)
        {
            // Llamar a la corrutina para esperar antes de cambiar de destino
            StartCoroutine(EsperarYCambiarDestino());
        }

        if (other.CompareTag("Player"))
        {
            // Cambia el estado a "Perseguir" cuando el agente colisiona con el jugador
            estado = "Perseguir";
        }
    }

    // Corrutina que espera 1.5 segundos antes de continuar
    private IEnumerator EsperarYCambiarDestino()
    {
        // Activar flag de espera
        esperando = true;

        // Espera 1.5 segundos
        yield return new WaitForSeconds(1.5f);

        // Incrementa la posición del destino
        pos++;

        // Si se excede el índice de destinos, vuelve al primero
        if (pos >= destino.Length)
        {
            pos = 0;
        }

        // Desactivar flag de espera
        esperando = false;
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            // Cambia el estado a "Perseguir" cuando el agente colisiona con el jugador
            estado = "Patrulla";
        }

    }
}
