using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, GameInput.IGameSceneInputActions
{
    public static InputManager Instance;

    private GameInput gameInput;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        gameInput = new GameInput();
        gameInput.Enable();
        gameInput.GameSceneInput.SetCallbacks(this);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GameInput.IGameSceneInputActions.OnTogglePauseMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        GameManager.Instance.TogglePauseMenu();
    }
}
