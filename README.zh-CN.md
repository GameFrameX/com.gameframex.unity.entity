<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Entity 实体对象组件

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.entity)](https://github.com/GameFrameX/com.gameframex.unity.entity/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.entity)](https://github.com/GameFrameX/com.gameframex.unity.entity/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

独立游戏前后端一体化解决方案 · 独立游戏开发者的圆梦大使

<br />

[文档](https://gameframex.doc.alianblank.com) · [快速开始](#快速开始) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 📑 目录

- [项目简介](#项目简介)
- [核心特性](#核心特性)
- [快速开始](#快速开始)
  - [安装](#安装)
- [使用示例](#使用示例)
  - [1. 定义 EntityLogic](#1-定义-entitylogic)
  - [2. 显示实体](#2-显示实体)
  - [3. 隐藏实体](#3-隐藏实体)
  - [4. 父子层级](#4-父子层级)
  - [5. 订阅事件](#5-订阅事件)
- [实体生命周期](#实体生命周期)
- [架构](#架构)
- [文档与资源](#文档与资源)
- [社区与支持](#社区与支持)
- [更新日志](#更新日志)
- [开源协议](#开源协议)

## 🏗️ 项目简介

**Entity 实体对象组件** 提供了完整的 Unity 游戏实体管理系统。它处理实体生命周期（显示、隐藏、回收）、分组管理、父子层级关系，并集成资源系统实现异步资源加载，集成对象池实现实例复用。

基于 GameFrameX 框架构建，使用 UniTask 实现异步操作，采用 Manager-Component-Helper 分层架构。

## ✨ 核心特性

- **🎯 异步实体生成** — 基于 UniTask 异步加载并实例化实体，支持进度回调和依赖资源回调
- **📦 实体分组** — 将实体组织到分组中，每个分组可独立配置对象池参数（容量、过期时间、自动释放间隔）
- **🔗 父子层级** — 附加/解除子实体，自动处理 Transform 父子关系
- **♻️ 对象池集成** — 通过对象池复用实体实例，提升内存效率
- **📢 事件驱动生命周期** — 订阅显示/隐藏/更新/依赖资源等事件，实现响应式工作流
- **🧩 EntityLogic 模式** — 通过继承 `EntityLogic` 实现业务逻辑，提供 OnInit/OnShow/OnHide/OnUpdate 等生命周期方法
- **🔒 实例管理** — 锁定实体实例或调整优先级以控制对象池回收策略

## 🚀 快速开始

### 安装

编辑 Unity 项目的 `Packages/manifest.json`，添加 `scopedRegistries` 部分：

```json
{
  "scopedRegistries": [
    {
      "name": "GameFrameX",
      "url": "https://gameframex.upm.alianblank.uk",
      "scopes": [
        "com.gameframex"
      ]
    }
  ],
  "dependencies": {
    "com.gameframex.unity.entity": "2.4.2"
  }
}
```

`scopes` 控制哪些包通过此注册表解析。只有以 `com.gameframex` 开头的包才会从这个注册表获取。

## 📖 使用示例

### 1. 定义 EntityLogic

创建 `EntityLogic` 子类来实现实体行为：

```csharp
using GameFrameX.Entity.Runtime;
using UnityEngine;

public class PlayerEntity : EntityLogic
{
    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);
        // 初始化组件引用等
    }

    protected internal override void OnShow(object userData)
    {
        base.OnShow(userData);
        // 实体显示时调用
    }

    protected internal override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);
        // 实体隐藏时调用
    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        // 每帧更新逻辑
    }
}
```

### 2. 显示实体

使用 `EntityComponent` 异步生成实体：

```csharp
// 获取 EntityComponent
var entityComponent = GameEntry.GetComponent<EntityComponent>();

// 异步显示实体
IEntity entity = await entityComponent.ShowEntityAsync<PlayerEntity>(
    entityId: 1,
    entityAssetName: "Assets/Prefabs/Player.prefab",
    entityGroupName: "PlayerGroup"
);
```

### 3. 隐藏实体

```csharp
// 按实体编号隐藏
entityComponent.HideEntity(1);

// 按实体引用隐藏
entityComponent.HideEntity(entity);

// 隐藏所有已加载实体
entityComponent.HideAllLoadedEntities();
```

### 4. 父子层级

附加子实体以建立父子关系：

```csharp
// 将子实体附加到父实体
entityComponent.AttachEntity(childEntity, parentEntity);

// 附加到指定 Transform 路径
entityComponent.AttachEntity(childEntity, parentEntity, "Weapon/RightHand");

// 解除子实体
entityComponent.DetachEntity(childEntity);

// 解除父实体的所有子实体
entityComponent.DetachChildEntities(parentEntity);
```

### 5. 订阅事件

```csharp
entityComponent.ShowEntitySuccess += (sender, e) =>
{
    Debug.Log($"实体显示成功: {e.Entity.Id}");
};

entityComponent.ShowEntityFailure += (sender, e) =>
{
    Debug.LogError($"实体显示失败: {e.ErrorMessage}");
};

entityComponent.HideEntityComplete += (sender, e) =>
{
    Debug.Log($"实体隐藏完成: {e.EntityId}");
};
```

## 🔄 实体生命周期

| 阶段 | 方法 | 说明 |
|------|------|------|
| 初始化 | `OnInit(object userData)` | 首次初始化，缓存引用 |
| 显示 | `OnShow(object userData)` | 实体变为可见且活跃 |
| 更新 | `OnUpdate(float, float)` | 显示状态下每帧调用 |
| 隐藏 | `OnHide(bool isShutdown, object userData)` | 实体被隐藏 |
| 回收 | `OnRecycle()` | 实体归还到对象池 |

父子事件：

| 事件 | 父实体 | 子实体 |
|------|--------|--------|
| 附加 | `OnAttached(childEntity, parentTransform, userData)` | `OnAttachTo(parentEntity, parentTransform, userData)` |
| 解除 | `OnDetached(childEntity, userData)` | `OnDetachFrom(parentEntity, userData)` |

## 🏛️ 架构

```
Runtime/
├── Entity/
│   ├── EntityComponent.cs       # Unity MonoBehaviour 桥接层
│   ├── EntityLogic.cs           # 实体行为基类
│   ├── EntityHelperBase.cs      # 实例化/释放抽象辅助器
│   ├── EntityGroupHelperBase.cs # 实体组抽象辅助器
│   ├── Entity/
│   │   ├── IEntityManager.cs    # 核心管理器接口
│   │   ├── EntityManager.cs     # 管理器实现（partial class）
│   │   ├── IEntity.cs           # 实体接口
│   │   ├── IEntityGroup.cs      # 实体组接口
│   │   └── ...
│   ├── ShowEntityInfo.cs        # 显示实体参数
│   └── AttachEntityInfo.cs      # 附加实体参数
├── EventArgs/                   # 生命周期事件参数
│   ├── ShowEntitySuccessEventArgs.cs
│   ├── ShowEntityFailureEventArgs.cs
│   ├── ShowEntityUpdateEventArgs.cs
│   ├── ShowEntityDependencyAssetEventArgs.cs
│   └── HideEntityCompleteEventArgs.cs
└── Editor/Inspector/            # 自定义检视器
```

## 📚 文档与资源

- [文档](https://gameframex.doc.alianblank.com)

## 💬 社区与支持

- QQ群: 467608841 / 233840761

## 📋 更新日志

查看 [Releases](https://github.com/gameframex/com.gameframex.unity.entity/releases) 了解更新日志。

## 📄 开源协议

本项目采用 [LICENSE](LICENSE.md) 中声明的开源协议。
