# CSE 3902 Game Project — Sprint 2 / CSE 3902 游戏项目 — Sprint 2

A MonoGame functionality gallery for Sprint 2 (Game Objects and Sprites). The current build is deliberately asset-free: colored placeholder sprites make every required state visible while the team works in parallel on final art and object behavior.

> 这是 Sprint 2（游戏对象与精灵）的 MonoGame 功能展示项目。目前版本特意不依赖外部美术资源，而是使用彩色占位精灵展示所有必需状态，方便团队并行开发最终素材和对象行为。

## Functionality check-in status / 功能检查进度

The starter currently provides:

> 当前项目骨架已经实现：

- Player movement with arrow keys or `WASD`, four facing directions, and walking animation. / 使用方向键或 `WASD` 移动玩家，支持四个朝向和行走动画。
- Sword attack with `Z` or `N`, damage state with `E`, and item selection with `1`–`3`. / 使用 `Z` 或 `N` 攻击，使用 `E` 进入受伤状态，使用 `1`–`3` 选择物品。
- Stationary block gallery cycled with `T` / `Y`. / 使用 `T` / `Y` 循环切换静止方块。
- Animated item gallery cycled with `U` / `I`. / 使用 `U` / `I` 循环切换带动画的物品。
- Independently moving and animated enemy/NPC gallery cycled with `O` / `P`. / 使用 `O` / `P` 循环切换能够独立移动和播放动画的敌人或 NPC。
- Reset with `R`; quit with `Q` or `Escape`. / 使用 `R` 重置；使用 `Q` 或 `Escape` 退出。
- Command, Factory Method, and object/sprite interfaces for separation of concerns. / 使用 Command、Factory Method 以及对象/精灵接口分离职责。

This is a functional foundation, not the final dungeon. Placeholder visuals and simplified motion will be replaced through the five assigned feature branches in [the Sprint 2 task plan](docs/SPRINT2_TASKS.md).

> 这是可运行的功能基础，并不是最终地牢。占位画面和简化运动将由 [Sprint 2 五人任务计划](docs/SPRINT2_TASKS.md) 中的五个功能分支逐步替换。

## Requirements and setup / 环境要求与安装

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) / .NET 8 SDK
- A Windows, macOS, or Linux graphics environment supported by MonoGame DesktopGL / MonoGame DesktopGL 支持的 Windows、macOS 或 Linux 图形环境
- Git

Run these commands from the repository root:

> 在仓库根目录运行以下命令：

```sh
dotnet restore GameProject/GameProject.csproj
dotnet build GameProject/GameProject.csproj
dotnet run --project GameProject/GameProject.csproj
```

Visual Studio users can open `GameProject/GameProject.csproj` directly. VS Code and Rider users can open the repository root and run the same commands.

> Visual Studio 用户可以直接打开 `GameProject/GameProject.csproj`。VS Code 和 Rider 用户可以打开仓库根目录并运行相同命令。

## Controls / 操作按键

| Keys / 按键 | Action / 功能 |
|---|---|
| Arrow keys / `WASD` | Move Link and change facing direction / 移动 Link 并改变朝向 |
| `Z` / `N` | Sword attack / 使用剑攻击 |
| `1`, `2`, `3` | Select secondary item slot / 选择副物品栏位 |
| `E` | Damage Link / 让 Link 受到伤害 |
| `T` / `Y` | Previous / next block / 上一个或下一个方块 |
| `U` / `I` | Previous / next item / 上一个或下一个物品 |
| `O` / `P` | Previous / next enemy or NPC / 上一个或下一个敌人或 NPC |
| `R` | Reset all objects / 重置所有对象 |
| `Q` / `Escape` | Quit / 退出游戏 |

The top red bar represents player health. The gold bar represents the selected item slot. Dots in the right panel show the selected block, item, and enemy.

> 顶部红色条表示玩家生命值，金色条表示当前物品栏位，右侧面板中的圆点分别表示当前选择的方块、物品和敌人。

## Architecture / 项目架构

```text
GameProject/
├── Commands/       Command interface and keyboard actions / 命令接口与键盘操作
├── Controllers/    Input mapping and key-edge detection / 输入映射与按键边沿检测
├── Core/           Shared object contracts, direction, gallery / 公共对象契约、方向和展示列表
├── Objects/        Player, blocks, items, enemies/NPCs / 玩家、方块、物品、敌人与 NPC
├── Sprites/        Drawing contract, placeholders, sprite factory / 绘制契约、占位精灵与精灵工厂
├── Game1.cs        Composition root and game loop / 组合入口与游戏循环
└── Program.cs      Application entry point / 程序入口
```

`Game1` composes the systems. `KeyboardController` translates input into commands. Objects own behavior and state but draw through `ISprite`; `SpriteFactory` owns visual construction. This lets art, player state, enemies, items, and controls evolve in separate branches with fewer merge conflicts.

