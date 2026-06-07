<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Entity エンティティコンポーネント

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.entity)](https://github.com/GameFrameX/com.gameframex.unity.entity/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.entity)](https://github.com/GameFrameX/com.gameframex.unity.entity/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援

<br />

[ドキュメント](https://gameframex.doc.alianblank.com) · [クイックスタート](#クイックスタート) · QQグループ: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

</div>

## 目次

- [プロジェクト概要](#プロジェクト概要)
- [主な機能](#主な機能)
- [クイックスタート](#クイックスタート)
  - [インストール](#インストール)
- [使用例](#使用例)
  - [1. EntityLogic の定義](#1-entitylogic-の定義)
  - [2. エンティティの表示](#2-エンティティの表示)
  - [3. エンティティの非表示](#3-エンティティの非表示)
  - [4. 親子階層](#4-親子階層)
  - [5. イベントの購読](#5-イベントの購読)
- [エンティティライフサイクル](#エンティティライフサイクル)
- [アーキテクチャ](#アーキテクチャ)
- [ドキュメントとリソース](#ドキュメントとリソース)
- [コミュニティとサポート](#コミュニティとサポート)
- [変更履歴](#変更履歴)
- [ライセンス](#ライセンス)

## プロジェクト概要

**Entity エンティティコンポーネント** は、Unity ゲーム向けの完全なエンティティ管理システムを提供します。エンティティのライフサイクル（表示、非表示、リサイクル）、グループ化、親子階層を管理し、アセットシステムと統合して非同期リソース読み込みを実現し、オブジェクトプールと統合してインスタンスの再利用を実現します。

GameFrameX フレームワーク上に構築され、UniTask を使用した非同期操作と Manager-Component-Helper のレイヤードアーキテクチャを採用しています。

## 主な機能

- **非同期エンティティ生成** — UniTask ベースの非同期読み込みとインスタンス化、進捗コールバックと依存リソースコールバックをサポート
- **エンティティグループ化** — エンティティをグループに編成し、各グループで個別にオブジェクトプール設定（容量、有効期限、自動解放間隔）が可能
- **親子階層** — 子エンティティのアタッチ/デタッチ、Transform の親子関係を自動処理
- **オブジェクトプール統合** — オブジェクトプールによるエンティティインスタンスの再利用でメモリ効率を向上
- **イベント駆動ライフサイクル** — 表示/非表示/更新/依存リソース等のイベントを購読し、リアクティブなワークフローを実現
- **EntityLogic パターン** — `EntityLogic` のサブクラス化でビジネスロジックを実装、OnInit/OnShow/OnHide/OnUpdate 等のライフサイクルメソッドを提供
- **インスタンス管理** — エンティティインスタンスのロックや優先度調整でプールの退避制御が可能

## クイックスタート

### インストール

Unity プロジェクトの `Packages/manifest.json` を編集し、`scopedRegistries` セクションを追加してください：

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

`scopes` は、どのパッケージをこのレジストリから解決するかを制御します。`com.gameframex` で始まるパッケージのみがこのレジストリから取得されます。

## 使用例

### 1. EntityLogic の定義

`EntityLogic` のサブクラスを作成してエンティティの動作を実装します：

```csharp
using GameFrameX.Entity.Runtime;
using UnityEngine;

public class PlayerEntity : EntityLogic
{
    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);
        // コンポーネント参照の初期化など
    }

    protected internal override void OnShow(object userData)
    {
        base.OnShow(userData);
        // エンティティ表示時に呼び出される
    }

    protected internal override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);
        // エンティティ非表示時に呼び出される
    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        // 毎フレームの更新ロジック
    }
}
```

### 2. エンティティの表示

`EntityComponent` を使用してエンティティを非同期生成します：

```csharp
// EntityComponent を取得
var entityComponent = GameEntry.GetComponent<EntityComponent>();

// エンティティを非同期表示
IEntity entity = await entityComponent.ShowEntityAsync<PlayerEntity>(
    entityId: 1,
    entityAssetName: "Assets/Prefabs/Player.prefab",
    entityGroupName: "PlayerGroup"
);
```

### 3. エンティティの非表示

```csharp
// エンティティ ID で非表示
entityComponent.HideEntity(1);

// エンティティ参照で非表示
entityComponent.HideEntity(entity);

// 読み込み済みのすべてのエンティティを非表示
entityComponent.HideAllLoadedEntities();
```

### 4. 親子階層

子エンティティをアタッチして親子関係を構築します：

```csharp
// 子エンティティを親エンティティにアタッチ
entityComponent.AttachEntity(childEntity, parentEntity);

// 特定の Transform パスにアタッチ
entityComponent.AttachEntity(childEntity, parentEntity, "Weapon/RightHand");

// 子エンティティをデタッチ
entityComponent.DetachEntity(childEntity);

// 親エンティティのすべての子をデタッチ
entityComponent.DetachChildEntities(parentEntity);
```

### 5. イベントの購読

```csharp
entityComponent.ShowEntitySuccess += (sender, e) =>
{
    Debug.Log($"エンティティ表示成功: {e.Entity.Id}");
};

entityComponent.ShowEntityFailure += (sender, e) =>
{
    Debug.LogError($"エンティティ表示失敗: {e.ErrorMessage}");
};

entityComponent.HideEntityComplete += (sender, e) =>
{
    Debug.Log($"エンティティ非表示完了: {e.EntityId}");
};
```

## エンティティライフサイクル

| 段階 | メソッド | 説明 |
|------|----------|------|
| 初期化 | `OnInit(object userData)` | 初回初期化、参照のキャッシュ |
| 表示 | `OnShow(object userData)` | エンティティが可視化されアクティブに |
| 更新 | `OnUpdate(float, float)` | 表示中の毎フレーム呼び出し |
| 非表示 | `OnHide(bool isShutdown, object userData)` | エンティティが非表示に |
| リサイクル | `OnRecycle()` | エンティティがオブジェクトプールに返却 |

親子イベント：

| イベント | 親エンティティ | 子エンティティ |
|----------|----------------|----------------|
| アタッチ | `OnAttached(childEntity, parentTransform, userData)` | `OnAttachTo(parentEntity, parentTransform, userData)` |
| デタッチ | `OnDetached(childEntity, userData)` | `OnDetachFrom(parentEntity, userData)` |

## アーキテクチャ

```
Runtime/
├── Entity/
│   ├── EntityComponent.cs       # Unity MonoBehaviour ブリッジ
│   ├── EntityLogic.cs           # エンティティ動作の基底クラス
│   ├── EntityHelperBase.cs      # インスタンス化/解放の抽象ヘルパー
│   ├── EntityGroupHelperBase.cs # エンティティグループの抽象ヘルパー
│   ├── Entity/
│   │   ├── IEntityManager.cs    # コアマネージャーインターフェース
│   │   ├── EntityManager.cs     # マネージャー実装（partial class）
│   │   ├── IEntity.cs           # エンティティインターフェース
│   │   ├── IEntityGroup.cs      # エンティティグループインターフェース
│   │   └── ...
│   ├── ShowEntityInfo.cs        # エンティティ表示パラメータ
│   └── AttachEntityInfo.cs      # エンティティアタッチパラメータ
├── EventArgs/                   # ライフサイクルイベント引数
│   ├── ShowEntitySuccessEventArgs.cs
│   ├── ShowEntityFailureEventArgs.cs
│   ├── ShowEntityUpdateEventArgs.cs
│   ├── ShowEntityDependencyAssetEventArgs.cs
│   └── HideEntityCompleteEventArgs.cs
└── Editor/Inspector/            # カスタムインスペクター
```

## ドキュメントとリソース

- [ドキュメント](https://gameframex.doc.alianblank.com)

## コミュニティとサポート

- QQグループ: 467608841 / 233840761

## 変更履歴

変更履歴は [Releases](https://github.com/gameframex/com.gameframex.unity.entity/releases) をご覧ください。

## ライセンス

このプロジェクトは [LICENSE](LICENSE.md) に記載されたライセンスに従います。
