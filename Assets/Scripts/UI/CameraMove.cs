using UnityEngine;
using UnityEngine.UI;

public class CameraTransition : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 targetEulerAngles;
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;

    public Image fadePanel; // arraste o Panel aqui
    public float fadeSpeed = 1f;

    private bool moving = false;

    void Update()
    {
        // Quando clicar, começa a mover + fade
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

            // Quando a câmera chega no alvo, para o movimento
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f &&
                Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                moving = false;
                Debug.Log("Transição concluída → chamar menu de dificuldade aqui");
            }
        }
    }
}
