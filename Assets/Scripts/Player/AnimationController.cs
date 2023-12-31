using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private string _idleAnimation;
    [SerializeField] private string _runAnimation;
    [SerializeField] private string _jumpAnimation;
    [SerializeField] private string _rollAnimation;
    [SerializeField] private string _stumbleAnimation;

    public void SwitchToIdle()
    {
        _animator.Play(_idleAnimation);
    }

    public void SwitchToRun()
    {

        _animator.SetTrigger(_runAnimation);
    }

    public void SwitchToJump()
    {
        _animator.SetTrigger(_jumpAnimation);
    }

    public void SwitchToRoll()
    {
        _animator.SetTrigger(_rollAnimation);
    }

    public void SwitchToStumble()
    {
        _animator.Play(_stumbleAnimation);
    }
}
