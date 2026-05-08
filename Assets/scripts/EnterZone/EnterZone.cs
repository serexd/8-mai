using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterZone : MonoBehaviour
{
    public GameObject messageUI;
    private bool playerInZone = false;

    void Start()
    {
        messageUI.SetActive(false);
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("EntryThree");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            messageUI.SetActive(true);


        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            messageUI.SetActive(false);
        }
    }
}
