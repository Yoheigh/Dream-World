using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerOwnedStates
{
    public class DefaultState : State<PlayerController>
    {
        public override void EnterState(PlayerController _entity)
        {
            // _entity.Input.CanMove(true);
            // _entity.Input.CanInteract(true);
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
            // _entity.Input.CanMove(false);
            //_entity.Input.CanInteract(false);
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
            _entity.animator.SetBool("isHolding", true);

            // ÀÌ°Å Áö±Ý isInteracting ÀÌ¶û ¿«¿©¼­ ÀÎÅÍ·¢¼Ç ¸·Èû
            // _entity.Input.CanInteract(false);
        }

        public override void UpdateState(PlayerController _entity)
        {;
            _entity.movement.Gravity();
            _entity.movement.MoveHolding(_entity.interaction.InteractionObj);
            
            
        }

        public override void ExitState(PlayerController _entity)
        {
            //_entity.gameObject.GetComponent<PlayerInteraction>().IsInteracting = false;
            //_entity.gameObject.GetComponent<PlayerInteraction>().dragableObj = null;
            //_entity.gameObject.GetComponent<PlayerInteraction>().obj = null;

            // ¿Í ¾¾ ÀÌ°Ô ¹¹ÀÓ

            _entity.animator.SetBool("isHolding", false);
        }

    }

    public class ClimbingState : State<PlayerController>
    {
        public override void EnterState(PlayerController _entity)
        {
            _entity.animator.SetBool("isClimbing", true);

            _entity.Input.CanInteract(false);
        }

        public override void UpdateState(PlayerController _entity)
        {
            _entity.movement.MoveVertical(_entity.interaction.InteractionObj);
            // _entity.SnapToVerticalPoint();
        }
        public override void ExitState(PlayerController _entity)
        {
            //_entity.gameObject.GetComponent<PlayerInteraction>().IsInteracting = false;
            //_entity.gameObject.GetComponent<PlayerInteraction>().dragableObj = null;
            //_entity.gameObject.GetComponent<PlayerInteraction>().obj = null;

            _entity.animator.SetBool("isClimbing", false);
            _entity.interaction.InteractionObj = null;
        }

    }

    public class InteractionState : State<PlayerController>
    {
        public override void EnterState(PlayerController _entity)
        {
            //_entity.Input.CanMove(false);
            _entity.Input.CanInteract(false);
            //_entity.fov.FindTargetsWithDelay(false);
        }

        public override void UpdateState(PlayerController _entity)
        {
            _entity.movement.Gravity();
            _entity.movement.GravityToFallCheck();
        }
        public override void ExitState(PlayerController _entity)
        {
            _entity.Input.CanInteract(true);
        }
    }

    public class DamagedState : State<PlayerController>
    {
        public override void EnterState(PlayerController _entity)
        {
            //_entity.Input.CanMove(false);
            //_entity.Input.CanInteract(false);
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
            //_entity.Input.CanMove(false);
            //_entity.Input.CanLook(false);
            //_entity.Input.CanInteract(false);
        }

        public override void UpdateState(PlayerController _entity)
        {

        }
        public override void ExitState(PlayerController _entity)
        {

        }

    }
}
