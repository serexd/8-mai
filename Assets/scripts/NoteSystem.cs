using UnityEngine;
using UnityEngine.UI;


public class NoteSystem : MonoBehaviour
{
    public GameObject NoteIteself;
    public GameObject Note_GameObject;
    public Text NoteText;

    [TextArea]
    public string noteContent;

    public AudioClip Bruit;
    public AudioSource source;
    public AudioClip PaperSound;

    public Text InteractionText;

    private bool canInteract = false;
    private GameObject currentPaper;

    void Start()
    {
        Note_GameObject.SetActive(false);
        InteractionText.text = "";
        SetCursor(false);
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            TakeNote();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Paper"))
        {
            canInteract = true;
            currentPaper = other.gameObject;

            InteractionText.text = "Press E to take the object";
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Paper"))
        {
            canInteract = false;
            currentPaper = null;

            InteractionText.text = "";
        }
    }

    void TakeNote()
    {
        canInteract = false;

        if (currentPaper != null)
        {
            currentPaper.SetActive(false);
        }

        Note_GameObject.SetActive(true);

        NoteText.text = noteContent;

        source.PlayOneShot(PaperSound);

        InteractionText.text = "";

        SetCursor(true);
    }

    public void CloseNote()
    {
        NoteIteself.SetActive(false);
        Note_GameObject.SetActive(false);
        NoteText.text = "";

        source.PlayOneShot(Bruit);

        SetCursor(false);
    }

    void SetCursor(bool active)
    {
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = active;
    }
}
