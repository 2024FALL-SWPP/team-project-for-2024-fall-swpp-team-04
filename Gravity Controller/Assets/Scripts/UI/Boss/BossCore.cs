using System.Collections;
using UnityEngine;

public class BossCore : MonoBehaviour, IInteractable
{
	[SerializeField] private Light _coreLight;
	[SerializeField] private GameObject _textCanvas;
	[SerializeField] private GameObject _videoCanvas;

	private VideoManager _videoManager;
	private bool _isInteractive= true;

	void Start()
	{
		_textCanvas.SetActive(false);
		_videoCanvas.SetActive(false);
		_coreLight.enabled = false;
		_videoManager = _videoCanvas.GetComponent<VideoManager>();
	}

	public void Interactive()
	{
		StartInteraction();
	}

	public bool IsInteractable()
	{
		return _isInteractive;
	}
	private void StartInteraction()
	{
		_isInteractive = false;
		_coreLight.enabled = true;

		// ������ �ƴ� �ؽ�Ʈ ���� ����
		StartCoroutine(ShowTextFirstCoroutine());
	}

	private IEnumerator ShowTextFirstCoroutine()
	{
		// �ؽ�Ʈ ĵ���� Ȱ��ȭ
		_textCanvas.SetActive(true);
		yield return new WaitForSeconds(3f); // 3�� ����
		_textCanvas.SetActive(false);		

		// �ؽ�Ʈ ���� �� �������� �̵� ����
		if (_videoManager._bossStageController != null)
			_videoManager._bossStageController.StartMoving();
	}
}
