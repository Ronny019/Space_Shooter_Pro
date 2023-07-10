using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;

    //ID for Powerups
    // 0 = TripleShot
    // 1 = Speed
    // 2 = Shields
    [SerializeField]
    private int _powerUpId; // 0 = TripleShot, 1 = Speed, 2 = Shields

    private AudioSource _powerUpAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        _powerUpAudioSource = GameObject.Find("/Audio_Manager/Powerup").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
        if(transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (other.CompareTag("Player"))
        {
            switch (_powerUpId)
            {
                case 0:
                    player.ActivateTripleShot();
                    break;
                case 1:
                    player.ActivateSpeed();
                    break;
                case 2:
                    player.ActivateShields();
                    break;
                default:
                    Debug.Log("Default Statement");
                    break;
            }
            _powerUpAudioSource.Play();
            Destroy(gameObject);
        }
    }
}
