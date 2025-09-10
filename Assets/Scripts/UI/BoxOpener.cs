using UnityEngine;
using System.Collections;

public class BoxLid : MonoBehaviour
{
    public Transform tampa;         // Referência da tampa
    public float anguloAberto = -90f; // Ângulo final no eixo X (ex: -90 pra abrir pra trás)
    public float speed = 2f;        // Velocidade da abertura

    private bool abrindo = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !abrindo)
        {
            StartCoroutine(AbrirTampa());
        }
    }

    IEnumerator AbrirTampa()
    {
        abrindo = true;

        // Rotação inicial da tampa
        Quaternion rotInicial = tampa.localRotation;
        // Rotação final = abre no eixo X mantendo Y e Z iguais
        Quaternion rotFinal = Quaternion.Euler(anguloAberto, rotInicial.eulerAngles.y, rotInicial.eulerAngles.z);

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            tampa.localRotation = Quaternion.Lerp(rotInicial, rotFinal, t);
            yield return null;
        }
    }
}
