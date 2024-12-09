using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Stage4 : MonoBehaviour, IDoor
{
	public void Open()
	{
		// deleteCeils ���� ��� �ڽ� ������Ʈ ��������
		foreach (Transform child in transform)
		{
			// �ڽ� ������Ʈ�� �ڽ� Ž��
			foreach (Transform grandchild in child)
			{
				// BoxCollider�� �ִ� ��� ����
				BoxCollider boxCollider = grandchild.GetComponent<BoxCollider>();
				if (boxCollider != null)
				{
					Destroy(boxCollider); // BoxCollider ����
				}
			}
		}
	}

	public void Close()
	{
		return;
	}
}