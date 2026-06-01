<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="GameFrameX Logo" width="160"/>

# Game Frame X Entity Component

[![License](https://img.shields.io/github/license/gameframex/com.gameframex.unity.entity)](https://github.com/gameframex/com.gameframex.unity.entity/blob/main/LICENSE)
[![Version](https://img.shields.io/github/v/release/gameframex/com.gameframex.unity.entity)](https://github.com/gameframex/com.gameframex.unity.entity/releases)
[![Documentation](https://img.shields.io/badge/Documentation-Documentation-blue)](https://gameframex.doc.alianblank.com)

All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams

[Documentation](https://gameframex.doc.alianblank.com) · [Quick Start](#quick-start) · [QQ Group](https://qm.qq.com/q/5kbDVBdUeS) · **Language**

**English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

---

## Project Overview

The **Entity Component** provides interfaces for managing entity objects within the game framework.

## Quick Start

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

## Documentation & Resources

- [Documentation](https://gameframex.doc.alianblank.com)

## Community & Support

- [QQ Group](https://qm.qq.com/q/5kbDVBdUeS)

## Changelog

See [Releases](https://github.com/gameframex/com.gameframex.unity.entity/releases) for changelog.

## License

This project is licensed under the terms in [LICENSE](LICENSE.md).
