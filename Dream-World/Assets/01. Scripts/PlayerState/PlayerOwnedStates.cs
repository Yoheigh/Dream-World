using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerOwnedStates
{
    public class DefaultState : State<PlayerMovement>
    {
        public override void EnterState(PlayerMovement _entity)
        {

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

    public class ClimbingState : State<PlayerMovement>
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
