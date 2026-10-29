using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InventaryControl : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            // Se crea el slot (no el item) dentro del panel del inventario
            GameObject slotObject = Instantiate(slotPrefab, inventoryPanel.transform);
            Slot slot = slotObject.GetComponent<Slot>();

            // Solo se crea un item si existe un prefab para esta posición
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
    }
}