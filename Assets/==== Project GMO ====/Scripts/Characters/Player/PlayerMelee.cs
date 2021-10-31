using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] private Transform meleePos;
    [SerializeField] private float meleeRadius;
    [SerializeField] private float meleeWindUpTime;
    [SerializeField] private float meleeWindDownTime;

    [SerializeField] private int meleeDamage;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            MeleeExecution();
        }
    }

    private void Melee()
    {
        Collider[] hits = Physics.OverlapSphere(meleePos.position, meleeRadius);
        HashSet<ICanBeDamage> hitted = new HashSet<ICanBeDamage>();

        for (int i = 0; i < hits.Length; i++)
        {
            ICanBeDamage canBeDamage = hits[i].GetComponentInParent<ICanBeDamage>();

            if (canBeDamage != null)
            {
                if (hits[i].transform.parent != transform)
                {
                    if (!hitted.Contains(canBeDamage))
                    {
                        print(canBeDamage);
                        canBeDamage.ReceiveDamage(new Damage(meleeDamage, null));
                        hitted.Add(canBeDamage);
                    }
                }
            }
        }
    }

    private async void MeleeExecution()
    {
        float windUpTime = Time.time + meleeWindUpTime;

        while (Time.time < windUpTime)
        {
            await Task.Yield();
        }

        Melee();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleePos.position, meleeRadius);
    }
}
