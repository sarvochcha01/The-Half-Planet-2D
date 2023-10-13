using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmoCountManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentAmmoText;
    [SerializeField] private TextMeshProUGUI magCapText;
    [SerializeField] private TextMeshProUGUI totalAmmoText;
    [SerializeField] private TextMeshProUGUI slashText;
    [SerializeField] private TextMeshProUGUI gunTypeText;

    [SerializeField] private Shooting gunScript;

    [SerializeField] private Slider reloadSlider;

    [SerializeField] private Transform holder;

    [SerializeField] private Transform gun;

    [SerializeField] private Animator animator;

    [SerializeField] private bool isShooting;
    [SerializeField] private bool isHolderLocated;

    private void Start()
    {
        isHolderLocated = false;
        
        reloadSlider = GameObject.FindGameObjectWithTag("ReloadProgressSlider").GetComponent<Slider>();
    }
    private void Update()
    {
        if (FindObjectOfType<GameManager>().IsGameOver()) return;

        if (!isHolderLocated)
        {
            holder = GameObject.Find("GunHolder").transform;
            isHolderLocated = true;
        }

        if (!holder.GetComponent<Holder>().GetIsEmpty())
        {
            ShowAmmoUI();
            gun = holder.GetChild(0).gameObject.transform;
            gunScript = gun.GetComponent<Shooting>();
            UpdateAmmoCountUI(gunScript.CurrentMagazineAmmoCount(), gunScript.GetTotalAmmoCount(), gunScript.GetGunType());
            reloadSlider.maxValue = gunScript.GetReloadDuration();

            if (gunScript.IsReloading())
            {
                reloadSlider.gameObject.SetActive(true);
                reloadSlider.value += Time.deltaTime;
            }
            else
            {
                reloadSlider.gameObject.SetActive(false);
                reloadSlider.value = 0f;
            }

            if (gunScript.IsShooting())
            {
                AnimateAmmo();
            }
        }
        else
        { 
            HideAmmoUI();
        }
    }

    private void AnimateAmmo()
    {
        animator.Play("AmmoCountAnimChange");
    }

    private void HideAmmoUI()
    {
        currentAmmoText.transform.gameObject.SetActive(false);
        gunTypeText.transform.gameObject.SetActive(false);
        totalAmmoText.transform.gameObject.SetActive(false);
        reloadSlider.gameObject.SetActive(false);
    }

    private void ShowAmmoUI()
    {
        currentAmmoText.transform.gameObject.SetActive(true);
        gunTypeText.transform.gameObject.SetActive(true);
        totalAmmoText.transform.gameObject.SetActive(true);

    }

    public void UpdateCurrentAmmoText(string text)
    {
        currentAmmoText.text = text;
    }

    public void UpdateMagCapText(string text)
    {
        magCapText.text = text;
    }

    public void UpdateTotalAmmoText(string text)
    {
        totalAmmoText.text = text;
    }

    public void UpdateGunTypeText(string text)
    {
        gunTypeText.text = text;
    }

    private void UpdateAmmoCountUI(int currentMagazineAmmoCount, int totalAmmoCount, string type)
    {
        UpdateCurrentAmmoText(currentMagazineAmmoCount.ToString());
        UpdateTotalAmmoText(totalAmmoCount.ToString());
        UpdateGunTypeText(type);

    }
}
