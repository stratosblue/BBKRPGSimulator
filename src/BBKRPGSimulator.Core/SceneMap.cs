using System.Drawing;
using System.IO;
using System.Linq;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Interface;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator
{
    /// <summary>
    /// 场景地图
    /// </summary>
    internal class SceneMap : ContextDependent, ICustomSerializeable
    {
        #region 字段

        /// <summary>
        /// 屏幕左上角对应地图的位置
        /// </summary>
        public Point MapScreenPos = Point.Empty;

        /// <summary>
        /// 屏幕左上角在地图中的位置
        /// </summary>
        public int MapScreenX = 1, MapScreenY = 1;

        /// <summary>
        /// 当前地图编号
        /// </summary>
        public int MapType = 1, MapIndex = 1;

        /// <summary>
        /// 当前脚本编号
        /// </summary>
        public int ScriptType = 1, ScriptIndex = 1;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 当前场景地图
        /// </summary>
        public ResMap Map { get; set; }

        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName { get; set; }

        /// <summary>
        /// 场景NPC
        /// id--NPC或场景对象 (1-40) 0为空
        /// </summary>
        public NPC[] SceneNPCs { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 场景地图
        /// </summary>
        /// <param name="context"></param>
        public SceneMap(SimulatorContext context) : base(context)
        { }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 检查指定点NPC当前是否可行走
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool CanNPCWalk(int x, int y)
        {
            return Map.CanWalk(x, y) &&
                   GetNpcFromPosInMap(x, y) == null &&
                   !Context.PlayContext.PlayerCharacter.PosInMap.EqualsLocation(x, y);
        }

        /// <summary>
        /// 检查指定点角色当前是否可行走
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool CanPlayerWalk(int x, int y)
        {
            if (Map == null)
            {
                return false;
            }
            return Map.CanPlayerWalk(x, y) && GetNpcFromPosInMap(x, y) == null;
        }

        /// <summary>
        /// 建一个宝箱，宝箱号码boxindex(角色图片，type为4)，
        /// 位置为（x，y），id为操作号（与NPC共用)
        /// </summary>
        public void CreateBox(int id, int boxIndex, int x, int y)
        {
            SceneObj newBox = Context.LibData.GetCharacter(4, boxIndex) as SceneObj;
            newBox.SetPosInMap(x, y);
            SceneNPCs[id] = newBox;
        }

        /// <summary>
        /// 创建NPC
        /// </summary>
        /// <param name="id"></param>
        /// <param name="npc"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CreateNpc(int id, int npc, int x, int y)
        {
            NPC newNPC = Context.LibData.GetCharacter(2, npc) as NPC;
            newNPC.SetPosInMap(x, y);
            SceneNPCs[id] = newNPC;
        }

        /// <summary>
        /// 移除所有NPC
        /// </summary>
        public void DeleteAllNpc()
        {
            //TODO 确认删除NPC与原版的差别
            for (int i = 0; i < SceneNPCs.Length; i++)
            {
                SceneNPCs[i] = null;
            }
        }

        /// <summary>
        /// 删除指定ID的箱子
        /// </summary>
        /// <param name="id"></param>
        public void DeleteBox(int id)
        {
            SceneNPCs[id] = null;
        }

        /// <summary>
        /// 删除指定id的NPC
        /// </summary>
        /// <param name="id"></param>
        public void DeleteNpc(int id)
        {
            SceneNPCs[id] = null;
        }

        /// <summary>
        /// 绘制场景
        /// </summary>
        /// <param name="canvas"></param>
        public void DrawScene(ICanvas canvas)
        {
            if (Map != null)
            {
                Map.DrawMap(canvas, MapScreenPos.X, MapScreenPos.Y);
            }

            int playY = 10000;
            bool hasPlayerBeenDrawn = false;
            var player = Context.PlayContext.PlayerCharacter;

            if (player != null)
            {
                playY = player.PosInMap.Y;
            }

            NPC[] npcs = GetSortedSceneNpcs();
            foreach (var npc in npcs)
            {
                if (!hasPlayerBeenDrawn && playY < npc.PosInMap.Y)
                {
                    player.DrawWalkingSprite(canvas, MapScreenPos);
                    hasPlayerBeenDrawn = true;
                }
                npc.DrawWalkingSprite(canvas, MapScreenPos);
            }

            if (player != null && !hasPlayerBeenDrawn)
            {
                player.DrawWalkingSprite(canvas, MapScreenPos);
            }
            Context.Util.DrawSideFrame(canvas);
        }

        /// <summary>
        /// 得到地图的(x,y)处的NPC，没有就返回null
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public NPC GetNpcFromPosInMap(int x, int y)
        {
            return SceneNPCs[GetNpcIdFromPosInMap(x, y)];
        }

        /// <summary>
        /// 通过xy获取地图对应位置的NPCID
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetNpcIdFromPosInMap(int x, int y)
        {
            for (int i = 1; i < SceneNPCs.Length; i++)
            {
                if (SceneNPCs[i] != null && SceneNPCs[i].PosInMap.EqualsLocation(x, y))
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取排序后的场景NPC列表
        /// 按y值从大到小排序，确保正确的遮挡关系
        /// </summary>
        /// <returns></returns>
        public NPC[] GetSortedSceneNpcs()
        {
            return SceneNPCs.Where(m => m != null).OrderBy(m => m.PosInMap.Y).ToArray();
        }

        /// <summary>
        /// 检查指定的NPC在屏幕内是否可见
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public bool IsNpcVisible(NPC npc)
        {
            Point point = npc.GetPosOnScreen(MapScreenPos);
            return point.X >= 0 &&
                   point.X < ResMap.WIDTH &&
                   point.Y >= 0 &&
                   point.Y <= ResMap.HEIGHT;
        }

        /// <summary>
        /// 检查指定id的NPC在屏幕内是否可见
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsNpcVisible(int id)
        {
            return IsNpcVisible(SceneNPCs[id]);
        }

        /// <summary>
        /// 载入地图
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <param name="index">资源编号</param>
        /// <param name="x">初始x坐标</param>
        /// <param name="y">初始y坐标</param>
        public void LoadMap(int type, int index, int x, int y)
        {
            Point point = Point.Empty;
            var playerCharacter = Context.PlayContext.PlayerCharacter;

            if (playerCharacter != null && Map != null)
            {
                point = playerCharacter.GetPosOnScreen(MapScreenPos);
            }

            Map = Context.LibData.GetMap(type, index);
            MapScreenPos.X = x;
            MapScreenPos.Y = y;

            //TODO 判断Player？
            if (point != Point.Empty && playerCharacter != null)
            {
                playerCharacter.SetPosOnScreen(point.X, point.Y, MapScreenPos);
            }

            MapType = type;
            MapIndex = index;
            MapScreenX = x;
            MapScreenY = y;
        }

        /// <summary>
        /// 重新载入地图
        /// </summary>
        public void ReLoadMap()
        {
            //HACK 载入时清空地图
            Map = null;

            LoadMap(MapType, MapIndex, MapScreenX, MapScreenY);
        }

        /// <summary>
        /// 设置地图的屏幕偏移
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetMapScreenPos(int x, int y)
        {
            MapScreenPos.X = x;
            MapScreenPos.Y = y;
        }

        /// <summary>
        /// 场景切换
        /// 如果地图(x, y)有地图事件，就触发该事件
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool TriggerMapEvent(int x, int y)
        {
            if (Map != null && Context.ScriptProcess.ScriptExecutor != null)
            {
                int id = Map.GetEventNum(x, y);
                if (id != 0)
                {
                    if (Context.ScriptProcess.ScriptExecutor.TriggerEvent(id + 40))
                    {
                        Context.ScriptProcess.ScriptRunning = true;
                        return true;
                    }
                }
            }
            // 未触发地图事件，随机战斗
            Context.CombatManage.StartNewRandomCombat();
            return false;
        }

        /// <summary>
        /// 按enter键后，检测并触发场景对象里的事件，如NPC对话，开宝箱等
        /// </summary>
        public void TriggerSceneObjEvent()
        {
            PlayerCharacter playerCharacter = Context.PlayContext.PlayerCharacter;
            int x = playerCharacter.PosInMap.X;
            int y = playerCharacter.PosInMap.Y;
            switch (playerCharacter.Direction)
            {
                case Direction.East: ++x; break;
                case Direction.North: --y; break;
                case Direction.South: ++y; break;
                case Direction.West: --x; break;
            }

            // NPC事件
            int npcId = GetNpcIdFromPosInMap(x, y);
            if (npcId != 0)
            {
                Context.ScriptProcess.ScriptRunning = Context.ScriptProcess.ScriptExecutor.TriggerEvent(npcId);
                return;
            }
            else if (TriggerMapEvent(x, y))
            {
                // 地图切换
            }
        }

        /// <summary>
        /// 更新场景信息
        /// </summary>
        /// <param name="delta"></param>
        public void Update(long delta)
        {
            for (int i = 1; i < SceneNPCs.Length; i++)
            {
                if (SceneNPCs[i] == null)
                {
                    continue;
                }
                SceneNPCs[i].Update(delta);
            }
        }

        public void Deserialize(BinaryReader binaryReader)
        {
            SceneName = binaryReader.ReadString();
            MapType = binaryReader.ReadInt32();
            MapIndex = binaryReader.ReadInt32();
            MapScreenX = binaryReader.ReadInt32();
            MapScreenY = binaryReader.ReadInt32();
            ScriptType = binaryReader.ReadInt32();
            ScriptIndex = binaryReader.ReadInt32();

            SceneNPCs = new NPC[41];

            int npcCount = binaryReader.ReadInt32();

            for (int i = 0; i < npcCount; i++)
            {
                int npcId = binaryReader.ReadInt32();
                SceneNPCs[npcId] = new NPC(Context);
                SceneNPCs[npcId].Deserialize(binaryReader);
            }
        }

        public void Serialize(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(SceneName);
            binaryWriter.Write(MapType);
            binaryWriter.Write(MapIndex);
            binaryWriter.Write(MapScreenX);
            binaryWriter.Write(MapScreenY);
            binaryWriter.Write(ScriptType);
            binaryWriter.Write(ScriptIndex);

            var npcCount = SceneNPCs.Where(m => m != null).Count();

            binaryWriter.Write(npcCount);

            for (int i = 0; i < SceneNPCs.Length; i++)
            {
                if (SceneNPCs[i] != null)
                {
                    binaryWriter.Write(i);
                    SceneNPCs[i].Serialize(binaryWriter);
                }
            }
        }

        #endregion 方法
    }
}