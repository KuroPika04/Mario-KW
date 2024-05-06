using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    private PlayerSpriteRenderer activeRenderer;

    private DeathAnimation deathAnimation;
    private CapsuleCollider2D capsuleCollider;

    public bool big => bigRenderer.enabled;
    public bool small => smallRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    public bool starpower { get; private set; }
    public bool jumper_mushroom { get; private set; }
    public bool fire_flower { get; private set; }

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        activeRenderer = smallRenderer;
    }

    public void Hit()
    {
        if (!dead && !starpower)
        {
            if (big)
            {
                Shrink();
            }
            else
            {
                Death();
            }
        }
    }

    public void Grow()
    {
        ScoreManager.instance.AddScore(200);
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(ScaleAnimation());
    }

    private void Shrink()
    {
        fire_flower = false;
        bigRenderer.enabled = false;
        smallRenderer.enabled = true;
        activeRenderer = smallRenderer;
        if (!jumper_mushroom)
        {
            activeRenderer.spriteRenderer.color = Color.white;
        }
        capsuleCollider.size = new Vector2(0.75f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);
        StartCoroutine(ScaleAnimation());
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if(Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;
        }
        
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

    public void StarPower()
    {
        StartCoroutine(StarPowerAnimation());
    }

    private IEnumerator StarPowerAnimation()
    {
        ScoreManager.instance.AddScore(200);
        starpower = true;

        float elapsed = 0f;
        float duration = 10f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;
        starpower = false;
    }
    public void JumperMushroom()
    {
        ScoreManager.instance.AddScore(300);
        StartCoroutine(JumperMushroomEffect());
    }
    private IEnumerator JumperMushroomEffect()
    {
        Debug.Log("JumperMushroom Active");
        jumper_mushroom = true;
        Color originalColor = activeRenderer.spriteRenderer.color;
        float timer = 0f;
        float duration = 15f;
        bool ColorChanging = false;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            activeRenderer.spriteRenderer.color = ColorChanging ? originalColor : Color.red;
            ColorChanging = !ColorChanging;
            yield return null;
        }
        if (fire_flower)
        {
            activeRenderer.spriteRenderer.color = Color.green;
        }
        else
        {
            activeRenderer.spriteRenderer.color = originalColor;
        }
        jumper_mushroom = false;
        Debug.Log("JumperMushroom Inactive");
    }
    public void FireFlower()
    {
        ScoreManager.instance.AddScore(300);
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        FireFlowerEffect();
        StartCoroutine(ScaleAnimation());
        
    }
    private void FireFlowerEffect()
    {
        Debug.Log("FireFlower Active");
        fire_flower = true;
        activeRenderer.spriteRenderer.color = Color.green;
    }

    private void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }
}
