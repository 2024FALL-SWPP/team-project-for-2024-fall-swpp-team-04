using UnityEngine;

public class CoreInteraction : MonoBehaviour, IInteractable
{
	[SerializeField] private Light _coreLight;
	[SerializeField] private GameObject _door;
	[SerializeField] private GameObject _wall;
	[SerializeField] private GameObject _spawner;
	[SerializeField] private Renderer _forceFieldRenderer;
	private GameManager _gameManager;

	private bool _isCheckingEnemies = false;

	void Start()
	{
		_gameManager = FindObjectOfType<GameManager>();
		_wall.SetActive(false);
		_spawner.SetActive(false);

		// ����� Emission�� �ʱ� ���¿��� ��Ȱ��ȭ
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
		// ���� Ȱ��ȭ
		if (_coreLight != null)
		{
			_coreLight.enabled = true;
		}
		// ForceField Emission Ȱ��ȭ
		if (_forceFieldRenderer != null)
		{
			Material forceFieldMaterial = _forceFieldRenderer.material;
			forceFieldMaterial.EnableKeyword("_EMISSION");
			forceFieldMaterial.SetColor("_EmissionColor", Color.white); // ���ϴ� ������ ����
		}

		_wall.SetActive(true);
		UIManager.Instance.TriggerCoreInteractionUi();
		_spawner.SetActive(true);

		StartCoroutine(StartCheckingEnemiesAfterDelay(62f));
	}

	private System.Collections.IEnumerator StartCheckingEnemiesAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);

		_isCheckingEnemies = true;

		while (_isCheckingEnemies)
		{
			if (_gameManager.GetActiveEnemies().Count == 0)
			{
				if (_door.TryGetComponent<IDoor>(out IDoor door))
				{
					door.Open();
				}
				_isCheckingEnemies = false;
			}

			yield return new WaitForSeconds(1f);
		}
	}
}
