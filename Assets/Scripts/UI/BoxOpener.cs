using UnityEngine;
using System.Collections;

public class BoxLid : MonoBehaviour
{
    public Transform tampa;         // Referência da tampa
    public float anguloAberto = -90f; // Ângulo final no eixo X (ex: -90 pra abrir pra trás)
    public float speed = 2f;        // Velocidade da abertura
    public AudioSource somAbrido;
    public GameObject Logo;
    public AudioSource Music;

    private bool abrindo = false;



    private void Start()
    {
        if (Music != null)
            Music.Play();
    }
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

        // Toca o som se existir
        if (somAbrido != null)
            somAbrido.Play();

        if (Logo != null)
            Logo.SetActive(false);

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
