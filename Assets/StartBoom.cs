using UnityEngine;
using UnityEngine.UI; // 如果你要控制的是 Image 本身，需要引用這個，如果是 GameObject 則不用

public class StartBoom : MonoBehaviour
{
    [Header("目標設定")]
    [Tooltip("要控制的特效控制器腳本")]
    [SerializeField] private VFXStaggeredController targetVFXController;

    [Header("UI 設定")]
    [Tooltip("進入範圍時要顯示的 UI 物件 (例如 Canvas 裡的一個 Image 或 Text)")]
    [SerializeField] private GameObject interactionUI;

    [Header("參數")]
    [Tooltip("觸發互動的按鍵")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [Tooltip("玩家物件的 Tag 名稱")]
    [SerializeField] private string playerTag = "Player";

    // 用來標記玩家是否在範圍內
    private bool isPlayerInZone = false;

    private void Start()
    {
        // 遊戲開始時，先確保提示 UI 是隱藏的
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

    private void Update()
    {
        // 只有當玩家在範圍內，且按下了指定按鍵 (E)
        if (isPlayerInZone && Input.GetKeyDown(interactKey))
        {
            TriggerEffect();
        }
    }

    private void TriggerEffect()
    {
        if (targetVFXController != null)
        {
            // 呼叫上一份代碼的 PlayAllEffects
            // 建議改用 ResetAndPlay()，這樣如果不小心連按，特效會重置而不是疊加播放
            targetVFXController.ResetAndPlay();
            // 如果你堅持只用 PlayAllEffects，請改成 targetVFXController.PlayAllEffects();
        }
        else
        {
            Debug.LogWarning("VFXStaggeredController 未指派！");
        }
    }

    // 當有物體進入碰撞框 (Trigger)
    private void OnTriggerEnter(Collider other)
    {
        // 檢查進入的是不是玩家 (透過 Tag 判斷)
        if (other.CompareTag(playerTag))
        {
            isPlayerInZone = true;

            // 顯示提示 UI
            if (interactionUI != null)
            {
                interactionUI.SetActive(true);
            }
        }
    }

    // 當物體離開碰撞框
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInZone = false;

            // 隱藏提示 UI
            if (interactionUI != null)
            {
                interactionUI.SetActive(false);
            }
        }
    }
}