using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OwnerSide : int
{
    Player = 0,
    Enemy,
}

public class Bullet : MonoBehaviour
{
    const float LifeTime = 15.0f;

    OwnerSide ownerSide = OwnerSide.Player;

    public Vector3 MoveDirection = Vector3.zero;

    public float Speed = 0.0f;

    bool NeedMove = false;

    bool Hited = false;

    float FiredTime;

    public int Damage = 1;

    Actor Owner;

    public string FilePath
    {
        get;
        set;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ProcessDisappearCondition())
            return;

        UpdateMove();
    }

    void UpdateMove()
    {
        if (!NeedMove)
            return;

        Vector3 moveVector = MoveDirection.normalized * Speed * Time.deltaTime;
        moveVector = AdjustMove(moveVector);
        transform.position += moveVector;
    }

    public void Fire(OwnerSide FireOwner, Vector3 firePosition, Vector3 direction, float speed, int damage)
    {
        ownerSide = FireOwner;
        transform.position = firePosition;
        MoveDirection = direction;
        Speed = speed;
        Damage = damage;

        NeedMove = true;
        FiredTime = Time.time;
    }

    Vector3 AdjustMove(Vector3 moveVector)
    {
        RaycastHit hitInfo;

        if(Physics.Linecast(transform.position, transform.position + moveVector, out hitInfo))
        {
            Actor actor = hitInfo.collider.GetComponentInParent<Actor>();
            if (actor && actor.IsDead)
                return moveVector;

            moveVector = hitInfo.point - transform.position;
            OnBulletCollision(hitInfo.collider);
        }

        return moveVector;
    }

    void OnBulletCollision(Collider collider)
    {
        if (Hited)
            return;

        if(collider.gameObject.layer == LayerMask.NameToLayer("EnemyBullet")
            || collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            return;
        }

        Actor actor = collider.GetComponentInParent<Actor>();
        if(actor && actor.IsDead)
            return;

        actor.OnBulletHited(Owner, Damage);

        /*if (ownerSide == OwnerSide.Player)
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy.IsDead)
                return;

            enemy.OnBulletHited(enemy, Damage);
        }
        else
        {
            Player player = collider.GetComponentInParent<Player>();
            if (player.IsDead)
                return;

            player.OnBulletHited(player, Damage);
        }*/

        Collider myCollider = GetComponentInChildren<Collider>();
        myCollider.enabled = false;

        Hited = true;
        NeedMove= false;

        GameObject go = SystemManager.Instance.EffectManager.GenerateEffect(EffectManager.BulletDisappearFxIndex, transform.position);
        go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        Disappear();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnBulletCollision(other);
    }

    bool ProcessDisappearCondition()
    {
        if(transform.position.x > 15.0f || transform.position.x < -15.0f || transform.position.y > 15.0f || transform.position.y < -15.0f)
        {
            Disappear();
            return true;
        }
        else if(Time.time - FiredTime > LifeTime)
        {
            Disappear();
            return true;
        }

        return false;
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
