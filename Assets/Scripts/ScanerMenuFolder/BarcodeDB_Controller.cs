using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using UnityEngine.UI;

public class BarcodeDB_Controller : MonoBehaviour {

	#region Params
	public string url;
	public RawImage cameraVisor;
	public Image indicator;
	public Dictionary<string, BarcodeDB_Item> barcodeDB;
	public List<Color> colorHistory;
	public GameObject pallete, colorPrefab;
	private const int _maxHistoryCount = 10;
	string fileName = "BarcodeDB";
	private static bool barcodeDB_loded;
	public static bool BarcodeFlag{
		get { return barcodeDB_loded; }
	}
    
	private const float redColorRefresh = 2f;
	#endregion
	private void OnEnable()
	{
		RefreshColors(Color.white,"");
	}

	IEnumerator Start () {

        this.PrintLog("IEnumerator Start");
		barcodeDB = new Dictionary<string, BarcodeDB_Item>();
		colorHistory = new List<Color>();

		SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            if (scene.name.Contains("Room"))
            {
				StartCoroutine(LoadDB());
            }
        };

		yield return new WaitUntil(()=> barcodeDB_loded);

        this.PrintLog("BarcodeDB_Loaded");

		Scaner.barcodeEvent += (string key, UnityEngine.UI.Text text) =>
		{
			string s = key.Remove(key.Length - 1);
			this.PrintLog("barcode key " + key + "barcode s " + s);
			//text.supportRichText = true;
			if (barcodeDB.ContainsKey(s))
			{
				var obj = barcodeDB[s];
				Vector4 vector = obj.color;
				Color color = new Color32((byte)vector.x, (byte)vector.y, (byte)vector.z, (byte)vector.w);
				ColorDBController.Instance.Redraw(color);
				colorHistory.Add(color);
				indicator.color = Color.green;
				cameraVisor.gameObject.SetActive(false);
				RefreshColors(color,barcodeDB[s].name);
			}else{
				StartCoroutine(RedColor());
			}
		};
	}

	public void RefreshColors (Color color, string name){
		GameObject go = null;
		if (colorHistory.Count>0){
			//for (int i = 1; i < pallete.transform.childCount; i++)
			//{
			//	Destroy(pallete.transform.GetChild(i).gameObject);
			//}
			//foreach (var item in colorHistory)
			//{
			if (colorHistory.Count > 0)
			{
				if (colorHistory.Count < _maxHistoryCount)
				{
					go = Instantiate(colorPrefab, pallete.transform);
				}
				else
				{
					go = Instantiate(colorPrefab, pallete.transform);
					go.transform.SetAsFirstSibling();
					Destroy(pallete.transform.GetChild(pallete.transform.childCount - 1));
				}

				go.GetComponent<ColorItem>().colorBody.color = color;
				go.GetComponent<ColorItem>().GetComponentInChildren<Text>().text = name;
				//}
			}
		}
	}

	private IEnumerator LoadDB()
    {
     //   this.PrintLog("LoadDB");

            string contentPath = null;
#if UNITY_EDITOR
		contentPath = Application.dataPath + "/Resources/" + fileName +".csv";
#elif UNITY_ANDROID || UNITY_IOS
        contentPath = Application.persistentDataPath + "/"+ fileName +".csv";
#endif
            string tempText = null;
            if (Utils.IsOnline())
            {
			WWW load = new WWW(url);
                yield return load;
                if (File.Exists(contentPath))
                    File.Delete(contentPath);

                StreamWriter writer = new StreamWriter(contentPath, true);
                writer.Write(load.text);
                writer.Close();

              //  this.PrintLog("LOAD: \n" + load.text);
            }

            if (File.Exists(contentPath))
            {
                //WWW www = new WWW(contentPath);
                //yield return www;
                var www = File.ReadAllText(contentPath);
                tempText = www;
            }
            else
            {
			var v = Resources.Load(fileName);
                tempText = v.ToString();
            }
            var temp = CSVReader.SplitCsvGrid(tempText);//www.text);
            foreach (var item in Enumerable.Range(2, temp.GetLength(0)))
            {
                if (temp[item, 0] == null)
                {
                    break;
                }
                foreach (int j in Enumerable.Range(1, temp.GetLength(1)))
                {

                    if (temp[0, j] == null)
                    {
                        break;
                    }
    				BarcodeDB_Item barcode_Item = new BarcodeDB_Item();

    				barcode_Item.name = FillField(temp[0, j]);

    				barcode_Item.color = StringToColor(temp[1, j]);

				    barcode_Item.barcode = FillField (temp[item, j]);

				this.PrintLog("barcode = " + barcode_Item.barcode);

				if (!barcodeDB.ContainsKey(barcode_Item.barcode))
				    barcodeDB.Add(barcode_Item.barcode, barcode_Item);
                }

            }
		barcodeDB_loded = true;
		this.PrintLog("Barcode DB loaded.DB count = " + barcodeDB.Count + ".First item key = ");
        
        }

	private IEnumerator RedColor(){
		indicator.color = Color.red;
		yield return new WaitForSeconds(redColorRefresh);
		indicator.color = Color.white;
        
	}

	private string FillField(string str, bool isRemove = false, int charTodelete = 0)
	{
		string result = str;
        
		return string.IsNullOrEmpty(result) ? "EMPTY FIELD" : result;
    }

	private Vector4 StringToColor(string str){
		Vector4 v = Vector4.zero;
		float x, y, z;
		try
		{
			string[] temp = str.Split(',');

			float.TryParse(temp[0], out x);
			float.TryParse(temp[1], out y);
			float.TryParse(temp[2], out z);
			v = new Vector4(x,y,z,255);
		}
		catch (System.Exception ex)
		{
			this.PrintLog(ex.Message);
		}
		return  v;
	}

	public void ClearStory()
	{
		for (int i = 1; i < pallete.transform.childCount; i++)
		{
			Destroy(pallete.transform.GetChild (i).gameObject);
		}

		colorHistory.Clear();
	}
}

