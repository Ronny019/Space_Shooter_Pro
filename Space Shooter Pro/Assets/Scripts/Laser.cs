using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    private bool _isEnemyLaser = false;

    public bool IsEnemyLaser { get => _isEnemyLaser; set => _isEnemyLaser = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {      
        if(_isEnemyLaser)
        {
            MoveDown();
        }
        else
        {
            MoveUp();
        }
        
    }


    private void MoveDown()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
        if (transform.position.y < -8.0f)
        {
            Transform laserParentTransform = transform.parent;
            if (laserParentTransform != null)
            {
                Destroy(laserParentTransform.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }

    }
    private void MoveUp()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.up);
        if (transform.position.y > 8.0f)
        {
            Transform laserParentTransform = transform.parent;
            if (laserParentTransform != null)
            {
                Destroy(laserParentTransform.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
            Destroy(gameObject);
        }
        if (other.CompareTag("Laser"))
        {
            Laser myLaser = gameObject.GetComponent<Laser>();
            Laser otherLaser = other.gameObject.GetComponent<Laser>();
            if ((myLaser.IsEnemyLaser && !otherLaser.IsEnemyLaser) || (!myLaser.IsEnemyLaser && otherLaser.IsEnemyLaser))
            {
                Destroy(gameObject);
                Destroy(other.gameObject);
            }
        }
    }
}
