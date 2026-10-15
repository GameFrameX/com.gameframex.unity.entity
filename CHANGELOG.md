## [3.0.1](https://github.com/gameframex/com.gameframex.unity.entity/compare/3.0.0...3.0.1) (2026-10-15)


### Bug Fixes

* **dependencies:** 对齐依赖至实际发布版本 ([0d147a7](https://github.com/gameframex/com.gameframex.unity.entity/commit/0d147a7654ae4d9ed7b1135666e8c4d2905d8aff))

# [3.0.0](https://github.com/gameframex/com.gameframex.unity.entity/compare/2.4.6...3.0.0) (2026-10-15)


* feat(module)!: 实体模块薄入口化并新增 EntityOptions ([0407009](https://github.com/gameframex/com.gameframex.unity.entity/commit/04070094031f50851f34dde707f61daf30e003d0))


### Bug Fixes

* **deps:** 同步依赖版本至最新 ([f6619c0](https://github.com/gameframex/com.gameframex.unity.entity/commit/f6619c0ed6bfd2e6c500c41c3cfae33b0850a561))
* **package:** 修正品牌命名 ([c64cfd6](https://github.com/gameframex/com.gameframex.unity.entity/commit/c64cfd68fcce8be78907198c7d8bd72940bcbdf2))


### BREAKING CHANGES

* EntityComponent 序列化字段移除，编辑期配置入口收敛到代码 EntityOptions。

## [Unreleased]

### ⚠ BREAKING CHANGES

* **module:** 新增 `EntityOptions` 启动配置（`GameFrameXOptions.Configure`，首场景加载前注入；字段默认值同原 Inspector 序列化默认）
* **module:** 组件值参数序列化字段移除，编辑期配置入口收敛到代码 Options；Play 期经活视图 / 模块 API 调整
* **module:** Helper 实现选择从类型名字符串改为 `Func` 工厂闭包（Custom 引用优先）

# [3.0.0](https://github.com/gameframex/com.gameframex.unity.entity/compare/2.4.6...3.0.0) (2026-10-08)


### ⚠ BREAKING CHANGES

* **module:** 模块契约全接口化：删除 `GameFrameworkModule` 抽象基类，改为 `IGameFrameworkModule` 标记接口 + 可选能力接口

**新契约形态与迁移对照：**

| 旧基类成员 | 新契约 | 迁移方式 |
|---|---|---|
| `: GameFrameworkModule` | `IGameFrameworkModule` | 声明处直接替换（标记接口，可注册进 `GameFrameworkEntry`） |
| `override void Update(float, float)` | `IUpdatableModule` | 真实实现改 `public`；空实现删除（未实现者不再被轮询） |
| `override void Shutdown()` | `IShutdownableModule` | 真实实现改 `public`；空实现删除（未实现者不再被关闭） |
| `override int Priority` | `IPrioritizedModule` | 改 `public int Priority { get; }`；未实现视为 0 |

本包变更：`EntityManager` 实现 `IUpdatableModule`、`IShutdownableModule`，6 处 partial 声明全部迁移。

`GameFrameworkEntry.Update` 仅分派 `IUpdatableModule`；`Shutdown` 按优先级反序仅分派 `IShutdownableModule`；tick 与关闭顺序与旧实现完全一致。能力方法仅由宿主入口调用，业务侧继续通过 `GetModule<IXxxManager>()` 访问模块。

## [2.4.6](https://github.com/gameframex/com.gameframex.unity.entity/compare/2.4.5...2.4.6) (2026-07-29)


### Bug Fixes

* **dependencies:** 更新依赖项版本以支持新功能和修复已知问题 ([66d2c7b](https://github.com/gameframex/com.gameframex.unity.entity/commit/66d2c7b3fdc05ad2812eafacbf3404058f2eaf57))

## [2.4.5](https://github.com/gameframex/com.gameframex.unity.entity/compare/2.4.4...2.4.5) (2026-07-03)


### Bug Fixes

* **entity:** 修正 EntityComponent 默认命名空间与空引用防护 ([0bbb04f](https://github.com/gameframex/com.gameframex.unity.entity/commit/0bbb04f02913735c5d2f5bb14f57ce4d680e2fff))

## [2.4.4](https://github.com/gameframex/com.gameframex.unity.entity/compare/2.4.3...2.4.4) (2026-06-08)


### Bug Fixes

* **entity:** 事件处理函数使用 Create 工厂方法创建 EventArgs ([da6a5bf](https://github.com/gameframex/com.gameframex.unity.entity/commit/da6a5bfb84251c39ec15dbaadf53750ab6f08f34))

## [2.4.3](https://github.com/gameframex/com.gameframex.unity.entity/compare/2.4.2...2.4.3) (2026-06-01)


### Bug Fixes

* **entity:** userData 安全类型转换与 OnRecycle null 检查防护 ([8fab6e4](https://github.com/gameframex/com.gameframex.unity.entity/commit/8fab6e44edbabe555c64332d8bae4ba332976299))
* **entity:** 修复 ReleaseEntityGroup 无效的保存恢复逻辑 ([f9deb80](https://github.com/gameframex/com.gameframex.unity.entity/commit/f9deb806b4ae80af9a6fea414679ea3ed186acc4))
* **entity:** 恢复 EventArgs 引用池释放，优化异常处理和异步取消 ([d4e1215](https://github.com/gameframex/com.gameframex.unity.entity/commit/d4e12159a6e5e9d3885f8e082528f49667212faa))


### Performance Improvements

* **entity:** EntityGroup 字典使用 instanceCapacity 作为初始容量 ([f6fa7e3](https://github.com/gameframex/com.gameframex.unity.entity/commit/f6fa7e30f1904441a2d44c34f3379e75d83fc4b9))
* **entity:** EntityGroup 添加 Dictionary 索引优化 O(1) 查找 ([9e1793b](https://github.com/gameframex/com.gameframex.unity.entity/commit/9e1793bfcacb98cffa0dd084245cba0ec9f2668b))
* **entity:** HideAllLoadedEntities 优化为 O(n) 批量处理 ([7cb9ad3](https://github.com/gameframex/com.gameframex.unity.entity/commit/7cb9ad3443e225136cdaf983e518822cb2bd8c64))

## [2.4.2](https://github.com/gameframex/com.gameframex.unity.entity/compare/2.4.1...2.4.2) (2026-05-28)


### Bug Fixes

* **ci:** 统一 .github 工作流配置 ([efd2f08](https://github.com/gameframex/com.gameframex.unity.entity/commit/efd2f08d6cec5545d05f7630e56819e8afc8a158))
* **deps:** 补充 package.json 中缺失的包依赖 ([166259c](https://github.com/gameframex/com.gameframex.unity.entity/commit/166259cfe3e742b1dbcaf9e00596140dc48bb499))

## [2.4.1](https://github.com/gameframex/com.gameframex.unity.entity/compare/2.4.0...2.4.1) (2026-03-05)


### Bug Fixes

* **EntityManager:** 修复实体资源加载成功判断条件 ([3dc46de](https://github.com/gameframex/com.gameframex.unity.entity/commit/3dc46de61ba0b4bd8e7e0e3197ae95030d00dd39))

# [2.4.0](https://github.com/gameframex/com.gameframex.unity.entity/compare/2.3.0...2.4.0) (2026-03-05)


### Bug Fixes

* **实体组件:** 将默认过期时间从极大值改为3600秒 ([fc8d84a](https://github.com/gameframex/com.gameframex.unity.entity/commit/fc8d84aab1d1c223df866b5d5834e10e58044fcd))


### Features

* **Entity:** 增加实体组默认容量和过期时间配置 ([06e0a48](https://github.com/gameframex/com.gameframex.unity.entity/commit/06e0a48e155257cfe818492cea0917ef964b97de))
* **Inspector:** 为实体组件检视器添加默认容量和过期时间配置 ([27edc8a](https://github.com/gameframex/com.gameframex.unity.entity/commit/27edc8add99d72be1d9bae4ff25302db76cf67a3))
* **实体组件:** 为实体实例对象池添加默认容量和过期时间的可配置属性 ([3c40983](https://github.com/gameframex/com.gameframex.unity.entity/commit/3c40983ce5d63cf0797c254eb2dcba76645f6de9))

# [2.3.0](https://github.com/gameframex/com.gameframex.unity.entity/compare/2.2.0...2.3.0) (2026-01-19)


### Features

* **实体管理:** 为实体组释放方法添加过期时间参数 ([1f8acdf](https://github.com/gameframex/com.gameframex.unity.entity/commit/1f8acdf87812799bfe4301d2dd85a317d88381a4))

# [2.2.0](https://github.com/gameframex/com.gameframex.unity.entity/compare/2.1.0...2.2.0) (2026-01-19)


### Features

* **实体管理:** 添加释放实体组功能 ([64737b4](https://github.com/gameframex/com.gameframex.unity.entity/commit/64737b4beb8be3cbbc5e511d7636316dba96a8fe))

# [2.1.0](https://github.com/gameframex/com.gameframex.unity.entity/compare/2.0.1...2.1.0) (2025-12-23)


### Features

* **ci:** change ci ([34acc7a](https://github.com/gameframex/com.gameframex.unity.entity/commit/34acc7a27595f0e3fb2aa851bcfeec6e0c820d3c))

# Changelog

## [Unreleased](https://github.com/GameFrameX/com.gameframex.unity.entity/tree/HEAD)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.entity/compare/2.0.0...HEAD)

**Merged pull requests:**

- 解决EntityManager.ShowEntityShow中嵌套GameFrameX.Entity.Runtime.ShowEntity… [\#2](https://github.com/GameFrameX/com.gameframex.unity.entity/pull/2) ([1802846624](https://github.com/1802846624))

## [2.0.0](https://github.com/GameFrameX/com.gameframex.unity.entity/tree/2.0.0) (2025-10-25)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.entity/compare/1.0.1...2.0.0)

## [1.0.1](https://github.com/GameFrameX/com.gameframex.unity.entity/tree/1.0.1) (2025-06-01)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.entity/compare/1.0.0...1.0.1)

## [1.0.0](https://github.com/GameFrameX/com.gameframex.unity.entity/tree/1.0.0) (2025-05-31)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.entity/compare/13d64b984dca6188f2ca1b8bb07d8c8546b82b1e...1.0.0)



\* *This Changelog was automatically generated by [github_changelog_generator](https://github.com/github-changelog-generator/github-changelog-generator)*
