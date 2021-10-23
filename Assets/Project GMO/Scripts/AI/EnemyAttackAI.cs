using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AttackRangeType
{
    MELEE,
    RANGED
}

[RequireComponent(typeof(EnemyMovementAI))]
public class EnemyAttackAI : MonoBehaviour
{
    private EnemyMovementAI movementAI;
    public NavMeshAgent agent { get => movementAI.agent; private set { } }

    public Transform target { get; private set; }

    [SerializeField] private AttackRangeType attackRangeType;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private float attackDuration;
    public float AttackDuration { get => attackDuration; private set { } }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private Transform shootPoint;

    [SerializeField] private MeshRenderer mesh;

    private void Start()
    {
        movementAI = GetComponent<EnemyMovementAI>();
    }

    private void Update()
    {
        target = movementAI.target;
    }
    public void Attack()
    {
        Shoot();
        AttackAnim();
    }

    public void Shoot()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        bulletInstance.GetComponent<Rigidbody>().AddForce(transform.forward * bulletVelocity);

        Destroy(bulletInstance, 2.0f);
    }
    public void AttackAnim()
    {
        mesh.material.color = Color.red;
        StartCoroutine(ResetColor());
    }

    public bool IsWithinAttackRange()
    {
        bool inRange = false;

        if (target)
        {
            if (Vector3.Distance(transform.position, target.position) < attackRange)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleLayer))
                {
                    inRange = true;
                }
            }
        }

        return inRange;
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.5f);
        mesh.material.color = Color.white;
    }
}
