using UnityEngine;
using TMPro;

public class CharacterStats : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text damageText;
    public TMP_Text movementText;

    EntityStats playerStats;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<EntityStats>();
    }

    private void Update()
    {
        healthText.text = "Health: " + player.GetComponent<EntityHealth>().Hitpoints.ToString() 
            + "/" + playerStats.MaxHitpoints.ToString();

        damageText.text = "Weapon Damage: " + (player.GetComponent<Combat>().GetBaseWeaponDamage
            + playerStats.GetBuffAdditive(BuffType.AttackStrength)).ToString();

        movementText.text = "Movement Speed: " + playerStats.MovementSpeed.ToString();
    }
}
