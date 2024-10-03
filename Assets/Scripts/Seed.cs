using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    private GameObject _player;
    private States _state;
    private enum States{
        Spawned,
        AttachedToPlayer,
        Planted
    }
    // Start is called before the first frame update
    void Start()
    {
        _state = States.Spawned;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case States.Spawned:
                break;
            case States.AttachedToPlayer:
                transform.position = _player.transform.position;
                break;
            case States.Planted:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_state==States.Spawned && other.gameObject.CompareTag("Player"))
        {
            print("SEED: Me toco el jugador");
            _player = other.GetComponent<PlayerController>().attacher;
            _state = States.AttachedToPlayer;
        }

        if (_state == States.AttachedToPlayer && other.gameObject.CompareTag("Terrain"))
        {
            _state = States.Planted;
        }
    }
}
