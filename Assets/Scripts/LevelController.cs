using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private CowController _cowController;

    [SerializeField] private TerrainController _terrainController;

    [SerializeField] private SeedController _seedController;

    [SerializeField] private PlayerController _playerController;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        _cowController.Initialize();
        _seedController.Initialize();
        _playerController.Initialize();
        _terrainController.Initialize();
    }

    public void Conclude()
    {
        _cowController.Conclude();
        _seedController.Conclude();
        _playerController.Conclude();
        _terrainController.Conclude();
    }
}
