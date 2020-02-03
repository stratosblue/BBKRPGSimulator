using System.Collections.Generic;
using System.Linq;

using BBKRPGSimulator.Characters;

namespace BBKRPGSimulator
{
    /// <summary>
    /// 游戏上下文信息
    /// </summary>
    internal class PlayContext : ContextDependent
    {
        #region 构造函数

        /// <summary>
        /// 游戏上下文信息
        /// </summary>
        /// <param name="context"></param>
        public PlayContext(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 属性

        /// <summary>
        /// 战斗概率
        /// 默认值20，概率为1/18
        /// </summary>
        public int CombatProbability { get; set; } = 20;

        /// <summary>
        /// 玩家金钱
        /// </summary>
        public int Money { get; set; } = 0;

        /// <summary>
        /// 玩家角色
        /// </summary>
        public PlayerCharacter PlayerCharacter { get => PlayerCharacters.FirstOrDefault(); }

        /// <summary>
        /// 玩家角色列表
        /// </summary>
        public List<PlayerCharacter> PlayerCharacters { get; set; } = new List<PlayerCharacter>();

        #endregion 属性

        #region 方法

        /// <summary>
        /// 创建玩家角色
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CreateActor(int actorId, int x, int y)
        {
            var newActor = Context.LibData.GetCharacter(1, actorId) as PlayerCharacter;
            newActor.SetPosOnScreen(x, y, Context.SceneMap.MapScreenPos);
            PlayerCharacters.Add(newActor);
        }

        /// <summary>
        /// 删除玩家角色
        /// </summary>
        /// <param name="actorId"></param>
        public void DeleteActor(int actorId)
        {
            for (int i = 0; i < PlayerCharacters.Count; i++)
            {
                if (PlayerCharacters[i].Index == actorId)
                {
                    PlayerCharacters.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 获取指定actorId的角色
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        public PlayerCharacter GetPlayer(int actorId)
        {
            return PlayerCharacters.Where(m => m.Index == actorId).FirstOrDefault();
        }

        #endregion 方法
    }
}