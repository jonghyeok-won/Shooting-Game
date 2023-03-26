using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    static SystemManager instance = null;

    [SerializeField]
    Player player;

    public Player Hero
    {
        get { return player; }
    }


    public EffectManager effectManager;

    public EffectManager EffectManager
    {
        get
        {
            return effectManager;
        }
    }

    public EnemyManager enemyManager;

    public EnemyManager EnemyManager
    {
        get
        {
            return enemyManager;
        }
    }


    public BulletManager bulletManager;

    public BulletManager BulletManager
    {
        get { return bulletManager; }
    }

    PrefabCacheSystem enemyCacheSystem = new PrefabCacheSystem();

    public PrefabCacheSystem EnemyCacheSystem
    {
        get
        {
            return enemyCacheSystem;
        }
    }

    PrefabCacheSystem bulletCacheSystem = new PrefabCacheSystem();

    public PrefabCacheSystem BulletCacheSystem
    {
        get
        {
            return bulletCacheSystem;
        }
    }

    PrefabCacheSystem effectCacheSystem = new PrefabCacheSystem();

    public PrefabCacheSystem EffectCacheSystem
    {
        get { return effectCacheSystem;}
    }

    public static SystemManager Instance
    {
        get 
        { 
            return instance; 
        }
    }


    GamePointAccumulator gamePointAccumulator = new GamePointAccumulator();

    public GamePointAccumulator GamePointAccumulator
    {
        get { return gamePointAccumulator; }
    }


    private void Awake()
    {
        if(instance != null) 
        {
            Debug.LogError("SystemManager error! Singletone error!");
            Destroy(gameObject);
            return;
        }

        instance= this;

        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
