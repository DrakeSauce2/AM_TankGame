using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform barrelEnd;
    [SerializeField] private ParticleSystem shotParticle;

    [Space]

    [SerializeField] private AudioSource shotSound;

    [Space]

    [SerializeField] private GameObject reloadReticle;
    [SerializeField] private Image reloadFill;
    [SerializeField] private float reloadTime;
    private float currentReloadTime = 0;
    bool isReloading = false;

    [Space]
   
    [SerializeField] private GameObject shellPrefab;

    [Space]

    [SerializeField] private LayerMask layerMask;

    private TargetGuide guide;
    private Shell shell;

    private void Awake()
    {
        shell = shellPrefab.GetComponent<Shell>();
        guide = GetComponent<TargetGuide>();

        currentReloadTime = reloadTime + 1;
    }

    private void Update()
    {
        Reload();

        if (Physics.Raycast(barrelEnd.position, barrelEnd.forward, out RaycastHit hitInfo, 10000f, layerMask))
        {
            Vector3 point = hitInfo.point;

            guide.SetCrosshair(Camera.main.WorldToScreenPoint(point));
        }
        else
        {
            guide.SetCrosshair(Camera.main.WorldToScreenPoint(new Vector3(barrelEnd.position.x + barrelEnd.forward.x * 100f, 
                                                                          barrelEnd.position.y + barrelEnd.forward.y * 100f,
                                                                          barrelEnd.position.z + barrelEnd.forward.z * 100f)));
        }
    }

    public void Shoot()
    {
        if (isReloading) return;

        Instantiate(shellPrefab, barrelEnd.position, barrelEnd.rotation);

        currentReloadTime = 0;
        reloadReticle.SetActive(true);

        shotSound.Play();

        shotParticle.Play();
    }

    private void Reload()
    {
        currentReloadTime += Time.deltaTime;
        reloadFill.fillAmount = currentReloadTime/reloadTime;

        if (currentReloadTime >= reloadTime)
        {
            reloadReticle.SetActive(false);
            isReloading = false;
        }
        else
        { 
            isReloading = true;
        }
    }

}
