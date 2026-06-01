<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="GameFrameX Logo" width="160"/>

# Game Frame X Entity 实体对象组件

[![License](https://img.shields.io/github/license/gameframex/com.gameframex.unity.entity)](https://github.com/gameframex/com.gameframex.unity.entity/blob/main/LICENSE)
[![Version](https://img.shields.io/github/v/release/gameframex/com.gameframex.unity.entity)](https://github.com/gameframex/com.gameframex.unity.entity/releases)
[![Documentation](https://img.shields.io/badge/Documentation-文档-blue)](https://gameframex.doc.alianblank.com)

独立游戏前后端一体化解决方案 · 独立游戏开发者的圆梦大使

[文档](https://gameframex.doc.alianblank.com) · [快速开始](#快速开始) · [QQ群](https://qm.qq.com/q/5kbDVBdUeS) · **语言**

[English](README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

---

## 项目简介

**Entity 实体对象组件 (Entity Component)** - 提供实体对象组件相关的接口。

## 快速开始

### 安装方式

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

## 文档与资源

- [文档](https://gameframex.doc.alianblank.com)

## 社区与支持

- [QQ群](https://qm.qq.com/q/5kbDVBdUeS)

## 更新日志

查看 [Releases](https://github.com/gameframex/com.gameframex.unity.entity/releases) 了解更新日志。

## 开源协议

本项目采用 [LICENSE](LICENSE.md) 中声明的开源协议。
