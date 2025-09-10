using UnityEngine;
using TMPro;

public class TextoPiscante : MonoBehaviour
{
    public TMP_Text texto;
    public float velocidade = 1f; // velocidade do fade
    private bool aumentando = true;
    private bool ativo = true; // controla se o texto ainda deve piscar

    private void Start()
    {
        if (texto == null)
            texto = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        // Detecta clique do player
        if (Input.GetMouseButtonDown(0)) // clique esquerdo ou toque na tela
        {
            ativo = false;
            texto.enabled = false; // faz o texto sumir
            return;
        }

        if (!ativo) return;

        // Faz o fade do texto
        Color cor = texto.color;
        if (aumentando)
        {
            cor.a += Time.deltaTime * velocidade;
            if (cor.a >= 1f) { cor.a = 1f; aumentando = false; }
        }
        else
        {
            cor.a -= Time.deltaTime * velocidade;
            if (cor.a <= 0f) { cor.a = 0f; aumentando = true; }
        }
        texto.color = cor;
    }
}
