﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using GameFrameX.Event.Runtime;
using GameFrameX.Runtime;

namespace GameFrameX.Entity.Runtime
{
    /// <summary>
    /// 显示实体更新事件。
    /// </summary>
    public sealed class ShowEntityUpdateEventArgs : GameEventArgs
    {
        /// <summary>
        /// 显示实体更新事件编号。
        /// </summary>
        public static readonly string EventId = typeof(ShowEntityUpdateEventArgs).FullName;

        /// <summary>
        /// 初始化显示实体更新事件的新实例。
        /// </summary>
        public ShowEntityUpdateEventArgs()
        {
            EntityId = 0;
            EntityLogicType = null;
            EntityAssetName = null;
            EntityGroupName = null;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取显示实体更新事件编号。
        /// </summary>
        public override string Id
        {
            get { return EventId; }
        }

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int EntityId { get; private set; }

        /// <summary>
        /// 获取实体逻辑类型。
        /// </summary>
        public Type EntityLogicType { get; private set; }

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        public string EntityAssetName { get; private set; }

        /// <summary>
        /// 获取实体组名称。
        /// </summary>
        public string EntityGroupName { get; private set; }

        /// <summary>
        /// 获取显示实体进度。
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建显示实体更新事件。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <param name="progress">显示实体进度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的显示实体更新事件。</returns>
        public static ShowEntityUpdateEventArgs Create(int entityId, string entityAssetName, string entityGroupName, float progress, object userData)
        {
            ShowEntityUpdateEventArgs showEntityUpdateEventArgs = ReferencePool.Acquire<ShowEntityUpdateEventArgs>();
            showEntityUpdateEventArgs.EntityId = entityId;
            showEntityUpdateEventArgs.EntityAssetName = entityAssetName;
            showEntityUpdateEventArgs.EntityGroupName = entityGroupName;
            showEntityUpdateEventArgs.Progress = progress;
            showEntityUpdateEventArgs.UserData = userData;
            return showEntityUpdateEventArgs;
        }

        /// <summary>
        /// 清理显示实体更新事件。
        /// </summary>
        public override void Clear()
        {
            EntityId = 0;
            EntityLogicType = null;
            EntityAssetName = null;
            EntityGroupName = null;
            Progress = 0f;
            UserData = null;
        }
    }
}