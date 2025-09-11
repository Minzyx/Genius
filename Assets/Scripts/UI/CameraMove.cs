using UnityEngine;
using UnityEngine.UI;

public class CameraTransition : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 targetEulerAngles;
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;

    public Image fadePanel; // arraste o Panel preto aqui
    public float fadeSpeed = 1f;

    public CanvasGroup menuDificuldade; // arraste o Canvas do menu aqui
    public float menuFadeSpeed = 2f;

    private bool moving = false;
    private bool menuFading = false;

    void Start()
    {
        if (menuDificuldade != null)
        {
            menuDificuldade.alpha = 0;       // começa invisível
            menuDificuldade.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !moving)
        {
            moving = true;
        }

        if (moving)
        {
            // Movimento da câmera
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            // Fade junto com o movimento
            Color c = fadePanel.color;
            c.a = Mathf.MoveTowards(c.a, 1f, Time.deltaTime * fadeSpeed);
            fadePanel.color = c;

            // Quando terminou de escurecer
            if (c.a >= 1f)
            {
                moving = false;
                ShowMenu();
            }
        }

        // Fade-in do menu
        if (menuFading && menuDificuldade.alpha < 1f)
        {
            menuDificuldade.alpha += Time.deltaTime * menuFadeSpeed;
        }
    }

    void ShowMenu()
    {
        if (menuDificuldade != null)
        {
            menuDificuldade.gameObject.SetActive(true);
            menuFading = true;
        }

        Debug.Log("Menu de dificuldade exibido com fade-in!");
    }
}
