using UnityEngine;
using UnityEngine.UI;

public class PerformanceMetrics : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text fpsText;
    [SerializeField] private Text frameTimeText;
    [SerializeField] private Text qualityText;
    [SerializeField] private Text memoryText;
    
    private float _timer = 0f;
    private float _updateInterval = 0.5f;
    
    private void Update()
    {
        _timer += Time.deltaTime;

        if (!(_timer >= _updateInterval)) return;
        
        UpdateMetrics();
        
        _timer = 0f;
    }
    
    private void UpdateMetrics()
    {
        // FPS
        float fps = 1.0f / Time.deltaTime;
        fpsText.text = $"FPS: {Mathf.Round(fps)}";
        
        // Frame Time (ms)
        float frameTime = Time.deltaTime * 1000f;
        frameTimeText.text = $"Frame Time: {frameTime:F1}ms";
        
        // Quality Settings
        string quality = QualitySettings.names[QualitySettings.GetQualityLevel()];
        qualityText.text = $"Quality: {quality}";

        // Memory Usage (MB)
        float memoryUsage = System.GC.GetTotalMemory(false) / (1024f * 1024f);
        memoryText.text = $"Memory: {memoryUsage:F1} MB";
    }
}