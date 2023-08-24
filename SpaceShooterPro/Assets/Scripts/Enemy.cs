using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _laserPrefab;

    private float _fireRate = 3f;
    private float _canFire = -1;
    
   // private Player _player;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private AudioSource _audioSource;
    private UIManager _uiManager;



    void Start()
    {
       // _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _boxCollider= GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_animator == null)
        {
            Debug.Log("Animator is null");
        }

        if (_uiManager == null)
        {
            Debug.Log("ui manager is null");
        }

        //if (_player == null)
        //{
        //    Debug.Log("player is null");
        //}

        if (_boxCollider == null)
        {
            Debug.Log("box collider is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
           _fireRate = Random.Range(3f, 7f);
           _canFire = Time.time + _fireRate;
           GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
           Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            foreach (Laser laser in lasers)
            {
                laser.SetIsEnemyLaser(true);
            }
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            transform.position = new Vector3(Random.Range(-8f, 8), 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _speed = 0f;
            other.gameObject.GetComponent<Player>().Damage();
            _animator.SetTrigger("onEnemyDeath");
           _boxCollider.enabled = false;
            _audioSource.Play();
            Destroy(this.gameObject,2.4f);
        }

        if (other.CompareTag("Laser"))
        {
            if (!other.GetComponent<Laser>().GetIsEnemyLaser())
            {
                
                _uiManager.UpdateScore(10);
                _speed = 0f;
                _animator.SetTrigger("onEnemyDeath");
                _boxCollider.enabled = false;
                _audioSource.Play();
                Destroy(this.gameObject, 2.4f);
                Destroy(other.gameObject); 
            }
        }
    }
}
