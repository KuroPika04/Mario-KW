using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;
    //public Koopa koopa;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player.starpower)
            {
                Hit();
            }
            else if(collision.transform.DotTest(transform, Vector2.down))
            {
                Flatten();
            }
            else
            {
                player.Hit();
            }
        }
        /*if (collision.gameObject.CompareTag("Shell"))
        {
            Koopa koopa = collision.gameObject.GetComponent<Koopa>();
            if (koopa.inShell)
            {
                Debug.Log("Hit by Shell");
                Hit();
            }
        }*/
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
        if (other.CompareTag("Fireball"))
        {
            Hit();
        }
    }

    private void Flatten()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EntityMovements>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite;
        ScoreManager.instance.AddScore(200);
        Destroy(gameObject, 0.3f);
    }

    private void Hit(){
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        ScoreManager.instance.AddScore(300);
        Destroy(gameObject, 3f);
    }
}
