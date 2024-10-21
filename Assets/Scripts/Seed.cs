using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Seed : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private GameObject plantZone;
    [SerializeField] private Sprite plant;
    private SpriteRenderer _spriteRenderer;
    public static Action<int> seedCounter;
    private GameObject _attacher;
    private PlayerController _player;
    private float timeToPlant;
    public static Action<float> increaseRegeneration;
    public static Action<bool> enableDragAndDrop;
    private float percentajeToIncrease;
    
    
    
    private States _state;
    public States State { get { return _state; } }
    
    public void Initialize()
    {
        _state = States.Spawned;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        percentajeToIncrease = _levelSettings.seedSettings.percentajeToRegenerationIncrease;
        timeToPlant = _levelSettings.seedSettings.timeToPlant;
    }

    public void Conclude()
    {
        seedCounter = null;
        increaseRegeneration = null;
        enableDragAndDrop = null;
        DragAndDrop.OnMouseUpAction -= StartPlant;
        StopAllCoroutines();
    }
    void Update()
    {
        switch (_state)
        {
            case States.Spawned:
                break;
            case States.AttachedToPlayer:
                transform.position = _attacher.transform.position;
                break;
            case States.Planted:
                break;
        }
    }
    private void OnEnable()
    {
        Vector3 newPos = new Vector3(Random.Range(-3f, 8f),Random.Range(-4.3f, 2f),0f);
        transform.position = newPos;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_state==States.Spawned && other.gameObject.CompareTag("Player") && !other.gameObject.GetComponent<PlayerController>().IsAttached)
        {
            _player = other.GetComponent<PlayerController>();
            _attacher = _player.Attach();
            _state = States.AttachedToPlayer;
            plantZone.GetComponent<PolygonCollider2D>().enabled = true;
        }

        if (_state == States.AttachedToPlayer && other.CompareTag("Terrain"))
        {
            DragAndDrop.OnMouseUpAction += StartPlant;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (_state == States.AttachedToPlayer && other.CompareTag("Terrain"))
        {
            DragAndDrop.OnMouseUpAction -= StartPlant;
        }
    }
    private void StartPlant()
    {
        plantZone.GetComponent<PolygonCollider2D>().enabled = false;
        _state = States.Planted;
        StartCoroutine(Plant());
    }
    IEnumerator Plant()
    {
        //Implementar que se active la animacion de plantar
        enableDragAndDrop?.Invoke(false);
        AnimController.Instance.PlayPickUpAnimation();
        yield return new WaitForSeconds(timeToPlant);
        _spriteRenderer.sprite = plant;
        AnimController.Instance.PlayIdleAnimation();
        seedCounter?.Invoke(1);
        DragAndDrop.OnMouseUpAction -= StartPlant;
        _player.IsAttached = false;
        enableDragAndDrop?.Invoke(true);
        increaseRegeneration?.Invoke(percentajeToIncrease);
    }

    public void SettingDestroyed()
    {
        seedCounter?.Invoke(-1);
        increaseRegeneration?.Invoke(-percentajeToIncrease);
        _state = States.Destroyed;
        gameObject.SetActive(false);
    }
}

public enum States
{
    Spawned,
    AttachedToPlayer,
    Planted,
    Destroyed
}
