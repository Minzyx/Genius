using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneStartFade : MonoBehaviour
{
    [Header("UI Fade")]
    public Image fadePanel;                 // arrasta aqui o Panel preto (Image que cobre a tela)
    [Range(0f, 1f)] public float targetAlpha = 0.5f; // alpha final (ex.: 0.5 = meio transparente)
    public float fadeSpeed = 1f;            // quanto maior, mais rápido
    public float startDelay = 0f;           // espera antes de começar o fade (segundos)
    public bool startAutomatically = true;  // se false, chame StartFade() manualmente

    void Awake()
    {
        // garante que começa todo preto
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 1f;
            fadePanel.color = c;
        }
    }

    IEnumerator Start()
    {
        if (startAutomatically)
        {
            if (startDelay > 0f) yield return new WaitForSeconds(startDelay);
            yield return StartCoroutine(FadeToTarget());
        }
    }

    // método público caso queira disparar de outro script
    public void StartFade()
    {
        StartCoroutine(FadeToTarget());
    }

    IEnumerator FadeToTarget()
    {
        if (fadePanel == null) yield break;

        Color c = fadePanel.color;
        // usa MoveTowards pra não travar por precisão
        while (c.a > targetAlpha + 0.001f)
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
            fadePanel.color = c;
            yield return null;
        }
    }
}
