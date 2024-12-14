using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeHandler : MonoBehaviour
{
	public Text start;
	public Button startButton;

	private SettingsSave _settingsSave;
	private GameSave _gameSave;

	[SerializeField] private GameObject _textCanvas;
	[SerializeField] private GameObject _loadingCanvas;

	private void Awake()
	{
		Object.DontDestroyOnLoad(this.gameObject);
	}

	public void LoadGame(string scene)
	{
		FetchSaves();
		if (_gameSave.HasProgressed())
		{
			// savefile exists
			// TODO alert
			//return;
		}

		_loadingCanvas.SetActive(true);
		_textCanvas.SetActive(false);
		_loadingCanvas.GetComponent<LoadingCanvas>().StartLoading();
		StartCoroutine(LoadGameCoroutine(scene));
	}

	public void ContinueGame(string scene)
	{
		FetchSaves();
		if (!_gameSave.HasProgressed())
		{
			// save is empty
			// TODO alert
			return;
		}

		_loadingCanvas.SetActive(true);
		_textCanvas.SetActive(false);
		_loadingCanvas.GetComponent<LoadingCanvas>().StartLoading();
		StartCoroutine(ContinueGameCoroutine(scene));
	}

	private void InitGameSave()
	{
		// TODO Initial settings
		var player = GameObject.Find("Player");
		GameObject summonPoint = null;
		if (_gameSave.atLobby) { 
			summonPoint = GameObject.Find("Summon Point").transform.GetChild(0).gameObject;
		}
		else {
			summonPoint = GameObject.Find("Summon Point").transform.GetChild(1).GetChild(_gameSave.stage - 1).gameObject;
		}
		player.transform.position = summonPoint.transform.position;
		player.transform.rotation = summonPoint.transform.rotation;

		var coreController = StageManager.Instance.gameObject.GetComponent<CoreController>();
		if (_gameSave.atLobby)
		{
			coreController.OnLoad(_gameSave.stage - 1);
			StageManager.Instance.InitIsCleared(_gameSave.stage - 1);
			player.GetComponent<PlayerController>().UpdateStage(_gameSave.stage);
			UIManager.Instance.EnergyGaugeUi();
			StageManager.Instance.LoadStage(_gameSave.stage - 1);
		}
	}

	private void InitSettingsSave()
	{
		var player = GameObject.Find("Player");
		player.GetComponent<PlayerMovement>().SetSensitivityMultiplier(_settingsSave.sensitivity);
		GameManager.Instance.SetBGMVolume(_settingsSave.backGroundVolume/100f);
		GameManager.Instance.SetSFXVolume(_settingsSave.effectVolume/100f);
	}

	IEnumerator LoadGameCoroutine(string scene)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
		//var mainMenu = SceneManager.GetActiveScene();
		//SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
		InitSettingsSave();
		//SceneManager.UnloadSceneAsync(mainMenu);

		Destroy(_loadingCanvas);
		Destroy(gameObject);
	}

	IEnumerator ContinueGameCoroutine(string scene)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
		//var mainMenu = SceneManager.GetActiveScene();
		//SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
		InitGameSave();
		InitSettingsSave();
		//SceneManager.UnloadSceneAsync(mainMenu);

		Destroy(_loadingCanvas);
		Destroy(gameObject);
	}

	private void FetchSaves()
	{
		_gameSave = CanvasSwitcher.Instance.gameSave;
		_settingsSave = CanvasSwitcher.Instance.settingsSave;
	}
}
