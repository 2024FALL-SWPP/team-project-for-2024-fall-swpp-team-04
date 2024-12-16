using System.Collections;
using UnityEngine;

public class BossStageController : MonoBehaviour
{
	[SerializeField] private Vector3 _moveDirection = Vector3.up;
	[SerializeField] private float _moveDistance = 1000f;
	[SerializeField] private float _moveDuration = 20f;

	[SerializeField] private GameObject _videoCanvas;
	[SerializeField] public GameObject _finalCanvas; // finalCanvas ������ ���� public���� ����

	private Vector3 _startPosition;
	private Vector3 _targetPosition;
	private bool _isMoving = false;

	void Start()
	{
		_startPosition = transform.position;
		_targetPosition = _startPosition + _moveDirection.normalized * _moveDistance;

		if (_finalCanvas != null)
			_finalCanvas.SetActive(false);
	}

	public void StartMoving()
	{
		Destroy(GameObject.Find("Player").GetComponent<PlayerController>());

		if (!_isMoving)
			StartCoroutine(MoveStage());
	}

	private IEnumerator MoveStage()
	{
		_isMoving = true;
		float elapsedTime = 0f;

		while (elapsedTime < _moveDuration)
		{
			transform.position = Vector3.Lerp(_startPosition, _targetPosition, elapsedTime / _moveDuration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		transform.position = _targetPosition;
		_isMoving = false;

		_videoCanvas.SetActive(true); _videoCanvas.SetActive(true);

		Destroy(GameObject.Find("Player").GetComponent<PlayerMovement>());

		// �������� �̵� �Ϸ� �� ���� ���
		VideoManager vm = FindObjectOfType<VideoManager>();
		if (vm != null)
			vm.PlayVideo();
	}
}
