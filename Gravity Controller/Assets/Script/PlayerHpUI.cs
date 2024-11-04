using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHpUI : MonoBehaviour
{
	[SerializeField] private Slider hpSlider; // HP ������
	public float currentHp;
	private float targetHp;
	private float maxHp = 100f;

	void Start()
	{
		currentHp = maxHp;
		targetHp = maxHp;
		hpSlider.maxValue = maxHp;
		hpSlider.value = currentHp;
	}

	public void UpdateHP(float newHp)
	{
		targetHp = newHp;
		StartCoroutine(HPDecrease());
	}

	private IEnumerator HPDecrease()
	{
		while (currentHp > targetHp)
		{
			currentHp = Mathf.Lerp(currentHp, targetHp, 0.2f);
			hpSlider.value = currentHp;

			// ��ǥġ�� ����� ��������� ���� ����
			if (Mathf.Abs(currentHp - targetHp) < 0.1f)
			{
				currentHp = targetHp;
				hpSlider.value = currentHp;
				yield break;
			}

			yield return new WaitForSeconds(0.02f);
		}
	}

}