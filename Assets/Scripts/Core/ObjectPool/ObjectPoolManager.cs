using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager
{
    #region Field

    private class ObjectInfo
    {
        public string objectName;
        public int size;

        public ObjectInfo(string name, int _size)
        {
            objectName = name;
            size = _size;
        }
    }

    private ObjectInfo[] poolList = new ObjectInfo[] {
        new ObjectInfo("PlayerProjectileFrame", 20),
        new ObjectInfo("EnemyProjectileFrame", 20),
        new ObjectInfo("FollowerProjectileFrame", 20),
        new ObjectInfo("Canvas_FloatingDamage", 35),
        new ObjectInfo("EnemyFrame", 10)
    };

    private string objectName;

    GameObject objPoolParent;

    private Dictionary<string, IObjectPool<GameObject>> poolDic = new Dictionary<string, IObjectPool<GameObject>>();

    #endregion

    #region Init

    public void Initialize()
    {
        objPoolParent = new GameObject("Object Polling List");

        for (int i = 0; i < poolList.Length; i++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreateProjectile, OnGetProjectile,
                OnReleaseProjectile, OnDestroyProjectile, maxSize: poolList[i].size);

            poolDic.Add(poolList[i].objectName, pool);

            for (int j = 0; j < poolList[i].size; j++)
            {
                objectName = poolList[i].objectName;
                ObjectPoolable poolGo = CreateProjectile().GetComponent<ObjectPoolable>();
                poolGo.Poolable.Release(poolGo.gameObject);
            }
        }
    }

    #endregion

    #region PoolMethod

    private GameObject CreateProjectile()
    {
        GameObject poolGo = Manager.Asset.InstantiatePrefab(objectName);
        poolGo.GetComponent<ObjectPoolable>().SetManagedPool(poolDic[objectName]);
        poolGo.transform.SetParent(objPoolParent.transform);
        return poolGo;
    }

    private void OnGetProjectile(GameObject projectile)
    {
        projectile.SetActive(true);
    }

    private void OnReleaseProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
    }

    private void OnDestroyProjectile(GameObject projectile)
    {
        GameObject.Destroy(projectile);
    }

    public GameObject GetGo(string goName)
    {
        objectName = goName;

        return poolDic[goName].Get();
    }

    #endregion
}
