using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject _bulletPrefab;
    private Transform _targetPlayer;

    private float _spawnDelay;

    public float _minDelay = 0.5f; 
    public float _maxDelay = 3.0f;
    private float _spawnRate;

    // Start is called before the first frame update
    void Start()
    {
        _targetPlayer = FindAnyObjectByType<PlayerController>().transform;
        _spawnDelay = 0f;
        _spawnRate = Random.Range(_minDelay, _maxDelay);
    }

    // Update is called once per frame
    void Update()
    {
        //Bulletspawn();

        _spawnDelay += Time.deltaTime;
        
        if (_spawnDelay >= _spawnRate)
        {
            _spawnDelay = 0f;

            GameObject bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);
            bullet.transform.LookAt(_targetPlayer);

            _spawnRate = Random.Range(_minDelay, _maxDelay);
        }
    }

    private void Bulletspawn()
    {

        //Random.Range(1f, 3f);

        //float Delay = Random.Range(minDelay, maxDelay);
        //Instantiate(_bulletPrefab, _targetPlayer.position, Quaternion.identity);

    }
}
