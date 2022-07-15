using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private Character[] _characters;
    [SerializeField] private float _nextTurnDelay = 1f;

    private int _curCharacterindex = -1;
    public Character CurrentCharacter;

    public event UnityAction<Character> OnBeginTurn;
    public event UnityAction<Character> OnEndTurn;

    public static TurnManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void OnEnable()
    {
        Character.OnDie += OnCharcterDie;
    }

    private void OnDisable()
    {
        Character.OnDie -= OnCharcterDie;
    }

    private void Start()
    {
        BeginNextTurn();
    }

    public void BeginNextTurn()
    {
        _curCharacterindex++;
        if (_curCharacterindex == _characters.Length)
            _curCharacterindex = 0;

        CurrentCharacter = _characters[_curCharacterindex];
        OnBeginTurn?.Invoke(CurrentCharacter);
    }

    public void EndTurn()
    {
        OnEndTurn?.Invoke(CurrentCharacter);
        Invoke(nameof(BeginNextTurn), _nextTurnDelay);
    }

    private void OnCharcterDie(Character character)
    {
        if (character.IsPlayer)
            Debug.Log("You lost!");
        else
            Debug.Log("You win!");
    }
}
