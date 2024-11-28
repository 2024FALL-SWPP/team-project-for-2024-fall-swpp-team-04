using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
	public CanvasGroup currentCanvas; // ���� Ȱ��ȭ�� ĵ����
	public CanvasGroup newCanvas; // Ȱ��ȭ�� ���ο� ĵ����
	public float fadeDuration = 0.5f; // ���̵� ȿ�� ���� �ð�
	public GameObject[] panels; // ��ư�� ����� �гε��� �迭�� ����.

	// ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
	public void ShowPanel(int index)
	{
		// ��� �г� ��Ȱ��ȭ
		foreach (GameObject panel in panels)
		{
			panel.SetActive(false);
		}

		// ���õ� �гθ� Ȱ��ȭ
		if (index >= 0 && index < panels.Length)
		{
			panels[index].SetActive(true);
		}
	}

	// ĵ������ ����ġ�ϴ� ù ��° ��ư �Լ�
	public void SwitchToNewCanvas()
	{
		StartCoroutine(FadeOutAndIn(currentCanvas, newCanvas));
	}

	// ĵ������ ����ġ�ϴ� �� ��° ��ư �Լ�
	public void SwitchToCurrentCanvas()
	{
		StartCoroutine(FadeOutAndIn(newCanvas, currentCanvas));
	}

	// ���̵� �ƿ��� ���̵� ���� ���ÿ� ����
	private IEnumerator FadeOutAndIn(CanvasGroup canvasToFadeOut, CanvasGroup canvasToFadeIn)
	{
		// ���� ĵ������ ���̵� �ƿ�
		if (canvasToFadeOut != null)
		{
			yield return StartCoroutine(FadeOut(canvasToFadeOut));
			canvasToFadeOut.gameObject.SetActive(false); // ���̵� �ƿ� �� ��Ȱ��ȭ
		}

		// �� ĵ������ Ȱ��ȭ
		if (canvasToFadeIn != null)
		{
			canvasToFadeIn.gameObject.SetActive(true);
			yield return StartCoroutine(FadeIn(canvasToFadeIn));
		}
	}

	// ���̵� �ƿ� ����
	private IEnumerator FadeOut(CanvasGroup canvasGroup)
	{
		float elapsedTime = 0f;

		while (elapsedTime < fadeDuration)
		{
			canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		canvasGroup.alpha = 0f; // ���̵� �ƿ� �Ϸ�
	}

	// ���̵� �� ����
	private IEnumerator FadeIn(CanvasGroup canvasGroup)
	{
		float elapsedTime = 0f;

		while (elapsedTime < fadeDuration)
		{
			canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		canvasGroup.alpha = 1f; // ���̵� �� �Ϸ�
	}
}
