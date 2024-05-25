using Logan;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyDamageLogic : MonoBehaviour
{
    public int initialHealth = 60;

    [SerializeField] private SoundManager soundManager;
    
    private int currentHealth;

    public bool IsAlive => currentHealth > 0;

    void Start()
    {
        currentHealth = initialHealth;
    }
    
    void Update()
    {
        if (IsAlive && Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            TakeDamage();
        }
    }
    
    public void TakeDamage()
    {
        currentHealth -= 3;
        if (IsAlive)
        {
            soundManager.PlayRandomPunchSound();   
        }
        else
        {
            currentHealth = 0;
            soundManager.PlayBoxingWinSound();
        }
    }
}
