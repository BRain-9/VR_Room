using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalleteDBCreator : MonoBehaviour
{

	#region Params
	public Dictionary<int, List <Vector3>> palleteDB;
	private int rows = 3, columns = 6;
	private const int maxValue = 100;
	public GameObject paletteCategoryItem;
	[SerializeField] Transform categorysParent;
	private bool dbFlag, paletteFlag;
	public static GameObject ChoosenItem;

	#endregion

	private void Awake()
	{
		palleteDB = new Dictionary<int, List<Vector3>>();
		//StartCoroutine (ShowPallete());
	}

	private IEnumerator ShowPallete(){
		Create_DB();
		yield return new WaitUntil(()=> dbFlag);
        CreatePalette();
	}

	private void Create_DB()
	{
		List<PalleteItem> palleteItems = new List<PalleteItem>();
		List<Vector3> colorArr = new List<Vector3>();
		float h = 0, s = 0, v = 0;
		int key = 0;
		Color[] colors =
		{     Color.red
			, new Color (1,0.65f,0.25f)
			, Color.yellow
			, Color.green
			, Color.blue
			, Color.magenta};

		for (int i = 0; i < colors.Length; i++)
		{
			Color col = colors[i];
			Color.RGBToHSV (col, out h, out s, out v);
            

			for (int iV = maxValue; iV > 20; iV -= 35)
			{
				colorArr = new List<Vector3>();
				for (int iS = 3; iS < maxValue; iS += 2)
				{
					s = (float) iS/100;
					v = (float) iV/100;
					colorArr.Add(new Vector3(h, s, v));
				}
				palleteDB.Add(key, colorArr);
                key++;
			}

		}
		dbFlag = true;
		this.PrintLog("Palette DB Created! Pallete arr = " + palleteDB.Keys.Count);
	}

	private void CreatePalette(){
		GameObject go = null;
		for (int i = 0; i < palleteDB.Keys.Count; i++)
		{
			go = Instantiate(paletteCategoryItem.gameObject,categorysParent);
			go.GetComponent<PalleteItem>().AddColorItems(palleteDB[i]);
			go.transform.GetChild(0).SetAsLastSibling();
		}
		paletteFlag = true;
	}

}
