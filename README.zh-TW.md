<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Entity 實體物件組件

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.entity)](https://github.com/GameFrameX/com.gameframex.unity.entity/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.entity)](https://github.com/GameFrameX/com.gameframex.unity.entity/releases)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使

<br />

[文檔](https://gameframex.doc.alianblank.com) · [快速開始](#快速開始) · [QQ群](https://qm.qq.com/q/5kbDVBdUeS)

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>
## 📑 目錄

- [項目簡介](#項目簡介)
- [核心特性](#核心特性)
- [快速開始](#快速開始)
  - [安裝](#安裝)
- [使用範例](#使用範例)
  - [1. 定義 EntityLogic](#1-定義-entitylogic)
  - [2. 顯示實體](#2-顯示實體)
  - [3. 隱藏實體](#3-隱藏實體)
  - [4. 父子層級](#4-父子層級)
  - [5. 訂閱事件](#5-訂閱事件)
- [實體生命週期](#實體生命週期)
- [架構](#架構)
- [文檔與資源](#文檔與資源)
- [社區與支援](#社區與支援)
- [更新日誌](#更新日誌)
- [開源協議](#開源協議)

## 🏗️ 項目簡介

**Entity 實體物件組件** 提供了完整的 Unity 遊戲實體管理系統。它處理實體生命週期（顯示、隱藏、回收）、分組管理、父子層級關係，並整合資源系統實現異步資源載入，整合物件池實現實例復用。

基於 GameFrameX 框架構建，使用 UniTask 實現異步操作，採用 Manager-Component-Helper 分層架構。

## ✨ 核心特性

- **🎯 異步實體生成** — 基於 UniTask 異步載入並實例化實體，支援進度回調和依賴資源回調
- **📦 實體分組** — 將實體組織到分組中，每個分組可獨立設定物件池參數（容量、過期時間、自動釋放間隔）
- **🔗 父子層級** — 附加/解除子實體，自動處理 Transform 父子關係
- **♻️ 物件池整合** — 透過物件池復用實體實例，提升記憶體效率
- **📢 事件驅動生命週期** — 訂閱顯示/隱藏/更新/依賴資源等事件，實現響應式工作流
- **🧩 EntityLogic 模式** — 透過繼承 `EntityLogic` 實現業務邏輯，提供 OnInit/OnShow/OnHide/OnUpdate 等生命週期方法
- **🔒 實例管理** — 鎖定實體實例或調整優先級以控制物件池回收策略

## 🚀 快速開始

### 安裝

編輯 Unity 專案的 `Packages/manifest.json`，添加 `scopedRegistries` 部分：

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

`scopes` 控制哪些套件透過此註冊表解析。只有以 `com.gameframex` 開頭的套件才會從這個註冊表取得。

## 📖 使用範例

### 1. 定義 EntityLogic

建立 `EntityLogic` 子類別來實現實體行為：

```csharp
using GameFrameX.Entity.Runtime;
using UnityEngine;

public class PlayerEntity : EntityLogic
{
    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);
        // 初始化元件參考等
    }

    protected internal override void OnShow(object userData)
    {
        base.OnShow(userData);
        // 實體顯示時呼叫
    }

    protected internal override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);
        // 實體隱藏時呼叫
    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        // 每幀更新邏輯
    }
}
```

### 2. 顯示實體

使用 `EntityComponent` 異步生成實體：

```csharp
// 取得 EntityComponent
var entityComponent = GameEntry.GetComponent<EntityComponent>();

// 異步顯示實體
IEntity entity = await entityComponent.ShowEntityAsync<PlayerEntity>(
    entityId: 1,
    entityAssetName: "Assets/Prefabs/Player.prefab",
    entityGroupName: "PlayerGroup"
);
```

### 3. 隱藏實體

```csharp
// 按實體編號隱藏
entityComponent.HideEntity(1);

// 按實體參考隱藏
entityComponent.HideEntity(entity);

// 隱藏所有已載入實體
entityComponent.HideAllLoadedEntities();
```

### 4. 父子層級

附加子實體以建立父子關係：

```csharp
// 將子實體附加到父實體
entityComponent.AttachEntity(childEntity, parentEntity);

// 附加到指定 Transform 路徑
entityComponent.AttachEntity(childEntity, parentEntity, "Weapon/RightHand");

// 解除子實體
entityComponent.DetachEntity(childEntity);

// 解除父實體的所有子實體
entityComponent.DetachChildEntities(parentEntity);
```

### 5. 訂閱事件

```csharp
entityComponent.ShowEntitySuccess += (sender, e) =>
{
    Debug.Log($"實體顯示成功: {e.Entity.Id}");
};

entityComponent.ShowEntityFailure += (sender, e) =>
{
    Debug.LogError($"實體顯示失敗: {e.ErrorMessage}");
};

entityComponent.HideEntityComplete += (sender, e) =>
{
    Debug.Log($"實體隱藏完成: {e.EntityId}");
};
```

## 🔄 實體生命週期

| 階段 | 方法 | 說明 |
|------|------|------|
| 初始化 | `OnInit(object userData)` | 首次初始化，快取參考 |
| 顯示 | `OnShow(object userData)` | 實體變為可見且活躍 |
| 更新 | `OnUpdate(float, float)` | 顯示狀態下每幀呼叫 |
| 隱藏 | `OnHide(bool isShutdown, object userData)` | 實體被隱藏 |
| 回收 | `OnRecycle()` | 實體歸還到物件池 |

父子事件：

| 事件 | 父實體 | 子實體 |
|------|--------|--------|
| 附加 | `OnAttached(childEntity, parentTransform, userData)` | `OnAttachTo(parentEntity, parentTransform, userData)` |
| 解除 | `OnDetached(childEntity, userData)` | `OnDetachFrom(parentEntity, userData)` |

## 🏛️ 架構

```
Runtime/
├── Entity/
│   ├── EntityComponent.cs       # Unity MonoBehaviour 橋接層
│   ├── EntityLogic.cs           # 實體行為基類
│   ├── EntityHelperBase.cs      # 實例化/釋放抽象輔助器
│   ├── EntityGroupHelperBase.cs # 實體分組抽象輔助器
│   ├── Entity/
│   │   ├── IEntityManager.cs    # 核心管理器介面
│   │   ├── EntityManager.cs     # 管理器實作（partial class）
│   │   ├── IEntity.cs           # 實體介面
│   │   ├── IEntityGroup.cs      # 實體分組介面
│   │   └── ...
│   ├── ShowEntityInfo.cs        # 顯示實體參數
│   └── AttachEntityInfo.cs      # 附加實體參數
├── EventArgs/                   # 生命週期事件參數
│   ├── ShowEntitySuccessEventArgs.cs
│   ├── ShowEntityFailureEventArgs.cs
│   ├── ShowEntityUpdateEventArgs.cs
│   ├── ShowEntityDependencyAssetEventArgs.cs
│   └── HideEntityCompleteEventArgs.cs
└── Editor/Inspector/            # 自訂檢視器
```

## 📚 文檔與資源

- [文檔](https://gameframex.doc.alianblank.com)

## 💬 社區與支援

- [QQ群](https://qm.qq.com/q/5kbDVBdUeS)

## 📋 更新日誌

查看 [Releases](https://github.com/gameframex/com.gameframex.unity.entity/releases) 了解更新日誌。

## 📄 開源協議

本專案採用 [LICENSE](LICENSE.md) 中聲明的開源協議。
