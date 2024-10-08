using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Seed : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private GameObject plantZone;
    private GameObject _attacher;
    private PlayerController _player;
    private float timeToPlant;
    public static Action<float> increaseRegeneration;
    public static Action<bool> enableDragAndDrop;
    private float percentajeToIncrease;
    
    
    private States _state;
    private enum States{
        Spawned,
        AttachedToPlayer,
        Planted
    }
    public void Initialize()
    {
        _state = States.Spawned;
        percentajeToIncrease = _levelSettings.seedSettings.percentajeToRegenerationIncrease;
        timeToPlant = _levelSettings.seedSettings.timeToPlant;
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
            plantZone.GetComponent<BoxCollider2D>().enabled = true;
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
        plantZone.GetComponent<BoxCollider2D>().enabled = false;
        _state = States.Planted;
        StartCoroutine(Plant());
    }
    IEnumerator Plant()
    {
        //Implementar que se active la animacion de plantar
        enableDragAndDrop?.Invoke(false);
        yield return new WaitForSeconds(timeToPlant);
        DragAndDrop.OnMouseUpAction -= StartPlant;
        _player.IsAttached = false;
        enableDragAndDrop?.Invoke(true);
        increaseRegeneration?.Invoke(percentajeToIncrease);
    }
}
