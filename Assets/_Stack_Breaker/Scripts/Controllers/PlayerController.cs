using System.Collections;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { private set; get; }


    [Header("Player Config")]
    [SerializeField] private float jumpUpVelocity = 15;
    [SerializeField] private float fallDownVelocity = -50;
    [SerializeField] private float minScale = 0.85f;
    [SerializeField] private float maxScale = 1.25f;
    [SerializeField] private float scalingFactor = 2;
    [SerializeField] private float timeCountToEnableImmortalMode = 0.3f;
    [SerializeField] private float immortalModeTime = 3f;

    [Header("Player References")]
    [SerializeField] private MeshRenderer meshRender = null;
    [SerializeField] private Material playerMaterial = null;
    [SerializeField] private GameObject trailEffect = null;
    [SerializeField] private ParticleSystem explodeEffect = null;
    [SerializeField] private ParticleSystem fireEffect = null;

    public float TargetY { private set; get; }
    public float ImmortalModeTime { get { return immortalModeTime; } }


    private Vector3 originalScale = Vector3.zero;
    private float lastSavedYPos = 0;
    private float currentJumpVelocity = 0;
    private float closestYAxis = -1;
    private float currentTimeCount = 0;
    private bool isPaused = false;
    private bool isTouchingScreen = false;
    private bool isImmortal = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(Instance.gameObject);
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        isImmortal = false;
        explodeEffect.gameObject.SetActive(false);
        fireEffect.gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = transform.position + Vector3.up * (currentJumpVelocity * Time.deltaTime + fallDownVelocity * Time.deltaTime * Time.deltaTime / 2);
        if (currentJumpVelocity < fallDownVelocity)
            currentJumpVelocity = fallDownVelocity;
        else
            currentJumpVelocity = currentJumpVelocity + fallDownVelocity * Time.deltaTime;
        if (currentJumpVelocity < 0)
        {
            Vector3 scale = transform.localScale;
            if (scale.x > minScale)
            {
                scale.x -= scalingFactor * Time.deltaTime;
            }
            else
                scale.x = minScale;
            transform.localScale = scale;
            float bottomY = (transform.position + Vector3.down * (meshRender.bounds.size.y / 2f)).y;
            float distance = bottomY - closestYAxis;
            if (distance <= 0.1f)
            {
                TargetY = closestYAxis;
                if (isImmortal) //Is on immortal mode
                {
                    if (!isTouchingScreen)
                    {
                        currentJumpVelocity = jumpUpVelocity;

                        Vector3 splashesPos = new Vector3(transform.position.x, closestYAxis + 0.05f, transform.position.z);
                    }
                    else
                    {
                        closestYAxis = -1;
                    }
                }
                else
                {
                    if (!isTouchingScreen)
                    {
                        currentTimeCount = Mathf.Clamp(currentTimeCount - Time.deltaTime, 0, timeCountToEnableImmortalMode);
                        currentJumpVelocity = jumpUpVelocity;

                        //Create splashes effects
                        Vector3 splashesPos = new Vector3(transform.position.x, closestYAxis + 0.05f, transform.position.z);
                    }
                    else
                    {
                        currentTimeCount += Time.deltaTime;
                        if (currentTimeCount >= timeCountToEnableImmortalMode)
                        {
                            currentTimeCount = timeCountToEnableImmortalMode;
                            StartCoroutine(CRCountingImmortalMode());
                        }

                        closestYAxis = -1;
                    }
                }
            }
        }
        else
        {
            Vector3 scale = transform.localScale;
            if (scale.x < maxScale)
            {
                scale.x += scalingFactor * Time.deltaTime;
            }
            else
                scale.x = maxScale;
            transform.localScale = scale;

        }


        if (Input.GetMouseButton(0))
        {
            isTouchingScreen = true;
        }

        if (Input.GetMouseButtonUp(0) && isTouchingScreen)
        {
            isTouchingScreen = false;
        }

    }

    private void Player_Living()
    {

        if (!isPaused)
        {
            meshRender.enabled = true;
            explodeEffect.gameObject.SetActive(false);
            fireEffect.gameObject.SetActive(false);
            trailEffect.SetActive(true);
            currentTimeCount = 0;
            transform.position = new Vector3(transform.position.x, lastSavedYPos, transform.position.z);
        }
    }

    private void Player_Died()
    {
        isTouchingScreen = false;
        meshRender.enabled = false;
        explodeEffect.gameObject.SetActive(true);
        explodeEffect.Play();
        trailEffect.SetActive(false);
        currentTimeCount = 0;
    }



    /// <summary>
    /// Wait a delay time and call Ingame states.
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>

    /// <summary>
    /// Counting to disable immortal mode.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRCountingImmortalMode()
    {
        isImmortal = true;
        fireEffect.gameObject.SetActive(true);
        fireEffect.Play();
        float timeTemp = immortalModeTime;
        while (timeTemp > 0)
        {
            yield return null;
            timeTemp -= Time.deltaTime;
            currentTimeCount = 0;
        }

        currentTimeCount = 0;
        isImmortal = false;
        fireEffect.Stop(true);
        yield return new WaitForSeconds(1f);
        fireEffect.gameObject.SetActive(false);
    }





    /// <summary>
    /// Set player coor by given color.
    /// </summary>
    /// <param name="color"></param>
    public void SetPlayerColor(Color color)
    {
        playerMaterial.color = color;
    }


}
