using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [Header("Configs")]
    public float sensibilidade = 0.05f;
    public float suavidade = 5f;
    public float limite = 2f;

    private Quaternion rotacaoInicial;

    void Start()
    {
        rotacaoInicial = transform.rotation;
    }

    void Update()
    {
        float mouseX = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float mouseY = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;

        Quaternion alvo = rotacaoInicial * Quaternion.Euler(-mouseY * limite * sensibilidade, mouseX * limite * sensibilidade, 0f);

        transform.rotation = Quaternion.Lerp(transform.rotation, alvo, Time.deltaTime * suavidade);
    }
}
