using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoteReader : MonoBehaviour
{
    public static NoteReader instance;
    
    [Header("UI")]
    public GameObject panel;
    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI pageText;
    public TextMeshProUGUI pageCounterText;

    [Header("Player Control")] 
    public PlayerController playerController;

    public CameraController cameraController;

    private string _noteTitle;
    private string[] _pages;
    private int _currentPage;
    private bool _isOpen;

    public GameObject nextButton;
    public GameObject previousButton;
    
    [Header("Page Input")]
    [SerializeField] private float pageInputCooldown = 0.25f;

    private float _lastPageInputTime;

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

    public void Open(String[] pages, String noteTitle)
    {
        gameObject.SetActive(true);
        //if (_isOpen) return;
        
        _noteTitle = noteTitle;
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
        _noteTitle = null;
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
        bool isPreviousButtonActive = (_currentPage == 0) ? false : true;
        previousButton.SetActive(isPreviousButtonActive);
        
        bool isNextButtonActive = (_currentPage == _pages.Length-1) ? false : true;
        nextButton.SetActive(isNextButtonActive);
        
        textTitle.text = _noteTitle;
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

    public bool IsOpen()
    {
        return _isOpen;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!_isOpen) return;
        if (!context.performed) return;
        
        if (Time.time - _lastPageInputTime < pageInputCooldown)
            return;

        Vector2 input = context.ReadValue<Vector2>();

        if (input.x > 0.5f)
        {
            NextPage();
            _lastPageInputTime = Time.time;
        }
        else if (input.x < -0.5f)
        {
            PrevioudPage();
            _lastPageInputTime = Time.time;
        }
    }
}
