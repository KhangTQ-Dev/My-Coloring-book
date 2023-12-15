using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabLobby : MonoBehaviour
{
    [SerializeField] private Button btnTab;

    // Start is called before the first frame update
    void Start()
    {
        btnTab.onClick.AddListener(OnClickBtnTab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(bool isTrue)
    {

    }

    private void OnClickBtnTab()
    {

    }
}
