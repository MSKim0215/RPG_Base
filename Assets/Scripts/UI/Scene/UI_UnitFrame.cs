using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitFrame : UI_Scene
{
    private enum Texts
    {
        Text_Level
    }

    public override void Init()
    {
        if (isInit) return;
        isInit = true;

        base.Init();

        Bind<Text>(typeof(Texts));

        //// TODO: 사용자의 인벤토리 정보 세팅 진행
        //for(int i = 0; i < 8; i++)
        //{
        //    GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(GetObject((int)GameObjects.ItemBag).transform).gameObject;
        //    UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>();
        //    invenItem.SetInfo($"집행검 +{i}");
        //    invenItem.Init();
        //}
    }

    public void SetLevel(int _level)
    {
        if (!isInit) Init();
        GetText((int)Texts.Text_Level).text = _level.ToString();
    }
}