using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterOwnedStates
{
    public class Idle : State<MonsterV2>
    {
        public override void EnterState(MonsterV2 _entity)
        {

        }

        public override void ExitState(MonsterV2 _entity)
        {

        }

        public override void UpdateState(MonsterV2 _entity)
        {
            _entity.PlayerDetected();

            _entity.CheckStopEnough();
        }
    }

    public class Patrol : State<MonsterV2>
    {
        public override void EnterState(MonsterV2 _entity)
        {

        }

        public override void ExitState(MonsterV2 _entity)
        {

        }

        public override void UpdateState(MonsterV2 _entity)
        {
            _entity.PlayerDetected();

            _entity.Patrol();
        }
    }

    public class Detect : State<MonsterV2>
    {
        public override void EnterState(MonsterV2 _entity)
        {
            _entity.OnDetect();
        }

        public override void ExitState(MonsterV2 _entity)
        {

        }

        public override void UpdateState(MonsterV2 _entity)
        {
            _entity.FollowTarget();
        }
    }

    public class Hit : State<MonsterV2>
    {
        public override void EnterState(MonsterV2 _entity)
        {

        }

        public override void ExitState(MonsterV2 _entity)
        {

        }

        public override void UpdateState(MonsterV2 _entity)
        {

        }
    }
}
