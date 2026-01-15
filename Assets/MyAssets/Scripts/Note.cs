using UnityEngine;

public class Note : MonoBehaviour, IInteractable
{
    [TextArea(2, 5)]
    public string noteTitle;
    [TextArea(5, 10)]
    public string[] pages;

    public void Interact()
    {
        if (pages == null || pages.Length == 0)
        {
            Debug.LogWarning("This note don't have pages.");
            return;
        }

        ShowPagesInConsole();
        NoteReader.instance.Open(pages, noteTitle);
    }

    private void ShowPagesInConsole()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            Debug.Log($"Página {i + 1}/{pages.Length}: {pages[i]}");
        }
    }
}
