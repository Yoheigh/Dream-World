using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerOwnedStates
{
    public class DefaultState : State<PlayerMovement>
    {
        public override void EnterState(PlayerMovement _entity)
        {
            _entity.animator.SetBool("isClimbing", false);
            _entity.animator.SetBool("isHolding", false);

            _entity.InitTest();
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
            
        }

        public override void UpdateState(PlayerMovement _entity)
        {

        }
        public override void ExitState(PlayerMovement _entity)
        {

        }

    }

    public class DraggingState : State<PlayerMovement>
    {
        public override void EnterState(PlayerMovement _entity)
        {
            _entity.animator.SetBool("isClimbing", false);
            _entity.animator.SetBool("isHolding", true);
        }

        public override void UpdateState(PlayerMovement _entity)
        {
            _entity.GroundCheck();
            _entity.Gravity();
            _entity.MoveHolding(_entity.gameObject.GetComponent<PlayerInteraction>().dragableObj);
        }

        public override void ExitState(PlayerMovement _entity)
        {
            _entity.gameObject.GetComponent<PlayerInteraction>().IsInteracting = false;
            _entity.gameObject.GetComponent<PlayerInteraction>().dragableObj = null;
            _entity.gameObject.GetComponent<PlayerInteraction>().obj = null;
        }

    }

    public class ClimbingState : State<PlayerMovement>
    {
        public override void EnterState(PlayerMovement _entity)
        {
            _entity.animator.SetBool("isHolding", false);
            _entity.animator.SetBool("isClimbing", true);
        }

        public override void UpdateState(PlayerMovement _entity)
        {
            _entity.MoveVertical();
            // _entity.SnapToVerticalPoint();
        }
        public override void ExitState(PlayerMovement _entity)
        {
            _entity.gameObject.GetComponent<PlayerInteraction>().IsInteracting = false;
            _entity.gameObject.GetComponent<PlayerInteraction>().dragableObj = null;
            _entity.gameObject.GetComponent<PlayerInteraction>().obj = null;
        }

    }

    public class InteractionState : State<PlayerMovement>
    {
        public override void EnterState(PlayerMovement _entity)
        {

        }

        public override void UpdateState(PlayerMovement _entity)
        {
            _entity.GroundCheck();
            _entity.Gravity();
        }
        public override void ExitState(PlayerMovement _entity)
        {

        }
    }

    public class CinematicState : State<PlayerMovement>
    {
        public override void EnterState(PlayerMovement _entity)
        {

        }

        public override void UpdateState(PlayerMovement _entity)
        {

        }
        public override void ExitState(PlayerMovement _entity)
        {

        }

    }
}
