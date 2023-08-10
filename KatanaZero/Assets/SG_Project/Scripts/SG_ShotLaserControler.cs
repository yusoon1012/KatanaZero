using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SG_ShotLaserControler : MonoBehaviour
{

    public SG_PlayerMovement player;
    private SpriteRenderer spriteRenderer = null;

    // 컬러 파,노,파,노 로 바꾸는 코루틴 사용하려면 아래 주석 사용하면됨
    private Coroutine changeColor = default;
    // changeColor = StartCoroutine(ColorChange());

    private WaitForFixedUpdate waitForFixedUpdate = default;

    private Color blue = default;
    private Color yellow = default;


    private Vector3 defaultScale = default;
    private Vector3 ShrinkageScale = default;

    // Start is called before the first frame update
    public void Awake()
    {
        // 스프라이트 랜더러가 비어있다면 GetComponent 안비어있으면 Pass
        if (spriteRenderer == default || spriteRenderer == null)
        {
            spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        }
        else { /*PASS*/ }

        // 파란색 RGB가 들어가있지 않을때에만 색 RGB 기입
        if (blue == default || blue == null)
        {
            blue = new Color(40, 130, 220);
        }
        else { /*PASS*/ }

        // 노란색 RGB가 들어가 있지 않을때에만 색 RGB 기입
        if (yellow == default || yellow == null)
        {
            yellow = new Color(255, 180, 0);
        }
        else { /*PASS*/ }

        //  waitForFixedUpdate 가 비어있을때에 캐싱
        if (waitForFixedUpdate == default || waitForFixedUpdate == null)
        {
            waitForFixedUpdate = new WaitForFixedUpdate();
        }
        else { /*PASS*/ }

        // 레이저 쏘면서 줄어들 값 여러번 사용하기에 캐싱
        if (ShrinkageScale == default || ShrinkageScale == null)
        {
            ShrinkageScale = new Vector3(-0.2f, 0f, 0f);
        }
        else { /*PASS*/ }

   

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        // SetActive == true 될 때 수행할 작업

        changeColor = StartCoroutine(ColorChange());
    }

    void OnDisable()
    {
        // SetActive == flase 될 때 수행할 작업
        // 코루틴에서 줄어든 크기 False될떄에 복구
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public void LaserAttack()
    {

    }

    IEnumerator ColorChange()
    {

        //  {파,노,파,노 로직
        spriteRenderer.color = blue;
        for (int i = 0; i <= 3; i++)
        {
            yield return waitForFixedUpdate;
        }

        spriteRenderer.color = yellow;

        for (int j = 0; j <= 3; j++)
        {
            yield return waitForFixedUpdate;
        }

        spriteRenderer.color = blue;

        for (int j = 0; j <= 3; j++)
        {
            yield return waitForFixedUpdate;
        }
        //  }파,노,파,노 로직

        spriteRenderer.color = yellow;

        for (int j = 0; j <= 3; j++)
        {
            //this.gameObject.transform.localScale
            yield return waitForFixedUpdate;
        }



    }
}
