using UnityEngine;

public class CoreInteraction : MonoBehaviour, IInteractable
{
	[SerializeField] private Light _coreLight;
	[SerializeField] private GameObject _door;
	[SerializeField] private GameObject _wall;
	[SerializeField] private GameObject _spawner;
	[SerializeField] private Renderer _forceFieldRenderer;
	[SerializeField] private CoreController _coreController;
	private GameManager _gameManager;

	private bool _isInteractable = true;
	private bool _hasEnemiesCleared = false; // �� óġ �Ϸ� ����

	void Start()
	{
		_gameManager = FindObjectOfType<GameManager>();
		_wall.SetActive(false);
		_spawner.SetActive(false);

		if (_coreLight != null)
		{
			_coreLight.enabled = false;
		}
		if (_forceFieldRenderer != null)
		{
			Material forceFieldMaterial = _forceFieldRenderer.material;
			forceFieldMaterial.DisableKeyword("_EMISSION");
			forceFieldMaterial.SetColor("_EmissionColor", Color.black);
		}
	}

	public void Interactive()
	{
		if (!_isInteractable) return;

		if (!_hasEnemiesCleared)
		{
			// ù ��° ��ȣ�ۿ�: ����(Light)�� Ȱ��ȭ
			_isInteractable = false;
			if (_coreLight != null)
			{
				_coreLight.enabled = true;
			}

			_wall.SetActive(true);
			UIManager.Instance.TriggerCoreInteractionUi();
			_spawner.SetActive(true);

			StartCoroutine(ClearEnemiesAndActivateEmission());
		}
		else
		{
			if (_forceFieldRenderer != null)
			{
				Material forceFieldMaterial = _forceFieldRenderer.material;
				forceFieldMaterial.EnableKeyword("_EMISSION");
				forceFieldMaterial.SetColor("_EmissionColor", Color.white); 
			}

			if (_door.TryGetComponent<IDoor>(out IDoor door))
			{
				door.Open();
			}
			_coreController.ResetCoreController();
		}
	}

	private System.Collections.IEnumerator ClearEnemiesAndActivateEmission()
	{
		yield return new WaitForSeconds(60f);
		SendOnDeathSignalToEnemies();

		// ��ȣ�ۿ� ���� ���� ����
		_hasEnemiesCleared = true;
		_isInteractable = true;
	}

	private void SendOnDeathSignalToEnemies()
	{
		var activeEnemies = _gameManager.GetActiveEnemies().ToArray(); 
		foreach (var enemy in activeEnemies)
		{
			if (enemy.TryGetComponent<IEnemy>(out IEnemy enemyScript))
			{
				enemyScript.OnDeath();
			}
		}
	}

	public bool IsInteractable() => _isInteractable;
}
