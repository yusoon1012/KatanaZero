using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPlayerLayerScript : StateMachineBehaviour
{

    bool firstRool = false;

    // �ִϸ��̼� ������ �ؽð��� ������ ����
    private int idleStateHash;
    private int rollStateHash;
    private int runStateHash;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���ο� ���·� ���� �� ����
        idleStateHash = Animator.StringToHash("Base Layer.Idle");
        rollStateHash = Animator.StringToHash("Base Layer.IsRoll");
        runStateHash = Animator.StringToHash("Base Layer.Run");

        if (stateInfo.fullPathHash == runStateHash)
        {
            Debug.Log("�ٴ°� ������ �������ϳ�?");
            animator.SetTrigger("ExitRun");
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ó���� ������ �������� ������ �� ������ ������ ����
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���°� ���� ���·� �ٲ�� ������ ����

        // "Roll" �ִϸ��̼� ���°� ���� �� Ư�� �۾��� �����մϴ�.
        if (stateInfo.fullPathHash == rollStateHash && firstRool == false)
        {
            firstRool = true;
            animator.Play("Run");
        }
      

    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // MonoBehaviour.OnAnimatorMove ���Ŀ� ����
    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // MonoBehaviour.OnAnimatorIK ���Ŀ� ����
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        // ��ũ��Ʈ�� ������ ���� ���� ��ȯ�� ������ ����
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        // ��ũ��Ʈ�� ������ ���� ��迡�� �������ö� ����
    }

}