public class BarcodeDB_Item{
	public string name, barcode;
	public Vector4 color;
}

/*
    private IEnumerator LoadContentDB()
    {
        string contentPath = null;
#if UNITY_EDITOR
        contentPath = Application.dataPath + "/Resources/Content.csv";
#elif UNITY_ANDROID || UNITY_IOS
        contentPath = Application.persistentDataPath + "/Content.csv";
#endif
        string tempText = null;
        if (Utils.Internet.IsOnline())
        {
            WWW load = new WWW(contentDB_Url);
            yield return load;
            if (File.Exists(contentPath))
                File.Delete(contentPath);
            StreamWriter writer = new StreamWriter(contentPath, true);
            writer.Write(load.text);
            writer.Close();
        }

        if (File.Exists(contentPath))
        {
            //WWW www = new WWW(contentPath);
            //yield return www;
            var www = File.ReadAllText(contentPath);
            tempText = www;
        }
        else
        {
            var v = Resources.Load("Content");
            tempText = v.ToString();

        }
        var temp = CSVReader.SplitCsvGrid(tempText);//www.text);
        foreach (var item in Enumerable.Range(2, temp.GetLength(0)))
        {
            if (temp[item, 0] == null)
            {
                break;
            }
            List<DB_Items.Content_Item> tempList = new List<DB_Items.Content_Item>();
            this.PrintLog("Epmty Temp list count = " + tempList.Count + "\ntemp second arr = " + temp.GetLength(1));
            foreach (int j in Enumerable.Range(1, temp.GetLength(1)))
            {

                if (temp[0, j] == null)
                {
                    break;
                }
                DB_Items.Content_Item content_Item = new DB_Items.Content_Item();

                content_Item.sex = FillField (temp[0, j]);

                content_Item.level = FillField (temp[1, j]);

                content_Item.value = FillField (temp[item, j]);

                tempList.Add(content_Item);
            }
            if (!string.IsNullOrEmpty(temp[item,0]))
                content_dictionaty.Add(temp[item, 0], tempList);
        }
        content_Loaded = true;
    }

*/