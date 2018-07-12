using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{

	#region Params
	[SerializeField] PalleteMenuController _palleteMenuController;
	[SerializeField] CategoryMenu _categoryMenu;
	[SerializeField] GameObject _panelsContainer, _scaner;
	[SerializeField] CameraFollow _cameraFollow;
	private static UI_Manager _instance;
	public static UI_Manager Instance
	{
		get { return _instance; }
	}
	public Animator _animator;
	public const string HIDE_PALLETE_TAG = "HidePallete"
		, SHOW_PALLETE_TAG = "ShowPallete";


	#endregion

	void Awake()
	{
		if (_instance != null && _instance != this)
			Destroy(this);
		_instance = this;
		DontDestroyOnLoad(this.gameObject);
		CloseElement();
	}

	public void PlayAnimation(string tag)
	{
			_animator.SetTrigger(tag);
	}

	public void PalleteCategoryManager()
	{
		_categoryMenu.gameObject.SetActive(true);
	}

	public void CloseElement()
	{
		this.PrintLog("Serch CLoseble elements");
		int count = 0;
		for (int i = 0; i < _panelsContainer.transform.childCount; i++)
		{
			var go = _panelsContainer.transform.GetChild(i).GetComponent<IClosebleObj>();
			if (go != null)
			{
				count++;
				go.Close();
			}
		}
		this.PrintLog("ClosebleObjects = " + count);
	}

	public void ShowPallete()
    {
		CloseElement();
		//_animator.SetTrigger(SHOW_PALLETE_TAG);
		_palleteMenuController.gameObject.SetActive(true);
    }

	public void ShowColorMenu()
	{
		CloseElement();
		StartCoroutine(ColorDBController.Instance.FadeIn());
	}

	public void ShowScaner()
	{
		CloseElement();
		_scaner.SetActive(true);
		for (int i = 0; i < _scaner.transform.childCount; i++)
            {
                _scaner.transform.GetChild(i).gameObject.SetActive(true);
            }
	}

}
