using System;
using TMPro;
using UnityEngine;

public class NoteReader : MonoBehaviour
{
    public static NoteReader instance;
    
    [Header("UI")]
    public GameObject panel;
    public TextMeshProUGUI pageText;
    public TextMeshProUGUI pageCounterText;

    [Header("Player Control")] 
    public PlayerController playerController;

    public CameraController cameraController;
    
    private string[] _pages;
    private int _currentPage;
    private bool _isOpen;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        gameObject.SetActive(false);
        panel.SetActive(false);
    }

    public void Open(String[] pages)
    {
        gameObject.SetActive(true);
        //if (_isOpen) return;
        
        _pages = pages;
        _currentPage = 0;
        _isOpen = true;
        
        panel.SetActive(true);
        UpdatePage();

        LockPlayer(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        //if (!_isOpen) return;
        
        panel.SetActive(false);
        _pages = null;
        _isOpen = false;

        LockPlayer(false);
    }

    public void NextPage()
    {
        if (!_isOpen) return;
        
        if (_currentPage < _pages.Length - 1)
        {
            _currentPage++;
            UpdatePage();
        }
    }

    public void PrevioudPage()
    {
        if (!_isOpen) return;
        
        if (_currentPage > 0)
        {
            _currentPage--;
            UpdatePage();
        }
    }

    private void UpdatePage()
    {
        pageText.text = _pages[_currentPage];
        pageCounterText.text = $"{_currentPage + 1} / {_pages.Length}";
    }

    private void LockPlayer(bool lockPlayer)
    {
        playerController.enabled = !lockPlayer;
        cameraController.enabled = !lockPlayer;
        
        Cursor.visible = lockPlayer;
        Cursor.lockState = lockPlayer ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
