using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private int _powerupID = 0;
    [SerializeField]
    private AudioClip _audioClip;

  
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Debug.Log("Kill Powerup");
            Destroy(this.gameObject);        
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (collision.transform.TryGetComponent<Player>(out var player))
            {

                AudioSource.PlayClipAtPoint(_audioClip, transform.position);

                switch (_powerupID)
                {
                case 0: player.TripleShotActive(); break;
                case 1: player.SpeedBoostActive(); break;
                case 2: player.ShieldActive();  break;  
                default: break;

                }
               
            }
            Destroy(this.gameObject);
        }
    }
}
