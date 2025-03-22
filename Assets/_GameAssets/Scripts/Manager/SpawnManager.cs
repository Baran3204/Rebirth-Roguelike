using NavMeshPlus.Components;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("References")]
   [SerializeField] private GameObject[] _enemyList;
   [SerializeField] private GameObject _medKit;
   [SerializeField] private Transform _spawnArea;

   [Header("Settings")]
   [SerializeField] private float _enemySpawnRadius;
   [SerializeField] private float _medkitSpawnRadius;
   [SerializeField] private float _enemySpawnCooldown;
   [SerializeField] private float _medkitSpawnCooldown;

   private float currentEnemyCooldown, currentMedkitCooldown;

   private void Awake() 
   {
        currentEnemyCooldown = _enemySpawnCooldown; 
   }

   private void Update() 
   {
     var currentState = GameManager.Instance.GetGameState();

     if(currentState != GameManager.GameState.Pause && currentState != GameManager.GameState.GameOver)
      {
          currentEnemyCooldown -= Time.deltaTime;
          currentMedkitCooldown -= Time.deltaTime;

         if(currentEnemyCooldown <= 0f)
         {
            SpawnEnemy();
            currentEnemyCooldown = _enemySpawnCooldown;
         } 
         if(currentMedkitCooldown <= 0f)
         {
            SpawnMedkit();
            currentMedkitCooldown = _medkitSpawnCooldown;
         }
      }
        
   }

   private void SpawnEnemy()
   {
        var currentEnemy = _enemyList[Random.Range(0, _enemyList.Length)]; 
        var currentSpawn = new Vector3
        (
           _spawnArea.transform.position.x + Random.Range(-_enemySpawnRadius, _enemySpawnRadius),    
           _spawnArea.transform.position.y + Random.Range(-_enemySpawnRadius, _enemySpawnRadius)
        );
        Instantiate(currentEnemy, currentSpawn, Quaternion.identity);
   }

   private void SpawnMedkit()
   {
      var currentSpawn = new Vector3
      (
         _spawnArea.transform.position.x + Random.Range(-_medkitSpawnRadius, _medkitSpawnRadius),    
         _spawnArea.transform.position.y + Random.Range(-_medkitSpawnRadius, _medkitSpawnRadius)    
      );

      float lucky = Random.Range(1, 101);
      
      if(lucky <= 25)
      {  
         GameObject medkit = Instantiate(_medKit, currentSpawn, Quaternion.identity);
         Destroy(medkit, 5f);
      }
       
   }  
}
