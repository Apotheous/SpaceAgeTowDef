using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldMain : MonoBehaviour
{
    // Altýn miktarýný tutan property
    private int _gold;
    public int Gold
    {
        get { return _gold; }
        private set
        {
            _gold = value;
            OnGoldChanged?.Invoke(_gold);  // Gold deðiþtiðinde event'i tetikle
            UpdateGoldUI();
        }
    }

    // Gold miktarýný gösterecek UI Text elemaný
    public Text goldText;

    // Altýn deðiþtiðinde çaðrýlacak event
    public event Action<int> OnGoldChanged;

    void Start()
    {
        // Baþlangýç altýn miktarýný belirle
        Gold = 2000; // Baþlangýç deðeri örnek olarak 1000
        UpdateGoldUI();
    }

    // Altýn miktarýný artýran metod
    public void AddGold(int amount)
    {
        if (amount < 0) return;
        Gold += amount;
    }

    // Altýn miktarýný azaltan metod
    public bool SpendGold(int amount)
    {
        if (amount < 0) return false;
        if (Gold >= amount)
        {
            Gold -= amount;
            return true;
        }
        else
        {
            Debug.LogError("Yetersiz Altýn!");
            return false;
        }
    }

    // Altýn miktarýný UI'da güncelleyen metod
    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + Gold.ToString();
        }
    }

    // Geriye kalan altýn miktarýný döndüren metod
    public int GetGoldAmount()
    {
        return Gold;
    }
}
