using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnumClass;

public class RaceButton : MonoBehaviour
{
    public CharacterRace race;  //�ν����Ϳ��� �����ϱ�
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => {
            UIManager.Instance.ShowPopup<UICreateDeckPhase2>("UICreateDeckPhase2");
            ControllerRegister.Get<FilterController>().InitFilter(race);
            Debug.Log($"Selected Race: {race}");
        });
    }
}
