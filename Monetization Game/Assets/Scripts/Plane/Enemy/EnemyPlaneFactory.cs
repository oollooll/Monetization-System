using System.Collections;
using Services;
using Services.ObjectPool;
using Services.ProjectUpdater;
using Services.SoundManager;
using Services.VibrationManagement;
using UnityEngine;

namespace Plane.Enemy
{
    public class EnemyPlaneFactory : MonoBehaviour
    {
        public EnemyPlane prefab;
        public DarkStriker _darkStricker;
        private ObjectPool<EnemyPlane> objectPool;
        private Coroutine _spawnRoutine;
        private GameOverManager _gameOverManager;
        private int _darkStrickerStatus;
        
        private SoundManager _soundManager;
        private VibrationManager _vibrationManager;

        public void Initialize(GameOverManager gameOverManager,SoundManager soundManager,VibrationManager vibrationManager)
        {
            _gameOverManager = gameOverManager;
            _soundManager = soundManager;
            _vibrationManager = vibrationManager;
            objectPool = new ObjectPool<EnemyPlane>(prefab, 5);
            _spawnRoutine = StartCoroutine(SpawnDelay());
        }

        private void SpawnObject()
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-2.5f, 2.5f), 0f);
            EnemyPlane enemy = objectPool.Get();
            enemy.Initialize(5,_soundManager,_vibrationManager);
            enemy.transform.position = transform.position + randomOffset;
            enemy.gameObject.SetActive(true);
        }
        
        private void SpawnDarkS()
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-2.5f, 2.5f), 0f);
            EnemyPlane enemy = Instantiate(_darkStricker);
            enemy.Initialize(10,_soundManager,_vibrationManager);
            enemy.transform.position = transform.position + randomOffset;
            enemy.gameObject.SetActive(true);
        }

        private void ReturnObject(EnemyPlane obj)
        {
            objectPool.Return(obj);
        }

        private IEnumerator SpawnDelay()
        {
            _darkStrickerStatus = PlayerPrefs.GetInt("DarkStriker");
            
            while (!_gameOverManager.IsGameOver)
            {
                if (_darkStrickerStatus == 0 && Random.Range(0,10) == 5)
                {
                    SpawnDarkS();
                    _darkStrickerStatus = 1;
                }
                else
                {
                    SpawnObject();
                }
                
                yield return new WaitForSeconds(1f);
            }
        }
    }
}