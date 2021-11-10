using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDamage : MonoBehaviour
{
    [SerializeField] float damageRate = 1f;
    [SerializeField] int damage = 1;

    //Keeps track of entities touching
    //Applies damage every set interval
    Dictionary<Health, float> targetsInContact = new Dictionary<Health, float>();

    //Add new target in contact with cactus
    private void OnTriggerEnter(Collider other)
    {
        Health target = other.gameObject.GetComponentInParent<Health>();
        if (target == null) return;

        if (!targetsInContact.ContainsKey(target))
        {
            targetsInContact[target] = damageRate; //Damage immediately in update()
        }
    }

    //Remove target in contact with cactus
    private void OnTriggerExit(Collider other)
    {
        Health target = other.gameObject.GetComponentInParent<Health>();
        if (target == null) return;

        if (targetsInContact.ContainsKey(target))
        {
            targetsInContact.Remove(target);
        }
    }

    private void Update()
        {
            Health[] targets = new Health[targetsInContact.Count];
            targetsInContact.Keys.CopyTo(targets, 0);

            foreach (var target in targets)
            {
                if (target.IsDead()) //Validate target
                {
                    targetsInContact.Remove(target);
                    continue;
                }

                targetsInContact[target] += Time.deltaTime;
                if (targetsInContact[target] >= damageRate) //Damage enemy again if contact is sustained for set damage rate
                {
                    target.TakeDamage(damage);
                    targetsInContact[target] = 0f;
                }
            }
        }
}