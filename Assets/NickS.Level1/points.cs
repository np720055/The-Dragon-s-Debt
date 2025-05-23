using UnityEngine;
using TMPro; // Use UnityEngine.UI if not using TextMeshPro

public class PointsDisplay : MonoBehaviour
{
    public StatUpgradeNPC statUpgradeNPC;
    public TextMeshProUGUI pointsText; // Use Text if you're using old UI system

    void Update()
    {
        if (statUpgradeNPC != null && pointsText != null)
        {
            pointsText.text = "" + statUpgradeNPC.playerPoints;
        }
    }
}
