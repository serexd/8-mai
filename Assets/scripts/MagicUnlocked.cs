using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MagicUnlocked : MonoBehaviour
{
    public PlatformAbility platformAbility;

    public Text interactionText;

    public AudioSource source;
    public AudioClip unlockSound;

    private bool canInteract = false;

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            UnlockAbility();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;

            interactionText.text = "Press E to take the object";
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;

            interactionText.text = "";
        }
    }

    void UnlockAbility()
    {
        // Active le script
        platformAbility.enabled = true;

        // Son
        source.PlayOneShot(unlockSound);

        // Message tutoriel
        StartCoroutine(ShowTutorial());

        // Détruit l'objet ramassé
        Destroy(gameObject);
    }

    IEnumerator ShowTutorial()
    {
        interactionText.text = "Jump + Shift to create a platform";

        yield return new WaitForSeconds(4f);

        interactionText.text = "";
    }
}  
