using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMapScrollView : MonoBehaviour
{
    [Serializable]
    internal struct MapTemplate
    {
        public string name;
        public Sprite sprite;
    }

    [SerializeField] private GameObject mapSlotPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private MapTemplate[] mapTemplates;

    private void Start()
    {
        foreach (var template in mapTemplates)
        {
            GameObject mapSlotObj = Instantiate(mapSlotPrefab, content);
            MapSlot mapSlot = mapSlotObj.GetComponent<MapSlot>();
            mapSlot.MapName = template.name;
            Image image = mapSlotObj.GetComponent<Image>();
            image.sprite = template.sprite;
        }
    }
}
