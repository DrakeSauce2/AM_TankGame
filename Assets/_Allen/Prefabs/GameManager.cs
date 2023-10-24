using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player UI")]
    [SerializeField] private GameObject viewShroud;
    [SerializeField] private GameObject repairingText;
    public GameObject BarrelShroud { get { return viewShroud; } }
    [Space]
    [SerializeField] private GameObject reloadReticle;
    [SerializeField] private Image reloadFill;
    public GameObject ReloadReticle { get { return reloadReticle; } }
    public Image ReloadFill { get { return reloadFill; } }

    [Space]
    [SerializeField] private RectTransform crossHair;
    public RectTransform crossHairTarget { get { return crossHair; } }

    [Header("Player HotBar")]
    [SerializeField] private GameObject hotBarPrefab;
    [SerializeField] private Transform hotBarAttachPoint;
    public GameObject HotBarPrefab
    {
        get { return hotBarPrefab; }
    }
    public Transform HotBarAttachPoint
    {
        get { return hotBarAttachPoint; }
    }

    [Header("Selection Screen")]
    [SerializeField] private GameObject selectionScreen;

    [Header("Test Buttons")]
    [SerializeField] private Button spawnEnemy;

    [Header("Death Model")]

    [SerializeField] private GameObject deadTankPrefab;
    public GameObject DeadTankPrefab { get { return deadTankPrefab; } }

    [Header("Test Destroy Tank Components")]

    [SerializeField] private List<TankComponent> playerTankComponents = new List<TankComponent>();

    [Header("Test Buttons Screen")]
    [SerializeField] private GameObject testScreen;
    bool isOpen = false;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleTestScreen();           
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void ToggleTestScreen()
    {
        isOpen = !isOpen;

        testScreen.SetActive(isOpen);
        Cursor.visible = isOpen;
    }

    public void SetPlayerTankComponents(List<TankComponent> compsToAdd)
    {
        playerTankComponents = compsToAdd;
    }

    public void DestroyTankComponents()
    {
        foreach (TankComponent comp in playerTankComponents)
        {
            comp.DisableComponent();
        }

        for (int i = 0; i < playerTankComponents.Count; i++)
        {
            playerTankComponents[i].DisableComponent();
        }
    }

    public void Reparing(bool state)
    {
        repairingText.SetActive(state);
    }

}
