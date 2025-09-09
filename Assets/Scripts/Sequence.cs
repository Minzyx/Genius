using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sequence : MonoBehaviour
{
    [Serializable]
    public class Item
    {
        [Tooltip("Botão colorido que mudará de cor.")]
        public GameObject Button;

        [Tooltip("Material padrão.")]
        public Material normalMaterial;

        [Tooltip("Material Neon.")]
        public Material neonMaterial;

        [HideInInspector] public Vector3 posInicial;
    }

    public enum Dificuldade { Facil, Medio, Dificil, Inifinito }

    [Header("Lista dos botões e texturas.")]
    public List<Item> items = new List<Item>();

    [Header("Configuração do Jogo")]
    public Dificuldade dificuldade = Dificuldade.Facil;

    [Header("Acertos")]
    [Min(0)] public int Acertos = 0;

    [Space(100)]
    [Header("UI")]
    public TextMeshProUGUI textoPontos;
    public TextMeshProUGUI textoRound;
    public GameObject WinSceneCanvas;
    public GameObject LoseSceneCanvas;
    public Button PlayGame;
    public GameObject PlayerCanvas;
    public TMP_Dropdown dropdown;

    private List<int> indices = new List<int>();
    private int ultimoIndice = -1;
    private int penultimoIndice = -1;

    private float normalTime = 1250;
    private int minTime = 350;
    private bool podeJogar = false;

    private int indiceJogador = 0;
    private int indiceHover = -1;

    [Header("Hover Config")]
    public float hoverAltura = 2.5f;
    public float scaleMultiplierIn = 0.95f;
    public float scaleMultiplierOut = 1.2f;
    public float hoverVelocidade = 10;

    private void Awake()
    {
        PlayerCanvas.SetActive(false);
        WinSceneCanvas.SetActive(false);
        LoseSceneCanvas.SetActive(false);

        foreach (var item in items)
            if (item.Button != null)
                item.posInicial = item.Button.transform.position;

        dropdown.onValueChanged.AddListener(SelectDificuldade);
    }

    void Update()
    {
        textoPontos.text = Acertos.ToString();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int novoHover = -1;

        if (Physics.Raycast(ray, out hit))
        {
            novoHover = items.FindIndex(x => x.Button == hit.transform.gameObject);
        }

        indiceHover = novoHover;

        for (int i = 0; i < items.Count; i++)
        {
            Vector3 alvo = items[i].posInicial;

            if (i == indiceHover && podeJogar)
                alvo += Vector3.up * hoverAltura;

            items[i].Button.transform.position =
                Vector3.Lerp(items[i].Button.transform.position, alvo, Time.deltaTime * hoverVelocidade);
        }

        if (!podeJogar)
            return;

        if (Input.GetMouseButtonDown(0) && indiceHover != -1)
        {
            int indiceClicado = indiceHover;
            StartCoroutine(FeedbackClique(indiceClicado));

            int esperado = indices[indiceJogador];
            if (indiceClicado == esperado)
            {
                indiceJogador++;

                if (indiceJogador >= indices.Count)
                {
                    podeJogar = false;
                    if (dificuldade == Dificuldade.Facil || dificuldade == Dificuldade.Inifinito)
                        Acertos++;
                    else if (dificuldade == Dificuldade.Medio)
                        Acertos += 2;
                    else if (dificuldade == Dificuldade.Dificil)
                        Acertos += 3;

                    if ((dificuldade == Dificuldade.Facil && indices.Count == 10) ||
                        (dificuldade == Dificuldade.Medio && indices.Count == 20) ||
                        (dificuldade == Dificuldade.Dificil && indices.Count == 30))
                    {
                        WinSceneCanvas.SetActive(true);
                        PlayGame.gameObject.SetActive(true);
                        dropdown.gameObject.SetActive(true);
                        var text = PlayGame.GetComponentInChildren<TextMeshProUGUI>();
                        text.text = "Reiniciar";
                        return;
                    }

                    RodarSequencia();
                }
            }
            else
            {
                textoPontos.gameObject.SetActive(false);
                LoseSceneCanvas.SetActive(true);
                PlayGame.gameObject.SetActive(true);
                dropdown.gameObject.SetActive(true);
                var text = PlayGame.GetComponentInChildren<TextMeshProUGUI>();
                text.text = "Tentar Novamente";
                podeJogar = false;
            }
        }
    }

    public void Jogar()
    {
        Acertos = 0;
        indices.Clear();
        ultimoIndice = -1;
        penultimoIndice = -1;
        normalTime = 1250;

        PlayerCanvas.SetActive(true);
        PlayGame.gameObject.SetActive(false);
        dropdown.gameObject.SetActive(false);
        WinSceneCanvas.SetActive(false);
        LoseSceneCanvas.SetActive(false);

        RodarSequencia();
    }
    
    public void SelectDificuldade(int indice)
    {
        switch (indice)
        {
            case 0:
                dificuldade = Dificuldade.Facil;
                break;
            case 1:
                dificuldade = Dificuldade.Medio;
                break;
            case 2:
                dificuldade = Dificuldade.Dificil;
                break;
            case 3:
                dificuldade = Dificuldade.Inifinito;
                break;

            default:
                dificuldade = Dificuldade.Facil;
                break;
        }
    }

    void RodarSequencia()
    {
        int novoIndice;
        int tentativas = 0;
        do
        {
            novoIndice = UnityEngine.Random.Range(0, items.Count);
            tentativas++;
        }
        while (((novoIndice == ultimoIndice && novoIndice == penultimoIndice) || novoIndice == ultimoIndice) && tentativas < 10);

        penultimoIndice = ultimoIndice;
        ultimoIndice = novoIndice;

        indices.Add(novoIndice);
        indiceJogador = 0;

        string qtd = dificuldade == Dificuldade.Facil ? "10" : dificuldade == Dificuldade.Medio ? "20" : dificuldade == Dificuldade.Dificil ? "30" : dificuldade == Dificuldade.Inifinito ? "∞" : "?";

        textoRound.text = $"{indices.Count} / {qtd}";

        StartCoroutine(RodarCores());
    }

    IEnumerator RodarCores()
    {
        yield return new WaitForSeconds(1f);

        if (indices.Count > 5)
            normalTime = Mathf.Max(minTime, (int)(normalTime * 0.85f));

        foreach (int i in indices)
        {
            var rend = items[i].Button.GetComponent<Renderer>();
            if (rend == null) continue;

            rend.material = items[i].neonMaterial;
            yield return new WaitForSeconds(normalTime / 1000f);

            rend.material = items[i].normalMaterial;
            yield return new WaitForSeconds(0.4f);
        }

        podeJogar = true;
    }

    IEnumerator FeedbackClique(int indice)
    {
        var rend = items[indice].Button.GetComponent<Renderer>();
        if (rend == null) yield break;

        Vector3 tamanhoOrigin = items[indice].Button.transform.localScale;
        Vector3 tamanhoPrimeiro = new Vector3(tamanhoOrigin.x * scaleMultiplierIn, tamanhoOrigin.y, tamanhoOrigin.z * scaleMultiplierIn);
        Vector3 tamanhoSegundo = new Vector3(tamanhoOrigin.x * scaleMultiplierOut, tamanhoOrigin.y, tamanhoOrigin.z * scaleMultiplierOut);

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * hoverVelocidade * 2;
            items[indice].Button.transform.localScale =
                Vector3.Lerp(tamanhoOrigin, tamanhoPrimeiro, t);
            yield return null;
        }

        rend.material = items[indice].neonMaterial;
        yield return new WaitForSeconds(0.15f);
        rend.material = items[indice].normalMaterial;

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * hoverVelocidade * 1.5f;
            items[indice].Button.transform.localScale =
                Vector3.Lerp(tamanhoPrimeiro, tamanhoSegundo, t);
            yield return null;
        }

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * hoverVelocidade;
            float elastic = Mathf.Sin(t * Mathf.PI * 0.5f);
            items[indice].Button.transform.localScale =
                Vector3.Lerp(tamanhoSegundo, tamanhoOrigin, elastic);
            yield return null;
        }
    }
}