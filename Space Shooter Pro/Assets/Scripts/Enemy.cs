using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;

    private Animator _enemyAnimator;

    private AudioSource _explosionAudioSource;

    private float _fireRate = 3.0f;

    private float _canFire = -1;

    //handle to animator

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        _player = playerObj.transform.GetComponent<Player>();
        _explosionAudioSource = GameObject.Find("/Audio_Manager/Explosion").GetComponent<AudioSource>();
        if(_player == null)
        {
            Debug.Log("Player not found");
        }
        _enemyAnimator = transform.GetComponent<Animator>();

        if(_enemyAnimator == null)
        {
            Debug.Log("Enemy Animator null");
        }
        //assign anim
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            //lasers[0].AssignEnemyLaser();
            //lasers[1].AssignEnemyLaser();

            for (int i= 0;i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7.0f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Laser"))
        {            
            if(_player != null)
            {
                _player.AddScore(10);
            }
            if (!other.gameObject.GetComponent<Laser>().IsEnemyLaser)
            {
                Destroy(other.gameObject);
                _enemyAnimator.SetTrigger("OnEnemyDeath");
                _speed = 0;
                Destroy(GetComponent<Collider2D>());
                Destroy(gameObject, 2.8f);
                if (_explosionAudioSource != null)
                {
                    _explosionAudioSource.Play();
                } 
            }
        }
        else if(other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (null != player)
            {
                player.Damage();
            }
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
            if (_explosionAudioSource != null)
            {
                _explosionAudioSource.Play();
            }
        }
    }
}
