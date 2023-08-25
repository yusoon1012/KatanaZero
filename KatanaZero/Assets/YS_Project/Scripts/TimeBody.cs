using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeBody : MonoBehaviour
{

    public bool isRewindin = false;
    List<Vector3> positions;
    List<Vector3> scales;
    List<AnimatorClipInfo[]> animations;
    List<string> animationNames;
    public Animator animator;
    public bool isRewindOver = false;
    private bool isReplay = false;
    private bool rewindSoundOn = false;
    int positionIdx=0;
    SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animations=new List<AnimatorClipInfo[]>();
        positions =new List<Vector3>();
        soundManager = FindAnyObjectByType<SoundManager>();
        scales =new List<Vector3>();
        animationNames = new List<string>();
    }

   
    // Update is called once per frame
    void Update()
    {
        positionIdx=positions.Count;
        if (Input.GetKey(KeyCode.R))
        {
            StartRewind();
            
        }
        if(Input.GetKeyUp(KeyCode.R))
        {
            StopRewind();
        }
        if (isRewindin)
        {
           StartCoroutine( RewindRoutine());
        }
      
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Replay();
        }
        
        if(isRewindOver)
        {
            StartCoroutine(RewindOverReset());
        }

       

    }
    private void FixedUpdate()
    {

        if (isRewindin == false && isReplay == false)
        {
            Record();
        }

    }
    void Rewind()
    {
        if(rewindSoundOn==false)
        {
            rewindSoundOn=true;
            soundManager.RewindSound();
        }
        Time.timeScale = 2f;
        if(positions.Count > 0)
        {
            Debug.Log("�����ε� ī��Ʈ��");
            transform.position = positions[0];
            transform.localScale = scales[0];
            positions.RemoveAt(0);
            scales.RemoveAt(0);
            AnimatorClipInfo[] animClipInfos = animations[0];
            animations.RemoveAt(0);
            if (animClipInfos.Length > 0)
            {
                AnimatorClipInfo animClipInfo = animClipInfos[0];
                AnimationClip animClip = animClipInfo.clip;

                // Calculate the time to start playing the animation from
                float reversedTime = animClip.length - animClipInfo.clip.length * animClipInfo.weight;

                // Play the animation in reverse
                animator.Play(animClip.name, 0, reversedTime);
            }
        }
        else if(positions.Count<=1)
        {

            Debug.Log("�����ε� ī��Ʈ��");
            Time.timeScale = 1;
            isRewindOver = true;
            StopRewind();
        }
    }
    void Record()
    {
        positions.Insert(0 ,transform.position);
        scales.Insert(0, transform.localScale);
        animations.Insert(0, animator.GetCurrentAnimatorClipInfo(0));
       
        
    }
    void StartReplay()
    {
        if(isReplay==false)
        {
            isReplay = true;
        }
    }
    void Replay()
    {


        if(isReplay==false)
        {
            isReplay = true;
            StartCoroutine(RePlay_IEnum());
        }
        
    }
    void StartRewind()
    {
        isRewindin=true;
    }
    void StopRewind()
    {
        isRewindin = false;
        Time.timeScale = 1f;


    }
    private IEnumerator RewindRoutine()
    {
        Time.timeScale = 3f;
        yield return new WaitForSeconds(0.00001f);
        Rewind();
        //Time.timeScale = 1f;

    }
    private IEnumerator RewindOverReset()
    {
        yield return new WaitForSeconds(1);
        isRewindOver = false;
    }
    private IEnumerator RePlay_IEnum()
    {
        //Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        //if(rigidBody!=null)
        //{
        //    rigidBody.gravityScale = 0f;
        //}

        //for (int i = positions.Count - 1; i >= 0; i--)
        //{
        //    transform.position = positions[i];
        //    transform.localScale = scales[i];
        //    AnimatorClipInfo[] animClipInfos = animations[i];
        //    if (animClipInfos.Length > 0)
        //    {
        //        AnimatorClipInfo animClipInfo = animClipInfos[0];
        //        AnimationClip animClip = animClipInfo.clip;

        //        // �ִϸ��̼� ���
        //        animator.Play(animClip.name,0, Time.deltaTime);
        //    }
        //    // float delay = 0.01f;
        //    yield return new WaitForSeconds(Time.deltaTime*6f);
        //}
        //Time.timeScale =1f;
        //rigidBody.gravityScale = 1f;
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody != null)
        {
            rigidBody.gravityScale = 0f;
        }

        // ��ϵ� �����͸� �ݺ��մϴ�.
        for (int i = 0; i < positions.Count; i++)
        {
            transform.position = positions[i];
            transform.localScale = scales[i];
            AnimatorClipInfo[] animClipInfos = animations[i];

            if (animClipInfos.Length > 0)
            {
                AnimatorClipInfo animClipInfo = animClipInfos[0];
                AnimationClip animClip = animClipInfo.clip;

                // Calculate the time to start playing the animation from
                float startTime = animClip.length * Time.deltaTime;

                // Play the animation from the calculated startTime
                animator.Play(animClip.name, 0, startTime);
            }

            // ��� �ӵ��� �����ϱ� ���� ª�� �ð� ���� ����մϴ�.
            yield return new WaitForSeconds(0.1f); // �ʿ��� ��� �ð��� ���� ���� �����ϼ���
        }

        Time.timeScale = 1f;

        if (rigidBody != null)
        {
            rigidBody.gravityScale = 1f;
        }

        // isReplay �÷��׸� �缳���մϴ�.
        isReplay = false;
    }

}
