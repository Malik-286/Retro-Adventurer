using hardartcore.CasualGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth = 100;
    [SerializeField] AudioClip losehealthSound;
    public bool isAlive = true;
    [SerializeField] ParticleSystem playerSafeGuardParticles;

    [Header("Colors Variables")]
    [SerializeField] Color defaultColor;

    [Header("Player Health Damage Variables")]
    [SerializeField] float damageCooldown = 3.0f; // 3 to 4 seconds delay
    private bool canTakeDamage = true; // Flag to control damage cooldown

    SpriteRenderer spriteRenderer;
     TimerPanel timerPanel;
  //  public GameObject deathPanel;
  //  public GameObject detailsPanel;
    ScreenShake screenShake;

    public bool isDeathPanelActive;


    bool isGamePaused = false;


    GamePlayUI gamePlayUI;

    void Start()
    {
        gamePlayUI = FindObjectOfType<GamePlayUI>();
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
         timerPanel = FindObjectOfType<TimerPanel>();
    //   deathPanel.SetActive(false);
        screenShake = FindObjectOfType<ScreenShake>();
        isDeathPanelActive = false;
    }


    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void IncreaseHealth(int amountToIncrease)
    {
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            return;
        }
        else
        {
            currentHealth += amountToIncrease;
        }
        
    }

    public void DecreaseHealth(int amountToDecrease)
    {
        currentHealth -= amountToDecrease;

        if(screenShake != null)
        {
            screenShake.source.GenerateImpulse();
        }
 
        if (this.currentHealth <= 0)
        {
            currentHealth = 0;
            isAlive = false;
                       
        }
    }

    bool breakss = true;

    void Update()
    {

        if (isAlive == false && !isDeathPanelActive)
        {
            isDeathPanelActive = true;
            gameObject.GetComponent<Animator>().SetBool("isDead", true);

            if (breakss)
            {
                Invoke(nameof(ShowDeathpanel), 3f);
                breakss = false;
            }
        }
        else if (isAlive && isDeathPanelActive == true)
        {

             // Revive logic
            isDeathPanelActive = false;
            gameObject.GetComponent<Animator>().SetBool("isDead", false);

            // Ensure all other animations are reset
            gameObject.GetComponent<Animator>().SetBool("isRunning", false);
            gameObject.GetComponent<Animator>().SetBool("isJumping", false);

            // Set idle state
            gameObject.GetComponent<Animator>().SetBool("isIdeling", true);

            // Optionally, reset player position or other states

            gameObject.GetComponent<PlayerController>().StopMoving();
            gameObject.GetComponent<PlayerController>().MoveLeftPressed();
            gameObject.GetComponent<PlayerController>().MoveRightPressed();
            gameObject.GetComponent<PlayerController>().StopMoving();

            gameObject.GetComponent<Animator>().SetBool("isIdeling", true);
        }

    }

    public void ShowDeathpanel()
    {
        gamePlayUI.deathPanel.GetComponent<Dialog>().ShowDialog();

        if (gamePlayUI.deathPanel.activeInHierarchy)
        {
            gamePlayUI.detailsPanel.SetActive(false);
            return;
        }
        gamePlayUI.detailsPanel.SetActive(true);

        if (GameManager.GetInstance() != null)
        {
            GameManager.GetInstance().ReloadGame();
        }
        Time.timeScale = 1;

        timerPanel.RestTime();
        Destroy(gameObject, 1f);
    }



    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy") && GetComponent<PlayerHealth>().enabled && canTakeDamage)
        {
            DecreaseHealth(25);
            spriteRenderer.color = Color.red;
            StartCoroutine(ResetPlayerColor());

            if (isAlive)
            {
                AudioManager.GetInstance().PlaySingleShotAudio(losehealthSound, 0.6f);
            }

            // Start cooldown
            StartCoroutine(DamageCooldown());
        }
    }


    
    IEnumerator ResetPlayerColor()
    {      
            yield return new WaitForSeconds(0.40f);
            spriteRenderer.color = defaultColor;
        
    }

    IEnumerator EnableAndDisablePlayerHealth()
    {
        GetComponent<PlayerHealth>().enabled = false;
        playerSafeGuardParticles.Play();
        yield return new WaitForSeconds(4f);
        GetComponent<PlayerHealth>().enabled = true;
        playerSafeGuardParticles.Stop();

    }

    public void EnableAndDisablePlayerHealthComponent()
    {
        StartCoroutine(EnableAndDisablePlayerHealth());
    }

    IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

}
