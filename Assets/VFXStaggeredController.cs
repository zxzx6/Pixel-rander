using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXStaggeredController : MonoBehaviour
{
    [System.Serializable]
    public class VFXEntry
    {
        public GameObject effectObject;  // 特效物件
        public float delayTime;          // 延遲播放時間(秒)
    }

    [Header("特效列表")]
    [SerializeField] private List<VFXEntry> vfxEntries = new List<VFXEntry>();

    [Header("設定")]
    [SerializeField] private bool playOnStart = true;  // 啟動時自動播放

    private void Start()
    {
        if (playOnStart)
        {
            PlayAllEffects();
        }
    }

    /// <summary>
    /// 播放所有特效(依照設定的延遲時間)
    /// </summary>
    public void PlayAllEffects()
    {
        StartCoroutine(PlayEffectsWithDelay());
    }

    /// <summary>
    /// 停止所有特效
    /// </summary>
    public void StopAllEffects()
    {
        StopAllCoroutines();

        foreach (var entry in vfxEntries)
        {
            if (entry.effectObject != null)
            {
                entry.effectObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 重置並重新播放所有特效
    /// </summary>
    public void ResetAndPlay()
    {
        StopAllEffects();
        PlayAllEffects();
    }

    private IEnumerator PlayEffectsWithDelay()
    {
        // 先確保所有特效都是關閉的
        foreach (var entry in vfxEntries)
        {
            if (entry.effectObject != null)
            {
                entry.effectObject.SetActive(false);
            }
        }

        // 按照延遲時間依序開啟特效
        foreach (var entry in vfxEntries)
        {
            if (entry.effectObject == null) continue;

            // 等待延遲時間
            if (entry.delayTime > 0)
            {
                yield return new WaitForSeconds(entry.delayTime);
            }

            // 開啟特效GameObject
            entry.effectObject.SetActive(true);
        }
    }
}