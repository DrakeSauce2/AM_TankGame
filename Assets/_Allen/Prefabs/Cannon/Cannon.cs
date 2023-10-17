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

    [SerializeField] private Vector3 offset;
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

        if (guide == null) return;

        if (Physics.Raycast(barrelEnd.position + offset, barrelEnd.forward + offset, out RaycastHit hitInfo, 10000f, layerMask))
        {
            Vector3 point = hitInfo.point;

            guide.SetCrosshair(CameraRig.Instance.GetActiveCamera().WorldToScreenPoint(point));
        }
        else
        {
            guide.SetCrosshair(CameraRig.Instance.GetActiveCamera().WorldToScreenPoint(new Vector3(barrelEnd.position.x + offset.x + barrelEnd.forward.x * 100f, 
                                                                                                   barrelEnd.position.y + offset.y + barrelEnd.forward.y * 100f,
                                                                                                   barrelEnd.position.z + offset.z + barrelEnd.forward.z * 100f)));
        }
    }

    public void Shoot()
    {
        if (isReloading) return;

        Instantiate(shellPrefab, barrelEnd.position, barrelEnd.rotation);

        currentReloadTime = 0;

        if(reloadReticle)
            reloadReticle.SetActive(true);

        shotSound.Play();

        shotParticle.Play();
    }

    private void Reload()
    {
        currentReloadTime += Time.deltaTime;

        ReloadGUI();

        if (currentReloadTime >= reloadTime)
        {           
            isReloading = false;
        }
        else
        { 
            isReloading = true;
        }
    }

    private void ReloadGUI()
    {
        if (!reloadFill || !reloadReticle) return;

        reloadFill.fillAmount = currentReloadTime / reloadTime;

        if (reloadFill.fillAmount >= 1f)
        {
            reloadReticle.SetActive(false);
        }

    }

}
