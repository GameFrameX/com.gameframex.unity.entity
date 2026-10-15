// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents and other related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws and regulations of the People's Republic of China and other international regulations.
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the full license text of the source code root directory.
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer and the project organization and contributors assume no responsibility.
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB 仓库：https://cnb.cool/GameFrameX
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
//  ==========================================================================================

using System;
using GameFrameX.Runtime;
using UnityEngine;

namespace GameFrameX.Entity.Runtime
{
    /// <summary>
    /// 实体模块启动配置（模块级 Options）。
    /// </summary>
    /// <remarks>
    /// 默认值即开箱值（与原 EntityComponent 序列化字段默认值同源）；游戏在首场景加载前（BeforeSceneLoad）经 <c>GameFrameXOptions.Configure(new EntityOptions{...})</c> 注入差异值，
    /// 由 <see cref="EntityComponent"/> 在 Awake 中一次性消费。
    /// </remarks>
    public sealed class EntityOptions
    {
        /// <summary>
        /// 实体组默认对象池容量（默认 512）。
        /// </summary>
        /// <remarks>启动期型：仅在组件 Awake 消费；运行期经 <see cref="EntityComponent.DefaultCapacity"/> 修改。</remarks>
        [GameFrameXStartupOnly]
        public int DefaultCapacity = 512;

        /// <summary>
        /// 实体组默认对象过期时长，以秒为单位（默认 3600 秒）。
        /// </summary>
        /// <remarks>启动期型：仅在组件 Awake 消费；运行期经 <see cref="EntityComponent.DefaultExpireTime"/> 修改。</remarks>
        [GameFrameXStartupOnly]
        public float DefaultExpireTime = 3600f;

        /// <summary>
        /// 是否开启显示实体更新事件（默认关闭）。
        /// </summary>
        /// <remarks>实时型：运行期可修改，立即生效。</remarks>
        public bool EnableShowEntityUpdateEvent = false;

        /// <summary>
        /// 是否开启显示实体时加载依赖资源事件（默认关闭）。
        /// </summary>
        /// <remarks>实时型：运行期可修改，立即生效。</remarks>
        public bool EnableShowEntityDependencyAssetEvent = false;

        /// <summary>
        /// 实体辅助器工厂（默认 DefaultEntityHelper）。
        /// </summary>
        /// <remarks>启动期型：仅在组件创建 Helper 时消费，运行期不可修改。</remarks>
        [GameFrameXStartupOnly]
        public Func<EntityHelperBase> EntityHelperFactory = () => new GameObject().AddComponent<DefaultEntityHelper>();

        /// <summary>
        /// 实体组辅助器工厂（默认 DefaultEntityGroupHelper）。
        /// </summary>
        /// <remarks>启动期型：仅在组件创建 Helper 时消费，运行期不可修改。</remarks>
        [GameFrameXStartupOnly]
        public Func<EntityGroupHelperBase> EntityGroupHelperFactory = () => new GameObject().AddComponent<DefaultEntityGroupHelper>();
    }
}
