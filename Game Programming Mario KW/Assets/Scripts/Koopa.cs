using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;
    public float shellSpeed = 12f;

    public bool inShell;
    public bool shellPush;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!inShell && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player.starpower)
            {
                Hit();
            }
            else if(collision.transform.DotTest(transform, Vector2.down))
            {
                EnterShell();
            }
            else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(inShell && other.CompareTag("Player")){
            if(!shellPush){
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);
            }
            else{
                Player player = other.GetComponent<Player>();
                player.Hit();
            }
        }
        else if(!inShell && other.gameObject.layer == LayerMask.NameToLayer("Shell")){
            Hit();
        }
        if (other.CompareTag("Fireball"))
        {
            Hit();
        }
    }

    private void EnterShell()
    {
        inShell = true;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<EntityMovements>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = shellSprite;
    }
    private void PushShell(Vector2 direction)
    {
        shellPush = true;
        ScoreManager.instance.AddScore(100);
        GetComponent<Rigidbody2D>().isKinematic = false;

        EntityMovements movement = GetComponent<EntityMovements>();
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void Hit(){
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        ScoreManager.instance.AddScore(300);
        Destroy(gameObject, 3f);
    }

    /*private void OnBecameInvisible(){
        if(shellPush){
            Destroy(gameObject);
        }
    }*/
}
