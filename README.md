<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Entity Component

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.entity)](https://github.com/GameFrameX/com.gameframex.unity.entity/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.entity)](https://github.com/GameFrameX/com.gameframex.unity.entity/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams

<br />

[Documentation](https://gameframex.doc.alianblank.com) · [Quick Start](#quick-start) · QQ Group: 467608841 / 233840761

<br />

**English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 📑 Table of Contents
- [Project Overview](#project-overview)
- [Key Features](#key-features)
- [Quick Start](#quick-start)
  - [Installation](#installation)
- [Usage Examples](#usage-examples)
  - [1. Define EntityLogic](#1-define-entitylogic)
  - [2. Show Entity](#2-show-entity)
  - [3. Hide Entity](#4-hide-entity)
  - [4. Parent-Child Hierarchy](#4-parent-child-hierarchy)
  - [5. Subscribe to Events](#5-subscribe-to-events)
- [Entity Lifecycle](#entity-lifecycle)
- [Architecture](#architecture)
- [Documentation & Resources](#documentation--resources)
- [Community & Support](#community--support)
- [Changelog](#changelog)
- [License](#license)

## 🏗️ Project Overview
The **Entity Component** provides a complete entity management system for Unity games. It handles entity lifecycle (show, hide, recycle), grouping, parent-child hierarchy, and integrates with the asset system for async resource loading and object pools for instance reuse.

Built on top of the GameFrameX framework, it uses UniTask for async operations and provides a Manager-Component-Helper layered architecture.

## ✨ Key Features
- **🎯 Async Entity Spawning** — Load and instantiate entities asynchronously via UniTask, with progress and dependency callbacks
- **📦 Entity Grouping** — Organize entities into groups with individual object pool settings (capacity, expire time, auto-release interval)
- **🔗 Parent-Child Hierarchy** — Attach/detach child entities with automatic Transform parenting
- **♻️ Object Pool Integration** — Reuse entity instances through object pools for memory efficiency
- **📢 Event-Driven Lifecycle** — Subscribe to Show/Hide/Update/DependencyAsset events for reactive workflows
- **🧩 EntityLogic Pattern** — Implement business logic by subclassing `EntityLogic` with OnInit/OnShow/OnHide/OnUpdate lifecycle methods
- **🔒 Instance Management** — Lock entity instances or adjust priority for pool eviction control

## 🚀 Quick Start
### Installation
Edit your Unity project's `Packages/manifest.json` and add the `scopedRegistries` section:

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

`scopes` controls which packages are resolved through this registry. Only packages whose names start with `com.gameframex` will be fetched from it.

## 📖 Usage Examples

### 1. Define EntityLogic
Create a subclass of `EntityLogic` to implement your entity's behavior:

```csharp
using GameFrameX.Entity.Runtime;
using UnityEngine;

public class PlayerEntity : EntityLogic
{
    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);
        // Initialize component references, etc.
    }

    protected internal override void OnShow(object userData)
    {
        base.OnShow(userData);
        // Called when the entity is shown
    }

    protected internal override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);
        // Called when the entity is hidden
    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        // Per-frame update logic
    }
}
```

### 2. Show Entity
Use `EntityComponent` to asynchronously spawn an entity:

```csharp
// Get the EntityComponent
var entityComponent = GameEntry.GetComponent<EntityComponent>();

// Show entity asynchronously
IEntity entity = await entityComponent.ShowEntityAsync<PlayerEntity>(
    entityId: 1,
    entityAssetName: "Assets/Prefabs/Player.prefab",
    entityGroupName: "PlayerGroup"
);
```

### 3. Hide Entity
```csharp
// Hide by entity ID
entityComponent.HideEntity(1);

// Hide by entity reference
entityComponent.HideEntity(entity);

// Hide all loaded entities
entityComponent.HideAllLoadedEntities();
```

### 4. Parent-Child Hierarchy
Attach child entities to form parent-child relationships:

```csharp
// Attach child to parent
entityComponent.AttachEntity(childEntity, parentEntity);

// Attach with a specific Transform path
entityComponent.AttachEntity(childEntity, parentEntity, "Weapon/RightHand");

// Detach child
entityComponent.DetachEntity(childEntity);

// Detach all children of a parent
entityComponent.DetachChildEntities(parentEntity);
```

### 5. Subscribe to Events
```csharp
entityComponent.ShowEntitySuccess += (sender, e) =>
{
    Debug.Log($"Entity shown: {e.Entity.Id}");
};

entityComponent.ShowEntityFailure += (sender, e) =>
{
    Debug.LogError($"Entity failed: {e.ErrorMessage}");
};

entityComponent.HideEntityComplete += (sender, e) =>
{
    Debug.Log($"Entity hidden: {e.EntityId}");
};
```

## 🔄 Entity Lifecycle

| Stage | Method | Description |
|-------|--------|-------------|
| Init | `OnInit(object userData)` | First-time initialization, cache references |
| Show | `OnShow(object userData)` | Entity becomes visible and active |
| Update | `OnUpdate(float, float)` | Per-frame update when entity is shown |
| Hide | `OnHide(bool isShutdown, object userData)` | Entity is hidden |
| Recycle | `OnRecycle()` | Entity returned to object pool |

Parent-child events:
| Event | Parent | Child |
|-------|--------|-------|
| Attach | `OnAttached(childEntity, parentTransform, userData)` | `OnAttachTo(parentEntity, parentTransform, userData)` |
| Detach | `OnDetached(childEntity, userData)` | `OnDetachFrom(parentEntity, userData)` |

## 🏛️ Architecture

```
Runtime/
├── Entity/
│   ├── EntityComponent.cs       # Unity MonoBehaviour bridge
│   ├── EntityLogic.cs           # Base class for entity behavior
│   ├── EntityHelperBase.cs      # Abstract helper for instantiation/release
│   ├── EntityGroupHelperBase.cs # Abstract helper for entity groups
│   ├── Entity/
│   │   ├── IEntityManager.cs    # Core manager interface
│   │   ├── EntityManager.cs     # Manager implementation (partial class)
│   │   ├── IEntity.cs           # Entity interface
│   │   ├── IEntityGroup.cs      # Entity group interface
│   │   └── ...
│   ├── ShowEntityInfo.cs        # Show entity parameters
│   └── AttachEntityInfo.cs      # Attach entity parameters
├── EventArgs/                   # Lifecycle event args
│   ├── ShowEntitySuccessEventArgs.cs
│   ├── ShowEntityFailureEventArgs.cs
│   ├── ShowEntityUpdateEventArgs.cs
│   ├── ShowEntityDependencyAssetEventArgs.cs
│   └── HideEntityCompleteEventArgs.cs
└── Editor/Inspector/            # Custom Inspector
```

## 📚 Documentation & Resources

- [Documentation](https://gameframex.doc.alianblank.com)

## 💬 Community & Support

- QQ Group: 467608841 / 233840761

## 📋 Changelog

See [Releases](https://github.com/gameframex/com.gameframex.unity.entity/releases) for changelog.

## 📄 License

This project is licensed under the terms in [LICENSE](LICENSE.md).
