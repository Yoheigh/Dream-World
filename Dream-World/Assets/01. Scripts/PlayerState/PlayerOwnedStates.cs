using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerOwnedStates
{
    public class DefaultState : State<PlayerMovement>
    {
        public override void EnterState(PlayerMovement _entity)
        {
            _entity.InitTest();
            _entity.animator.SetBool("isClimbing", false);
            _entity.animator.SetBool("isHolding", false);
        }

        public override void UpdateState(PlayerMovement _entity)
        {
            _entity.GroundCheck();
            _entity.Gravity();
            _entity.Move();
        }

        public override void ExitState(PlayerMovement _entity)
        {

        }
    }

    public class FallingState : State<PlayerMovement>
    {
        public override void EnterState(PlayerMovement _entity)
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState(PlayerMovement _entity)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState(PlayerMovement _entity)
        {
            throw new System.NotImplementedException();
        }
    }

    public class DraggingState : State<PlayerMovement>
    {
        public override void EnterState(PlayerMovement _entity)
        {
            _entity.animator.SetBool("isClimbing", false);
            _entity.animator.SetBool("isHolding", true);
        }

        public override void ExitState(PlayerMovement _entity)
        {

        }

        public override void UpdateState(PlayerMovement _entity)
        {
            _entity.GroundCheck();
            _entity.Gravity();
            _entity.MoveHolding();
        }
    }

    public class ClimbingState : State<PlayerMovement>
    {
        public override void EnterState(PlayerMovement _entity)
        {
            _entity.animator.SetBool("isHolding", false);
            _entity.animator.SetBool("isClimbing", true);
        }

        public override void ExitState(PlayerMovement _entity)
        {
            _entity.gameObject.GetComponent<PlayerInteraction>().IsInteracting = false;
            Debug.Log(_entity.gameObject.GetComponent<PlayerInteraction>().IsInteracting);
        }

        public override void UpdateState(PlayerMovement _entity)
        {
            _entity.MoveVertical();
        }
    }

    public class CinematicState : State<PlayerMovement>
    {
        public override void EnterState(PlayerMovement _entity)
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState(PlayerMovement _entity)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState(PlayerMovement _entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
