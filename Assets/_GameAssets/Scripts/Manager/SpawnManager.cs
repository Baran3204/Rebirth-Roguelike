using NavMeshPlus.Components;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("References")]
   [SerializeField] private GameObject[] _enemyList;
   [SerializeField] private Transform _spawnArea;

   [Header("Settings")]
   [SerializeField] private float _spawnRadius;
   [SerializeField] private float _enemySpawnCooldown;

   private float currentCooldown;

   private void Awake() 
   {
        currentCooldown = _enemySpawnCooldown; 
   }

   private void Update() 
   {
     var currentState = GameManager.Instance.GetGameState();

     if(currentState != GameManager.GameState.Pause && currentState != GameManager.GameState.GameOver)
      {
          currentCooldown -= Time.deltaTime;

         if(currentCooldown <= 0f)
         {
             SpawnEnemy();
             currentCooldown = _enemySpawnCooldown;
         } 
      }
        
   }

   private void SpawnEnemy()
   {
        var currentEnemy = _enemyList[Random.Range(0, _enemyList.Length)]; 
        var currentSpawn = new Vector3
        (
           _spawnArea.transform.position.x + Random.Range(-_spawnRadius, _spawnRadius),    
           _spawnArea.transform.position.y + Random.Range(-_spawnRadius, _spawnRadius)    

        );
        Instantiate(currentEnemy, currentSpawn, Quaternion.identity);

   }
}
