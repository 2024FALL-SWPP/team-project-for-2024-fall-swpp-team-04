using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreController : MonoBehaviour, IInteractable
{
    [Header("Initial Global Light")]
    [SerializeField] private float _initialSunLightIntensity;
    [SerializeField] private Color _initialEnvironmentLightColor;
    [SerializeField] private float _initialEnvironmentLightIntensity;
    [SerializeField] private Color _initialFogColor;
    [SerializeField] private float _initialFogDensity;
    [Header("Global Light On")]
    [SerializeField] private Light _sunLight;
    [SerializeField] private float _sunLightIntensity;
    [SerializeField] private Color _environmentLightColor;
    [SerializeField] private float _environmentLightIntensity;
    [SerializeField] private float _fogDensity;
    [SerializeField] private float _lightOnDamping;
    [SerializeField] private List<Light> _stageLights;
	public bool _isInteractable = true;
    private float _epsilon = 1e-5f;
    private int _current_stage = 1;

    [Header("Core Restored")]
    [SerializeField] private Renderer[] _batteries;
    [SerializeField] private Material _blueEmission;
    [SerializeField] private Light[] _batteryLights;
    [SerializeField] private float _batteryLightIntensity;
    [SerializeField] private PlayerController _player;

	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private AudioClip _lightSound;
	[SerializeField] private AudioClip _coreSound;

	[SerializeField] private float _lightSoundMaxVolume = 1f;
	[SerializeField] private float _coreSoundMaxVolume = 1f;

	// Start is called before the first frame update
	void Start()
    {
        InitializeGlobalLight();
		// StartCoroutine(GlobalLightOn());
		_player = FindObjectOfType<PlayerController>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interactive() {
        if(_isInteractable) {
			_isInteractable = false;
			_audioSource.volume = _lightSoundMaxVolume * GameManager.Instance.GetSFXVolume();
			_audioSource.PlayOneShot(_lightSound);
			StartCoroutine(GlobalLightOn());
        }

        RestoreCore(_current_stage - 1);
        StartCoroutine(UIManager.Instance.ShowStageIntro(_current_stage - 1));
        _player.UpdateStage(_current_stage + 1);
		_player.InitState();
        UIManager.Instance.EnergyGaugeUi();
        for (int i = 0; i < _stageLights.Count; i++)
        {
	        if (i == _current_stage-1)
	        {
		        _stageLights[i].intensity = 10;
	        }
	        else
	        {
		        _stageLights[i].intensity = 0f;
	        }
        }

		StartCoroutine(RemainingProcess());
	}

	private IEnumerator RemainingProcess()
	{
		yield return new WaitForSeconds(0f);
		StageManager.Instance.LoadStage(_current_stage);
		_current_stage++;
		Autosave.SaveGameSave(true, _current_stage);
    }
    
    private void InitializeGlobalLight()
    {
		_isInteractable = true;

		_sunLight.intensity = _initialSunLightIntensity;
        RenderSettings.ambientLight = _initialEnvironmentLightColor;
        RenderSettings.ambientIntensity = _initialEnvironmentLightIntensity;
        RenderSettings.fogColor = _initialFogColor;
        RenderSettings.fogDensity = _initialFogDensity;
        if (_stageLights != null)
        {
	        foreach (Light stageLight in _stageLights)
	        {
		        stageLight.intensity = 0f;
	        }
        }
	}

    private IEnumerator GlobalLightOn() {
        while(_sunLight.intensity < _sunLightIntensity - _epsilon) {
            _sunLight.intensity = Mathf.Lerp(_sunLight.intensity, _sunLightIntensity, Time.deltaTime * _lightOnDamping);
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, _environmentLightColor, Time.deltaTime * _lightOnDamping);
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, _environmentLightIntensity, Time.deltaTime * _lightOnDamping);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, _fogDensity, Time.deltaTime * _lightOnDamping);
            yield return null;
        }
        _sunLight.intensity = _sunLightIntensity;
        RenderSettings.ambientLight = _environmentLightColor;
        RenderSettings.ambientIntensity = _environmentLightIntensity;
        RenderSettings.fogDensity = _fogDensity;

	}

    public void RestoreCore(int stage) {
        if(stage < 0 || stage > 3) {
            Debug.Log("CoreController.RestoreCore: Invalid stage index");
            return;
        }
        _batteries[stage].materials[1] = _blueEmission;
        _batteryLights[stage].intensity = _batteryLightIntensity;
		_audioSource.volume = _coreSoundMaxVolume * GameManager.Instance.GetSFXVolume();
		_audioSource.PlayOneShot(_coreSound);
	}
    public bool IsInteractable()
    {
	    return _isInteractable; 
    }

    public void ResetCoreController()
    {
	    _isInteractable = true;
    }

	public void OnLoad(int stage)
	{
		if(stage == 0)
		{
			return;
		}

		_isInteractable = false;
		StartCoroutine(GlobalLightOn());

		for(int i=0;i < stage;i++)
		{
			RestoreCore(i);
		}

		for (int i = 0; i < _stageLights.Count; i++)
		{
			if (i == stage - 1)
			{
				_stageLights[i].intensity = 10f;
			}
			else
			{
				_stageLights[i].intensity = 0f;
			}
		}

		_current_stage = stage + 1;
	}
}
