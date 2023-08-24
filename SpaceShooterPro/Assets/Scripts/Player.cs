using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour

{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private bool _isPlayerOne;
    


    private AudioSource _audioSource;
    private float _speedMultiplier = 1f;
    private SpawnManager _spawnManager;
    private float _canFire = -1f;
    private bool _isTripleShotActive = false;
    private bool _isShielding = false;
    private UIManager _uiManager;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager.isCoopMode == false)
        {
            transform.position = new Vector3(0, 0, 0);
        }
        else
        { }

        if (_spawnManager == null)
        {
            Debug.Log("spawn manager is null");
        }
        if (_uiManager == null)
        {
            Debug.Log("ui manager is null");
        }

        _audioSource.clip = _laserSoundClip;
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();


        if (((Input.GetKey(KeyCode.Space) && _isPlayerOne) || (Input.GetMouseButton(0)) && !_isPlayerOne) && Time.time > _canFire)
        {
            ShootLaser();
        }
    }


    private void CalculateMovement()
    {

        float horizontalInput; 
        float verticalinput;

        if (_isPlayerOne)
        {
            horizontalInput = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
            verticalinput = Input.GetKey(KeyCode.S) ? -1 : Input.GetKey(KeyCode.W) ? 1 : 0;
        }
        else
        {
            horizontalInput = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
            verticalinput = Input.GetKey(KeyCode.DownArrow) ? -1 : Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
        }
       
        transform.Translate(_speed * Time.deltaTime * new Vector3(horizontalInput, verticalinput, 0) * _speedMultiplier);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        } 
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

    }

    private void ShootLaser()
    {
        _canFire = Time.time + _fireRate;

        _audioSource.Play();

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
        }
           
    }

    public void Damage()
    {
        if (_isShielding)
        {
            _isShielding = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        --_lives;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            if (_spawnManager != null)
                _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive= true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive= false;
    }

    public void SpeedBoostActive()
    {
        _speedMultiplier = 2f;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _speedMultiplier = 1f;
    }

    public void ShieldActive()
    {
        _isShielding = true;
        _shieldVisualizer.SetActive(true);
    }
}
