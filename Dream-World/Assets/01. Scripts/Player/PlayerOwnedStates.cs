using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerOwnedStates
{
    public class DefaultState : State<PlayerController>
    {
        public override void EnterState(PlayerController _entity)
        {
            _entity.input.CanMove(true);
            _entity.fov.FindTargetsWithDelay(true);

            //_entity.animator.SetBool("isClimbing", false);
            //_entity.animator.SetBool("isHolding", false);

            //_entity.InitTest();
        }

        public override void UpdateState(PlayerController _entity)
        {
            _entity.movement.Gravity();
            _entity.movement.GravityToFallCheck();
            _entity.movement.Move();

            _entity.interaction.NearObjectCheck();
        }

        public override void ExitState(PlayerController _entity)
        {
            _entity.fov.FindTargetsWithDelay(false);
            _entity.fov.ClearTargets();
        }
    }

    public class FallingState : State<PlayerController>
    {
        public override void EnterState(PlayerController _entity)
        {
            _entity.input.CanMove(false);
        }

        public override void UpdateState(PlayerController _entity)
        {
            _entity.movement.Gravity();
            _entity.movement.Move();

            if (_entity.movement.isGround == true)
            {
                if (_entity.movement.IsNextStateDefault() == false)
                {
                    _entity.movement.FallDamageCheck();
                    return;
                }
                _entity.ChangeState(PlayerStateType.Default);
            }
        }

        public override void ExitState(PlayerController _entity)
        {

        }

    }

    public class DraggingState : State<PlayerController>
    {
        public override void EnterState(PlayerController _entity)
        {
            //_entity.animator.SetBool("isClimbing", false);
            //_entity.animator.SetBool("isHolding", true);
        }

        public override void UpdateState(PlayerController _entity)
        {;
            _entity.movement.Gravity();
            _entity.movement.MoveHolding(_entity.interaction.InteractionObj);
            //_entity.movement.MoveHolding(_entity.gameObject.GetComponent<PlayerInteraction>().dragableObj);
        }

        public override void ExitState(PlayerController _entity)
        {
            //_entity.gameObject.GetComponent<PlayerInteraction>().IsInteracting = false;
            //_entity.gameObject.GetComponent<PlayerInteraction>().dragableObj = null;
            //_entity.gameObject.GetComponent<PlayerInteraction>().obj = null;

            // �� �� �̰� ����
        }

    }

    public class ClimbingState : State<PlayerController>
    {
        public override void EnterState(PlayerController _entity)
        {
            //_entity.animator.SetBool("isHolding", false);
            //_entity.animator.SetBool("isClimbing", true);
        }

        public override void UpdateState(PlayerController _entity)
        {
            //_entity.MoveVertical();
            // _entity.SnapToVerticalPoint();
        }
        public override void ExitState(PlayerController _entity)
        {
            //_entity.gameObject.GetComponent<PlayerInteraction>().IsInteracting = false;
            //_entity.gameObject.GetComponent<PlayerInteraction>().dragableObj = null;
            //_entity.gameObject.GetComponent<PlayerInteraction>().obj = null;
        }

    }

    public class InteractionState : State<PlayerController>
    {
        public override void EnterState(PlayerController _entity)
        {
            _entity.input.CanMove(false);
            _entity.fov.FindTargetsWithDelay(false);
        }

        public override void UpdateState(PlayerController _entity)
        {
            _entity.movement.Gravity();
            _entity.movement.GravityToFallCheck();
        }
        public override void ExitState(PlayerController _entity)
        {

        }
    }

    public class DamagedState : State<PlayerController>
    {
        public override void EnterState(PlayerController _entity)
        {
            
        }

        public override void UpdateState(PlayerController _entity)
        {
            _entity.movement.Gravity();
        }
        public override void ExitState(PlayerController _entity)
        {

        }

    }

    public class CinematicState : State<PlayerController>
    {
        public override void EnterState(PlayerController _entity)
        {

        }

        public override void UpdateState(PlayerController _entity)
        {

        }
        public override void ExitState(PlayerController _entity)
        {

        }

    }
}
