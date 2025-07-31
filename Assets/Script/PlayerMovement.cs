using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    
    [Header("减速效果")]
    [SerializeField] private float slowEffectDuration = 3f;
    [SerializeField] private float slowEffectAmount = 0.5f;
    
    // 私有变量
    private float currentMoveSpeed;
    private bool isSlowed = false;
    private Coroutine slowCoroutine;
    
    void Start()
    {
        currentMoveSpeed = moveSpeed;
    }
    
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }
    
    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        
        if (movement.magnitude > 0.1f)
        {
            transform.Translate(movement * currentMoveSpeed * Time.deltaTime, Space.World);
        }
    }
    
    private void HandleRotation()
    {
        // 获取鼠标输入进行旋转
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
    }
    
    public void ApplySlowEffect(float slowAmount, float duration)
    {
        // 如果已经有减速效果，先停止之前的
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }
        
        slowCoroutine = StartCoroutine(SlowEffectCoroutine(slowAmount, duration));
    }
    
    private IEnumerator SlowEffectCoroutine(float slowAmount, float duration)
    {
        isSlowed = true;
        float originalSpeed = currentMoveSpeed;
        
        // 应用减速
        currentMoveSpeed *= slowAmount;
        
        // TODO: 播放减速音效
        // AudioManager.Instance.PlaySound("slow_effect", transform.position);
        
        // 等待减速持续时间
        yield return new WaitForSeconds(duration);
        
        // 恢复速度
        currentMoveSpeed = originalSpeed;
        isSlowed = false;
        
        // TODO: 播放恢复音效
        // AudioManager.Instance.PlaySound("speed_recovery", transform.position);
    }
    
    public bool IsSlowed()
    {
        return isSlowed;
    }
    
    public float GetCurrentSpeed()
    {
        return currentMoveSpeed;
    }
} 