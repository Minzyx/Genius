using UnityEngine;
using System.Collections;

public class CameraTransition : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 targetEulerAngles;
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;

    public CanvasGroup fadePanel; // seu painel existente
    public float fadeSpeed = 1f;

    private bool moving = false;

    void Update()
    {
        // Inicia movimento no clique
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

            // Checa se chegou perto do alvo
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f &&
                Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                moving = false;
                StartCoroutine(FadePanel());
            }
        }
    }

    IEnumerator FadePanel()
    {
        fadePanel.alpha = 0;
        fadePanel.gameObject.SetActive(true);

        while (fadePanel.alpha < 1)
        {
            fadePanel.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Aqui você ativa ou mostra o menu de dificuldade
        Debug.Log("Menu de dificuldade pronto!");
    }
}
