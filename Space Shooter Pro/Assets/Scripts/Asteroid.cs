using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 30.0f;

    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;

    private AudioSource _explosionAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _explosionAudioSource = GameObject.Find("/Audio_Manager/Explosion").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(_rotateSpeed * Time.deltaTime * Vector3.forward);
    }

    //check for laser collision
    // instantiate explosion at the position of asteroid(us)
    // destroy the explosion after 3 secs

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab,transform.position,Quaternion.identity);
            if (_explosionAudioSource != null)
            {
                _explosionAudioSource.Play(); 
            }
            //gameObject.SetActive(false);
            //Destroy(explosionInstance, 3.0f);
            _spawnManager.StartSpawning();
            Destroy(gameObject, 0.2f);
        }
    }
}