> `Game1` 只负责组合各个系统；`KeyboardController` 将输入转换为命令。对象负责自己的行为和状态，但通过 `ISprite` 完成绘制；`SpriteFactory` 负责创建视觉表现。这样美术、玩家状态、敌人、物品和输入控制可以在不同分支中并行开发，减少合并冲突。

## Team workflow / 团队协作流程

1. Pick only your assigned task in [docs/SPRINT2_TASKS.md](docs/SPRINT2_TASKS.md). / 只选择任务计划中分配给自己的任务。
2. Check out the pre-created branch listed for you. / 切换到已经为自己创建好的分支。
3. Move the matching issue to **In Progress** and update remaining effort. / 将对应 Issue 移动到 **In Progress** 并更新剩余工作量。
4. Commit small, buildable changes; do not commit `bin/` or `obj/`. / 每次提交保持小而且可编译，不要提交 `bin/` 或 `obj/`。
5. Run `./scripts/verify.sh` and complete manual acceptance checks before opening a PR. / 创建 PR 前运行 `./scripts/verify.sh` 并完成人工验收。
6. Use the PR template and request the assigned reviewer; merge only after review and green CI. / 使用 PR 模板并邀请指定 reviewer，只能在 review 完成且 CI 通过后合并。
7. Move the issue to **Done** and set remaining effort to zero. / 完成后将 Issue 移动到 **Done**，并把剩余工作量设为零。

See [CONTRIBUTING.md](CONTRIBUTING.md) for exact Git commands and review rules.

> 具体 Git 命令和代码审查规则请查看 [CONTRIBUTING.md](CONTRIBUTING.md)。

### Codex consistency / Codex 一致性

Every Codex session is governed by the repository-level [AGENTS.md](AGENTS.md). It freezes shared contracts, assigns file ownership, prohibits cross-feature rewrites, and defines one verification command. Each branch also has a focused prompt under `.codex/tasks/`. Do not ask Codex to “finish Sprint 2”; use only the task prompt matching the current branch.

> 每个 Codex 会话都必须遵守仓库根目录的 [AGENTS.md](AGENTS.md)。该文件冻结公共接口、规定文件所有权、禁止跨功能重写，并定义统一验证命令。每个分支在 `.codex/tasks/` 下还有专用任务提示。不要笼统地让 Codex“完成 Sprint 2”，只能使用与当前分支匹配的任务提示。

Recommended first prompt for every teammate:

> 每位队友推荐使用的第一条提示：

```text
Read AGENTS.md, docs/ARCHITECTURE_CONTRACT.md, and the task file matching this branch. Implement only that assignment, stay inside owned paths, run ./scripts/verify.sh, and summarize any integration needs.

阅读 AGENTS.md、docs/ARCHITECTURE_CONTRACT.md，以及与当前分支对应的任务文件。只实现该分支的任务，只修改允许的路径，运行 ./scripts/verify.sh，并总结所有需要集成的内容。
```

## Known limitations / 已知限制

- Sprites are geometric placeholders; no copyrighted game assets are committed. / 当前精灵是几何占位图，仓库中没有提交受版权保护的游戏素材。
- Object galleries demonstrate behaviors independently. Collision, room transitions, inventory UI, audio, and a complete dungeon are outside this check-in scaffold. / 对象展示区只用于独立演示行为；碰撞、房间切换、背包 UI、音效和完整地牢不属于本次 check-in 骨架范围。
- Enemy motion is deterministic and projectiles are assigned to the enemy task. / 当前敌人运动是确定性的，投射物功能已分配给敌人任务。
- There is no on-screen font yet; this README is the current control reference. / 当前还没有屏幕字体，本 README 是现阶段的操作说明。

## Documentation / 项目文档

- [Sprint 2 requirements checklist / Sprint 2 要求检查表](docs/SPRINT2_CHECKLIST.md)
- [Five-person task plan / 五人任务计划](docs/SPRINT2_TASKS.md)
- [Architecture and integration contract / 架构与集成契约](docs/ARCHITECTURE_CONTRACT.md)
- [Code review template / 代码审查模板](docs/code-reviews/TEMPLATE.md)
- [Sprint reflection template / Sprint 回顾模板](docs/SPRINT2_REFLECTION_TEMPLATE.md)

## Current verification / 当前验证状态

`dotnet restore`, formatting, analyzers, and the Release build pass with zero warnings and zero errors. GitHub Actions runs the same `./scripts/verify.sh` command on every pull request and push to `main`.

> `dotnet restore`、代码格式检查、analyzer 和 Release 构建均已通过，结果为零警告、零错误。GitHub Actions 会在每个 PR 和每次推送到 `main` 时运行同一个 `./scripts/verify.sh` 命令。
