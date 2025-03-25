using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPUIManager : MonoBehaviour
{
    [SerializeField] private EnemyController ec;
    [SerializeField] private PlayerController pc;
    [SerializeField] private Slider HPSlider;
    [SerializeField] private Slider BulkSlider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public async UniTask HPViewer(int damage)
    {
        if (ec)
        {
            await BulkSlider.DOValue(ec.Hp - damage,0.5f);
            ec.Hp -= damage;
            if(ec.Hp <= 0)
            {
                ec.Dead();
            }
            HPSlider.DOValue(ec.Hp,1.0f); 
        }
        if (pc)
        {
            await BulkSlider.DOValue(pc.Hp - damage,0.5f);
            pc.Hp -= damage;
            if(pc.Hp <= 0)
            {
                pc.Dead();
            }
            HPSlider.DOValue(pc.Hp,1.0f); 
        }

    }

}
