using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attack_cooldown;
    private float cooldown_timer = 100;
    [SerializeField] private Transform firepoint;
    public GameObject elements;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && cooldown_timer > attack_cooldown && player.fire_flower)
        {
            attack();
        }
        cooldown_timer += Time.deltaTime;
    }
    private void attack()
    {
        cooldown_timer = 0;
        Instantiate(elements, firepoint.position, firepoint.rotation);
    }
}