//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月21日 17:07:02
//------------------------------------------------------------

namespace ET
{
    public enum RoleCast
    {
        /// <summary>
        /// 友善的
        /// </summary>
        Friendly,

        /// <summary>
        /// 敌对的
        /// </summary>
        Adverse,

        /// <summary>
        /// 中立的
        /// </summary>
        Neutral
    }

    [System.Flags]
    public enum RoleCamp
    {
        Player = 0b0000001,
        Monster = 0b0000010,
    }

    public enum RoleTag
    {
        Sprite,
        AttackRange,
        NoCollision,
        Hero,
        Map,
        Creeps,
        SkillCollision,
    }

    public class B2S_RoleCastComponentAwakeSystem : AwakeSystem<B2S_RoleCastComponent, RoleCamp, RoleTag>
    {
        public override void Awake(B2S_RoleCastComponent self, RoleCamp a, RoleTag b)
        {
            self.RoleCamp = a;
            self.RoleTag = b;
        }
    }

    public class B2S_RoleCastComponent : Entity, IAwake<RoleCamp,RoleTag>
    {
        public RoleTag RoleTag;

        /// <summary>
        /// 归属阵营
        /// </summary>
        public RoleCamp RoleCamp;

        /// <summary>
        /// 获取与目标的关系
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public RoleCast GetRoleCastToTarget(Unit unit)
        {
            if (unit.GetComponent<B2S_RoleCastComponent>() == null)
            {
                return RoleCast.Friendly;
            }

            RoleCamp roleCamp = unit.GetComponent<B2S_RoleCastComponent>().RoleCamp;

            if (roleCamp == this.RoleCamp)
            {
                return RoleCast.Friendly;
            }
            
            if (roleCamp != this.RoleCamp)
            {
                if (roleCamp == RoleCamp.Player)
                {
                    return RoleCast.Neutral;
                }
                else
                {
                    return RoleCast.Adverse;
                }
            }

            return RoleCast.Friendly;
        }
    }
}