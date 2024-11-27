using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreInteraction : MonoBehaviour, IInteractable
{
	[SerializeField] private Material _coreDeactivatedMaterial; // �ھ� ��Ȱ��ȭ ���� ��Ƽ����
	[SerializeField] private Light _coreLight; // �ھ� ������ ����
	[SerializeField] private GameObject _coreUI; 

	void Start()
	{
	}

	public void Interactive()
	{
		// ������ ��Ȱ��ȭ�Ͽ� �ھ��� ���¸� �ð������� ǥ��
		if (_coreLight != null)
		{
			_coreLight.enabled = false;
		}


		// �ھ�� ��ȣ�ۿ� �� UIManager�� ���� UI�� ����
		UIManager.Instance.TriggerCoreInteractionUi();
	}
}