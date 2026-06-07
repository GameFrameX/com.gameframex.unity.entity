<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Entity 엔티티 컴포넌트

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.entity)](https://github.com/GameFrameX/com.gameframex.unity.entity/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.entity)](https://github.com/GameFrameX/com.gameframex.unity.entity/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현

<br />

[문서](https://gameframex.doc.alianblank.com) · [빠른 시작](#빠른-시작) · QQ 그룹: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

</div>

## 📑 목차

- [프로젝트 개요](#프로젝트-개요)
- [주요 기능](#주요-기능)
- [빠른 시작](#빠른-시작)
  - [설치](#설치)
- [사용 예시](#사용-예시)
  - [1. EntityLogic 정의](#1-entitylogic-정의)
  - [2. 엔티티 표시](#2-엔티티-표시)
  - [3. 엔티티 숨기기](#3-엔티티-숨기기)
  - [4. 부모-자식 계층](#4-부모-자식-계층)
  - [5. 이벤트 구독](#5-이벤트-구독)
- [엔티티 라이프사이클](#엔티티-라이프사이클)
- [아키텍처](#아키텍처)
- [문서 및 자료](#문서-및-자료)
- [커뮤니티 및 지원](#커뮤니티-및-지원)
- [변경 로그](#변경-로그)
- [라이선스](#라이선스)

## 🏗️ 프로젝트 개요

**Entity 엔티티 컴포넌트** 는 Unity 게임을 위한 완전한 엔티티 관리 시스템을 제공합니다. 엔티티 라이프사이클(표시, 숨기기, 재활용), 그룹화, 부모-자식 계층을 관리하며, 에셋 시스템과 통합하여 비동기 리소스 로딩을, 객체 풀과 통합하여 인스턴스 재사용을 구현합니다.

GameFrameX 프레임워크 기반으로 구축되어 UniTask를 사용한 비동기 작업과 Manager-Component-Helper 계층 아키텍처를 채택하고 있습니다.

## ✨ 주요 기능

- **🎯 비동기 엔티티 생성** — UniTask 기반 비동기 로딩 및 인스턴스화, 진행률 콜백 및 종속 리소스 콜백 지원
- **📦 엔티티 그룹화** — 엔티티를 그룹으로 구성하며, 각 그룹에서 개별적으로 객체 풀 설정(용량, 만료 시간, 자동 해제 간격) 가능
- **🔗 부모-자식 계층** — 자식 엔티티 연결/해제, Transform 부모-자식 관계 자동 처리
- **♻️ 객체 풀 통합** — 객체 풀을 통한 엔티티 인스턴스 재사용으로 메모리 효율 향상
- **📢 이벤트 기반 라이프사이클** — 표시/숨기기/업데이트/종속 리소스 등의 이벤트를 구독하여 반응형 워크플로 구현
- **🧩 EntityLogic 패턴** — `EntityLogic` 서브클래싱으로 비즈니스 로직을 구현, OnInit/OnShow/OnHide/OnUpdate 등의 라이프사이클 메서드 제공
- **🔒 인스턴스 관리** — 엔티티 인스턴스 잠금 또는 우선순위 조정으로 풀 퇴거 제어 가능

## 🚀 빠른 시작

### 설치

Unity 프로젝트의 `Packages/manifest.json`을 편집하여 `scopedRegistries` 섹션을 추가하세요:

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

`scopes`는 이 레지스트리를 통해 어떤 패키지를 해석할지 제어합니다. `com.gameframex`로 시작하는 패키지만 이 레지스트리에서 가져옵니다.

## 📖 사용 예시

### 1. EntityLogic 정의

`EntityLogic` 서브클래스를 생성하여 엔티티 동작을 구현합니다:

```csharp
using GameFrameX.Entity.Runtime;
using UnityEngine;

public class PlayerEntity : EntityLogic
{
    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);
        // 컴포넌트 참조 초기화 등
    }

    protected internal override void OnShow(object userData)
    {
        base.OnShow(userData);
        // 엔티티 표시 시 호출
    }

    protected internal override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);
        // 엔티티 숨김 시 호출
    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        // 매 프레임 업데이트 로직
    }
}
```

### 2. 엔티티 표시

`EntityComponent`를 사용하여 엔티티를 비동기적으로 생성합니다:

```csharp
// EntityComponent 가져오기
var entityComponent = GameEntry.GetComponent<EntityComponent>();

// 엔티티 비동기 표시
IEntity entity = await entityComponent.ShowEntityAsync<PlayerEntity>(
    entityId: 1,
    entityAssetName: "Assets/Prefabs/Player.prefab",
    entityGroupName: "PlayerGroup"
);
```

### 3. 엔티티 숨기기

```csharp
// 엔티티 ID로 숨기기
entityComponent.HideEntity(1);

// 엔티티 참조로 숨기기
entityComponent.HideEntity(entity);

// 로드된 모든 엔티티 숨기기
entityComponent.HideAllLoadedEntities();
```

### 4. 부모-자식 계층

자식 엔티티를 연결하여 부모-자식 관계를 구축합니다:

```csharp
// 자식 엔티티를 부모 엔티티에 연결
entityComponent.AttachEntity(childEntity, parentEntity);

// 특정 Transform 경로에 연결
entityComponent.AttachEntity(childEntity, parentEntity, "Weapon/RightHand");

// 자식 엔티티 분리
entityComponent.DetachEntity(childEntity);

// 부모 엔티티의 모든 자식 분리
entityComponent.DetachChildEntities(parentEntity);
```

### 5. 이벤트 구독

```csharp
entityComponent.ShowEntitySuccess += (sender, e) =>
{
    Debug.Log($"엔티티 표시 성공: {e.Entity.Id}");
};

entityComponent.ShowEntityFailure += (sender, e) =>
{
    Debug.LogError($"엔티티 표시 실패: {e.ErrorMessage}");
};

entityComponent.HideEntityComplete += (sender, e) =>
{
    Debug.Log($"엔티티 숨김 완료: {e.EntityId}");
};
```

## 🔄 엔티티 라이프사이클

| 단계 | 메서드 | 설명 |
|------|--------|------|
| 초기화 | `OnInit(object userData)` | 최초 초기화, 참조 캐시 |
| 표시 | `OnShow(object userData)` | 엔티티가 표시되고 활성화됨 |
| 업데이트 | `OnUpdate(float, float)` | 표시 상태에서 매 프레임 호출 |
| 숨기기 | `OnHide(bool isShutdown, object userData)` | 엔티티가 숨겨짐 |
| 재활용 | `OnRecycle()` | 엔티티가 객체 풀에 반환됨 |

부모-자식 이벤트:

| 이벤트 | 부모 엔티티 | 자식 엔티티 |
|--------|-------------|-------------|
| 연결 | `OnAttached(childEntity, parentTransform, userData)` | `OnAttachTo(parentEntity, parentTransform, userData)` |
| 분리 | `OnDetached(childEntity, userData)` | `OnDetachFrom(parentEntity, userData)` |

## 🏛️ 아키텍처

```
Runtime/
├── Entity/
│   ├── EntityComponent.cs       # Unity MonoBehaviour 브릿지
│   ├── EntityLogic.cs           # 엔티티 동작 기본 클래스
│   ├── EntityHelperBase.cs      # 인스턴스화/해제 추상 헬퍼
│   ├── EntityGroupHelperBase.cs # 엔티티 그룹 추상 헬퍼
│   ├── Entity/
│   │   ├── IEntityManager.cs    # 핵심 매니저 인터페이스
│   │   ├── EntityManager.cs     # 매니저 구현 (partial class)
│   │   ├── IEntity.cs           # 엔티티 인터페이스
│   │   ├── IEntityGroup.cs      # 엔티티 그룹 인터페이스
│   │   └── ...
│   ├── ShowEntityInfo.cs        # 엔티티 표시 매개변수
│   └── AttachEntityInfo.cs      # 엔티티 연결 매개변수
├── EventArgs/                   # 라이프사이클 이벤트 인자
│   ├── ShowEntitySuccessEventArgs.cs
│   ├── ShowEntityFailureEventArgs.cs
│   ├── ShowEntityUpdateEventArgs.cs
│   ├── ShowEntityDependencyAssetEventArgs.cs
│   └── HideEntityCompleteEventArgs.cs
└── Editor/Inspector/            # 커스텀 인스펙터
```

## 📚 문서 및 자료

- [문서](https://gameframex.doc.alianblank.com)

## 💬 커뮤니티 및 지원

- QQ 그룹: 467608841 / 233840761

## 📋 변경 로그

변경 로그는 [Releases](https://github.com/gameframex/com.gameframex.unity.entity/releases)에서 확인하세요.

## 📄 라이선스

이 프로젝트는 [LICENSE](LICENSE.md)에 명시된 라이선스를 따릅니다.
