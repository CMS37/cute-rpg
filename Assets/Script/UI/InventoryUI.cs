using UnityEngine;
using UnityEngine.UI;
using TMPro;                  // ← 추가
using System.Collections.Generic;
using Managers;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel;
    public GameObject bagSlotPrefab;
    public Transform bagSlotParent;

    private void Start()
    {
        panel.SetActive(false);
        GameManager.Instance.inputManager.onInventoryToggle += ToggleInventory;
    }
    private void OnDestroy()
    {
        GameManager.Instance.inputManager.onInventoryToggle -= ToggleInventory;
    }
    private void ToggleInventory()
    {
        panel.SetActive(!panel.activeSelf);
        if (panel.activeSelf) RefreshBag();
    }

    public void RefreshBag()
    {
        foreach (Transform t in bagSlotParent)
            Destroy(t.gameObject);

        var inv = InventoryManager.Instance.GetInventory();
        foreach (var (data, count) in inv)
        {
            var slot = Instantiate(bagSlotPrefab, bagSlotParent);
            // 아이콘 세팅
            slot.transform.Find("Icon").GetComponent<Image>().sprite = data.icon;

            // TextMeshProUGUI로 가져오기
            var countText = slot.transform.Find("Count")
                                 .GetComponent<TextMeshProUGUI>();
            countText.text = count.ToString();

            // 클릭 이벤트
            string id = data.id;
            slot.GetComponent<Button>().onClick.AddListener(() =>
                InventoryManager.Instance.Use(id));
        }
    }
}
