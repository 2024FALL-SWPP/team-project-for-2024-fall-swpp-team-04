using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
	[SerializeField] private List<GameObject> spawnerObjects;
	[SerializeField] private float _time = 5f;
	[SerializeField] private int _enemies = 3;
	[SerializeField] private float _customDelay = 0.3f;

	private List<IEnemyFactory> _spawners = new List<IEnemyFactory>();
	private int _currentSpawnerIndex = 0;

	void Start()
	{
		foreach (var obj in spawnerObjects)
		{
			var spawner = obj.GetComponent<IEnemyFactory>();
			_spawners.Add(spawner);
		}

		InvokeRepeating(nameof(SpawnEnemies), 0f, _time);
	}

	private void SpawnEnemies()
	{
		for (int i = 0; i < _enemies; i++)
		{
			if (_spawners[_currentSpawnerIndex] is DelayedSpawner delayedSpawner)
			{
				delayedSpawner.SetTimer(i * _customDelay);
			}
			else
			{
				_spawners[_currentSpawnerIndex].SpawnEnemy();
			}
		}

		_currentSpawnerIndex = (_currentSpawnerIndex + 1) % _spawners.Count;
	}

}
