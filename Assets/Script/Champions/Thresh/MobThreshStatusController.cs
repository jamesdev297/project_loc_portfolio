using UnityEngine;

namespace Script.Champions.Thresh
{
    public class MobThreshStatusController : MobStatusController
    {
        // Start is called before the first frame update
        private void Awake()
        {
            moveController = GetComponent<MobMoveController>();
        }
    
        protected override void stateInit()
        {
            mobAINoneState = new MobThreshAINoneState(this);
            mobAIChaseState = new MobThreshAIChaseState(this);
            mobAIRunAwayState = new MobThreshAIRunAwayState(this);
            mobAIDistance = new MobThreshAIDistanceState(this);
        }
    }
}