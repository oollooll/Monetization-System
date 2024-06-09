using System.Collections;
using System.Collections.Generic;
using Plane.Enemy;
using Services;
using Services.ObjectPool;
using Services.ProjectUpdater;
using UnityEngine;

namespace Plane.Bullet
{
    public class BulletFactory : MonoBehaviour
    {
        public Bullet prefab;
        private ObjectPool<Bullet> objectPool;
        private ScoreUpdater _scoreUpdater;
        private Coroutine _spawnRoutine;
        private GameOverManager _gameOverManager;

        public void Initialize(ScoreUpdater scoreUpdater,GameOverManager gameOverManager)
        {
            _scoreUpdater = scoreUpdater;
            _gameOverManager = gameOverManager;
            objectPool = new ObjectPool<Bullet>(prefab, 10);
            foreach (var bullet in objectPool.pool)
            {
                bullet.ScoreUpdated += _scoreUpdater.UpdateScore;
            }
            _spawnRoutine = StartCoroutine(SpawnDelay());
        }

        private void SpawnObject()
        {
            Bullet bullet = objectPool.Get();
            bullet.Initialize(5);
            bullet.transform.position = transform.position;
            bullet.gameObject.SetActive(true);
        }

        private void ReturnObject(Bullet obj)
        {
            objectPool.Return(obj);
        }

        private IEnumerator SpawnDelay()
        {
            yield return new WaitForSeconds(1f);
            while (!_gameOverManager.IsGameOver)
            {
                SpawnObject();
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}