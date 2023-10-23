using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public static Minimap Instance;

    [SerializeField] private Camera minimapCamera;
    public Camera MinimapCamera { get { return minimapCamera; } }

    [SerializeField] private RectTransform playerMinimap;
    public RectTransform PlayerMinimap { get {  return playerMinimap; } }

    [SerializeField] private Vector3 offset;
    public Vector3 Offset { get { return offset; } }

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }



}
