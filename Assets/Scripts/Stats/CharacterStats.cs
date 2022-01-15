using UnityEngine;
using TMPro;

public class CharacterStats : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text damageText;
    public TMP_Text movementText;
    public TMP_Text dodgeText;

    EntityStats playerStats;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<EntityStats>();
    }

    private void Update()
    {
        healthText.text = "Health: " + player.GetComponent<EntityHealth>().GetHitpoints().ToString() 
            + "/" + playerStats.MaxHitpoints.ToString();

        damageText.text = "Weapon Damage: " + (player.GetComponent<Combat>().GetBaseWeaponDamage
            + playerStats.GetBuffAdditive(BuffType.AttackStrength)).ToString();

        movementText.text = "Movement Speed: " + playerStats.MovementSpeed.ToString();

        dodgeText.text = "Dodge Cooldown: " + playerStats.DodgeRechargeTime.ToString() + "s";

        if (ChipUI.selectedChip != null)
        {
            ChipBuff chipBuff = ChipUI.selectedChip.itemData.chipBuffs[0];

            switch (chipBuff.buffType)
            {
                case BuffType.Health:
                    healthText.text = healthText.text + " (+" + chipBuff.addiditiveValue + ")";
                    break;

                case BuffType.AttackStrength:
                    damageText.text = damageText.text + " (+" + chipBuff.addiditiveValue + ")";
                    break;

                case BuffType.MovementSpeed:
                    movementText.text = movementText.text + " (+" + chipBuff.addiditiveValue + ")";
                    break;

                case BuffType.DodgeRecharge:
                    dodgeText.text = dodgeText.text + " (" + chipBuff.addiditiveValue + "s)";
                    break;
            }
        }
    }
}
