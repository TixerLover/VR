using UnityEngine;

public class PatrullaVoladora : MonoBehaviour
{
    [Header("Ruta de Vuelo")]
    public Transform[] puntosRuta; 
    public float velocidad = 15f;
    public float velocidadGiro = 5f;
    public float distanciaCambio = 2f;

    private int indiceActual = 0;

    void Update()
    {
        if (puntosRuta.Length == 0) return;

        Vector3 direccion = puntosRuta[indiceActual].position - transform.position;

        if (direccion != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadGiro * Time.deltaTime);
        }

        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, puntosRuta[indiceActual].position) < distanciaCambio)
        {
            indiceActual = (indiceActual + 1) % puntosRuta.Length;
        }
    }
}