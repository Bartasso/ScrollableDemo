using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Images_Logic : MonoBehaviour
{
    public GameObject cellPrefab;
    public Button updateButton;
    private List<Cell> cells = new List<Cell>();

    private class Cell
    {
        public Material ImageMaterial;
        public string Name;
        public string Time;   
    }

    void Start()
    {
        cells.Clear();
        RefreshDirectory();
        updateButton.onClick.AddListener(UpdateClick);
    }

    public void RefreshDirectory()
    {
        //var imagesDataList = new List<string>(Directory.GetFiles(@"C:\Scrollable"));
        var imagesDataList = new List<string>(Directory.GetFiles(@"Assets\Images","*.png"));
        foreach (string item in imagesDataList)
        {   
            var texture = new Texture2D(2, 2);
            texture.LoadImage(File.ReadAllBytes(item));
            texture.Apply();
            var material = new Material(Shader.Find("UI/Default"));
            material.mainTexture = texture;
            var Interval = DateTime.Now - File.GetCreationTime(item);
            var cell = new Cell() { ImageMaterial = material, Name = Path.GetFileName(item), Time = (Interval.Days +" Days, " + Interval.Hours + " Hours, and " + Interval.Seconds + " Seconds" ).ToString() };
            if (cells.Exists(x=> x.Name == cell.Name))
            {
                return;
            }
            else
            {
                cells.Add(cell);
                CreateCell(cells.IndexOf(cell));
            }
        }
    }

    public void CreateCell(int i)
    {
        GameObject cellObject = new GameObject();
        GameObject imageObject = new GameObject();
        GameObject timeObject = new GameObject();
        GameObject nameObject = new GameObject();

        cellObject.name = "Cell";
        imageObject.name = "Image";
        timeObject.name = "Time";
        nameObject.name = "Name";

        cellObject.transform.SetParent(gameObject.transform);
        imageObject.transform.SetParent(cellObject.transform);
        timeObject.transform.SetParent(cellObject.transform);
        nameObject.transform.SetParent(cellObject.transform);

        cellObject.AddComponent<RectTransform>();
        imageObject.AddComponent<Image>();
        timeObject.AddComponent<TextMeshProUGUI>();
        nameObject.AddComponent<TextMeshProUGUI>();

        imageObject.GetComponent<Image>().material = cells[i].ImageMaterial;
        timeObject.GetComponent<TextMeshProUGUI>().text = "Time since creation: " + cells[i].Time;
        nameObject.GetComponent<TextMeshProUGUI>().text = "Name: " + cells[i].Name;
        timeObject.GetComponent<TextMeshProUGUI>().fontSize = 20;
        nameObject.GetComponent<TextMeshProUGUI>().fontSize = 20;

        imageObject.GetComponent<RectTransform>().position = new Vector3(-190, 0, 0);
        timeObject.GetComponent<RectTransform>().position = new Vector3(50, -20, 0);
        nameObject.GetComponent<RectTransform>().position = new Vector3(50, 20, 0);

        timeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 50);
        nameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 50);
    }

    public void DestroyAll()
    {
        foreach(Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateClick()
    {
        cells.Clear();
        DestroyAll();
        RefreshDirectory();
    }
}
