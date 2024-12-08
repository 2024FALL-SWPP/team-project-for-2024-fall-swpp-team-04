using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
	public CanvasGroup currentCanvas; // ���� Ȱ��ȭ�� ĵ����
	public CanvasGroup newCanvas; // Ȱ��ȭ�� ���ο� ĵ����
	public float fadeDuration = 0.5f; // ���̵� ȿ�� ���� �ð�
	public GameObject[] panels; // ��ư�� ����� �гε��� �迭�� ����.

	[SerializeField] private GameObject[] _sliders;
	[SerializeField] private float[] _defaultValues;

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
		// Load settings
		LoadSettingsSave();
		StartCoroutine(FadeOutAndIn(currentCanvas, newCanvas));
	}

	// ĵ������ ����ġ�ϴ� �� ��° ��ư �Լ�
	public void SwitchToCurrentCanvas()
	{
		// Save settings
		SaveSettingsSave();
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

	private void LoadSettingsSave()
	{
		string json;
		if (!FileManager.LoadFromFile("settings.dat", out json))
		{
			// No file; Set to default
			SetSliders(_defaultValues);
			return;
		}
		var settingsSave = SettingsSave.Restore(json);
		if(settingsSave == null)
		{
			// Invalid file; Set to default
			Debug.Log("Invalid file: " + "settings.dat");
			SetSliders(_defaultValues);
			return;
		}
		var values = new float[3];
		values[0] = settingsSave.backGroundVolume;
		values[1] = settingsSave.effectVolume;
		values[2] = settingsSave.sensitivity;
		SetSliders(values);
	}
	private void SaveSettingsSave()
	{
		var save = new SettingsSave();
		save.backGroundVolume = _sliders[0].GetComponent<Slider>().value;
		save.effectVolume = _sliders[1].GetComponent<Slider>().value;
		save.sensitivity = _sliders[2].GetComponent<Slider>().value;

		FileManager.WriteToFile("settings.dat", JsonUtility.ToJson(save));
	}

	private void SetSliders(float[] values)
	{
		for(int i=0; i< values.Length; i++)
		{
			if (values[i] < 0) values[i] = 0;
			if (values[i] > 100) values[i] = 100;
			_sliders[i].GetComponent<Slider>().value = values[i];
		}
	}
}
