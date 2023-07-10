using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float  _normalSpeed = 3.5f;

    [SerializeField]
    private float _speedMultiplier = 2.0f;
    
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;

    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    [SerializeField]
    private bool _isTripleShotActive = false;

    [SerializeField]
    private bool _isShieldsActive = false;

    [SerializeField]
    private GameObject _playerShield;

    [SerializeField]
    private int _score;

    [SerializeField]
    private GameObject[] _engines; //0 = right, 1=  left
    private int _otherEngineNumber;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject _laserAudio;

    AudioSource _laserAudioSource;

    [SerializeField]
    private GameObject _explosionAudio;

    AudioSource _explosionAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -3.9f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _speed = _normalSpeed;
        _playerShield.SetActive(false);
        _laserAudioSource = _laserAudio.GetComponent<AudioSource>();
        _explosionAudioSource = _explosionAudio.GetComponent<AudioSource>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

#if UNITY_ANDROID
        if ((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire")) && Time.time > _canFire)
        {
            FireLaser();
        }
#elif UNITY_IOS
        if ((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire")) && Time.time > _canFire)
        {
            FireLaser();
        }
#else
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) && Time.time > _canFire)
        {
            FireLaser();
        }
#endif
    }

    private void FireLaser()
    {      
        _canFire = Time.time + _fireRate;

        if(_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity); 
        }    

        if(_laserAudioSource == null)
        {
            Debug.LogError("Laser Audio Source is null");
        }
        else
        {
            _laserAudioSource.Play();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal"); //Input.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical"); //Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(_speed * Time.deltaTime * direction);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);


        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.2f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.2f, transform.position.y, 0);
        }
    }
    public void Damage()
    {
        if(_isShieldsActive)
        {
            _isShieldsActive = false;
            _playerShield.SetActive(false);
            return;
        }
        _lives--;
        _uiManager.UpdateLives(_lives);
        switch (_lives)
        {
            case 2:
                {
                    int engineNumber = Random.Range(0, 2);
                    _otherEngineNumber = 1 - engineNumber;
                    _engines[engineNumber].SetActive(true);
                    break;
                }

            case 1:
                _engines[_otherEngineNumber].SetActive(true);
                break;
            case 0:
                _spawnManager.OnPlayerDeath();
                Destroy(gameObject);
                _explosionAudioSource.Play();
                break;
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        IEnumerator powerDownCoroutine = TripleShotPowerDown();
        StartCoroutine(powerDownCoroutine);
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void ActivateSpeed()
    {
        _speed = _normalSpeed * _speedMultiplier;
        StartCoroutine(SpeedPowerDown());
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = _normalSpeed;
    }

    public void ActivateShields()
    {
        _isShieldsActive = true;
        _playerShield.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += 10;
        _uiManager.UpdateScoreUI(_score);
    }
}
