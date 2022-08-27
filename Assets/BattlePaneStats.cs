using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePaneStats : MonoBehaviour
{
    public int columnPowerTotal = 0;

    void Start(){
        
    }
    public void UpdateGraphics(Transform battleColumn){
        BattleColumn battleColumnStats = battleColumn.gameObject.GetComponent<BattleColumn>();
        this.gameObject.transform.GetChild(battleColumn.GetSiblingIndex()).GetComponentInChildren<Text>().text = battleColumnStats.CalculateColumnStats().ToString();
    }
}
