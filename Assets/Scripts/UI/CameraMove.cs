using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraTransition : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 targetEulerAngles;
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;

    public Image fadePanel; // arraste o Panel preto aqui
    public float fadeSpeed = 1f;

    public string nextSceneName = "SampleScene";

    private bool moving = false;


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

            // Quando a tela ficar toda preta, troca de cena
            if (Mathf.Approximately(c.a, 1f))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }

        
    }

   

}
