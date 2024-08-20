using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldMain : MonoBehaviour
{
    // Alt�n miktar�n� tutan property
    private int _gold;
    public int Gold
    {
        get { return _gold; }
        private set
        {
            _gold = value;
            OnGoldChanged?.Invoke(_gold);  // Gold de�i�ti�inde event'i tetikle
            UpdateGoldUI();
        }
    }

    // Gold miktar�n� g�sterecek UI Text eleman�
    public Text goldText;

    // Alt�n de�i�ti�inde �a�r�lacak event
    public event Action<int> OnGoldChanged;

    void Start()
    {
        // Ba�lang�� alt�n miktar�n� belirle
        Gold = 2000; // Ba�lang�� de�eri �rnek olarak 1000
        UpdateGoldUI();
    }

    // Alt�n miktar�n� art�ran metod
    public void AddGold(int amount)
    {
        if (amount < 0) return;
        Gold += amount;
    }

    // Alt�n miktar�n� azaltan metod
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
            Debug.LogError("Yetersiz Alt�n!");
            return false;
        }
    }

    // Alt�n miktar�n� UI'da g�ncelleyen metod
    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + Gold.ToString();
        }
    }

    // Geriye kalan alt�n miktar�n� d�nd�ren metod
    public int GetGoldAmount()
    {
        return Gold;
    }
}
