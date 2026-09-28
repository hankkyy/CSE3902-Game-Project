# CSE 3902 Game Project — Sprint 2

> # CSE 3902 游戏项目 — Sprint 2

A MonoGame functionality gallery for Sprint 2 (Game Objects and Sprites). The integrated build includes an original CC0 player atlas, dedicated player/enemy behaviors, a start menu, and an on-screen gameplay HUD. Dedicated item/block implementations remain assigned but unsubmitted.

> 这是 Sprint 2（游戏对象与精灵）的 MonoGame 功能展示项目。集成版本包含原创 CC0 玩家图集、独立玩家／敌人行为、开始菜单和游戏 HUD；独立物品／方块实现仍未提交。

## Functionality check-in status

> **功能检查进度**

The current build provides:

> 当前版本已经实现：

- Player movement with arrow keys or `WASD`, four facing directions, and walking animation.

> - 使用方向键或 `WASD` 移动玩家，支持四个朝向和行走动画。

- Attack state with `Z` or `N`, damage state with `E`, and item selection with `1`–`3`.

> - 使用 `Z` 或 `N` 攻击，使用 `E` 进入受伤状态，使用 `1`–`3` 选择物品。

- Stationary block gallery cycled with `T` and `Y`.

> - 使用 `T` 和 `Y` 循环切换静止方块。

- Animated item gallery cycled with `U` and `I`.

> - 使用 `U` 和 `I` 循环切换带动画的物品。

- Independently moving and animated enemy/NPC gallery cycled with `O` and `P`.

> - 使用 `O` 和 `P` 循环切换能够独立移动和播放动画的敌人或 NPC。

- Reset with `R`; quit with `Q` or `Escape`.

> - 使用 `R` 重置；使用 `Q` 或 `Escape` 退出。

- A start menu entered with `Enter`, with `Q`/`Escape` available in both the menu and gameplay.

> - 启动时显示菜单，按 `Enter` 进入游戏；菜单和游戏中都可以按 `Q`/`Escape` 退出。

- On-screen health, item slot, action, facing direction, gallery names/indexes, and control instructions.

> - 屏幕显示生命值、物品槽、角色动作、朝向、展示对象名称和序号，以及操作说明。

- Typed discrete commands and 25 headless input/menu regression checks.

> - 单次触发操作使用专门的命令类，并提供 25 项无需图形窗口的输入／菜单回归检查。

- Command, Factory Method, and object/sprite interfaces for separation of concerns.

> - 使用 Command、Factory Method 以及对象和精灵接口分离职责。

This is the completed Sprint 2 functionality gallery, not the final dungeon. The five assignments and their integration boundaries are recorded in [the Sprint 2 task plan](docs/SPRINT2_TASKS.md).

> 这是完成后的 Sprint 2 功能展示，并不是最终地牢。五项分工及集成边界记录在 [Sprint 2 五人任务计划](docs/SPRINT2_TASKS.md) 中。

## Requirements and setup

> **环境要求与安装**

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

> - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

- A Windows, macOS, or Linux graphics environment supported by MonoGame DesktopGL.

> - MonoGame DesktopGL 支持的 Windows、macOS 或 Linux 图形环境。

- Git.

> - Git。

Run these commands from the repository root:

> 在仓库根目录运行以下命令：

```sh
dotnet restore GameProject/GameProject.csproj
dotnet build GameProject/GameProject.csproj
dotnet run --project GameProject/GameProject.csproj
```

Visual Studio users can open `GameProject/GameProject.csproj` directly. VS Code and Rider users can open the repository root and run the same commands.

> Visual Studio 用户可以直接打开 `GameProject/GameProject.csproj`。VS Code 和 Rider 用户可以打开仓库根目录并运行相同命令。

## Controls

> **操作按键**

### `Enter`

Start gameplay from the menu. Gameplay controls are inactive until the game starts; a discrete key held in the menu must be released and pressed again to trigger in gameplay.

> **`Enter`**
>
> 从开始菜单进入游戏。进入前，移动、攻击、受伤和切换操作不会改变游戏对象；在菜单中按住的单次触发按键，需要松开再按才能在游戏中触发。

### Arrow keys or `WASD`

Move Link and change facing direction.

> **方向键或 `WASD`**
>
> 移动 Link 并改变朝向。

### `Z` or `N`

Attack with the sword. The HUD briefly shows `ATTACKING`, and the directional attack clip is selected from the player atlas.

> **`Z` 或 `N`**
>
> 使用剑攻击。HUD 会短暂显示 `ATTACKING`，同时播放玩家图集中的对应方向攻击动画。

### `1`, `2`, or `3`

Select the secondary item slot.

> **`1`、`2` 或 `3`**
>
> 选择副物品栏位。

### `E`

Damage Link.

> **`E`**
>
> 让 Link 受到伤害。

### `T` or `Y`

Show the previous or next block.

> **`T` 或 `Y`**
>
> 显示上一个或下一个方块。

### `U` or `I`

Show the previous or next item.

> **`U` 或 `I`**
>
> 显示上一个或下一个物品。

### `O` or `P`

Show the previous or next enemy/NPC.

> **`O` 或 `P`**
>
> 显示上一个或下一个敌人或 NPC。

### `R`

Reset all objects during gameplay, including health, selected item, positions, timers, and gallery indexes. Gameplay stays active.

> **`R`**
>
> 游戏中重置对象的位置、生命、物品选择、计时器和展示列表序号，并继续停留在游戏画面。

### `Q` or `Escape`

Quit from either the start menu or gameplay.

> **`Q` 或 `Escape`**
>
> 从开始菜单或游戏画面退出。

The top HUD shows health, selected item slot, action, and facing direction. The red and gold bars visualize health and item selection. The right panel shows each gallery's name, current selection, count, and previous/next keys. The bottom row lists the gameplay controls.

> 顶部 HUD 显示生命值、物品槽、动作与朝向，红条和金条分别表示生命与物品选择。右侧显示每组对象的名称、当前序号、总数和切换键；底部显示游戏操作说明。

## Architecture

> **项目架构**

```text
GameProject/
├── Commands/       Command interface and keyboard actions
├── Controllers/    Input mapping and key-edge detection
├── Core/           Shared object contracts, direction, gallery
├── Objects/        Player, blocks, items, enemies/NPCs
├── Sprites/        Drawing contracts, player animation clips, sprite factory
├── States/         Menu-to-gameplay session state
├── UI/             Start menu, pixel text, status HUD and help
├── Game1.cs        Composition root and game loop
└── Program.cs      Application entry point
```

```text
GameProject/
├── Commands/       命令接口与键盘操作
├── Controllers/    输入映射与按键边沿检测
├── Core/           公共对象契约、方向和展示列表
├── Objects/        玩家、方块、物品、敌人与 NPC
├── Sprites/        绘制契约、玩家动画片段与精灵工厂
├── States/         菜单与游戏中的会话状态
├── UI/             开始菜单、像素文字、状态与操作说明
├── Game1.cs        组合入口与游戏循环
└── Program.cs      程序入口
```

`Game1` composes the systems. `KeyboardController` translates input into commands. Objects own behavior and state but draw through `ISprite`; `SpriteFactory` owns visual construction. This lets art, player state, enemies, items, and controls evolve in separate branches with fewer merge conflicts.

> `Game1` 只负责组合各个系统；`KeyboardController` 将输入转换为命令。对象负责自己的行为和状态，但通过 `ISprite` 完成绘制；`SpriteFactory` 负责创建视觉表现。这样美术、玩家状态、敌人、物品和输入控制可以在不同分支中并行开发，减少合并冲突。

## Team workflow

> **团队协作流程**

1. Pick only your assigned task in [docs/SPRINT2_TASKS.md](docs/SPRINT2_TASKS.md).

   > 只选择任务计划中分配给自己的任务。

2. Check out the pre-created branch listed for you.

   > 切换到已经为自己创建好的分支。

3. Move the matching issue to **In Progress** and update remaining effort.

   > 将对应 Issue 移动到 **In Progress** 并更新剩余工作量。

4. Commit small, buildable changes; do not commit `bin/` or `obj/`.

   > 每次提交保持小而且可编译，不要提交 `bin/` 或 `obj/`。

5. Run `./scripts/verify.sh` and complete manual acceptance checks before opening a PR.

   > 创建 PR 前运行 `./scripts/verify.sh` 并完成人工验收。

6. Use the PR template and request the assigned reviewer; merge only after review and green CI.

   > 使用 PR 模板并邀请指定 reviewer，只能在 review 完成且 CI 通过后合并。

7. Move the issue to **Done** and set remaining effort to zero.

   > 完成后将 Issue 移动到 **Done**，并把剩余工作量设为零。

See [CONTRIBUTING.md](CONTRIBUTING.md) for exact Git commands and review rules.

> 具体 Git 命令和代码审查规则请查看 [CONTRIBUTING.md](CONTRIBUTING.md)。

### Codex consistency

> **Codex 一致性**

Every Codex session is governed by the repository-level [AGENTS.md](AGENTS.md). It freezes shared contracts, assigns file ownership, prohibits cross-feature rewrites, and defines one verification command. Each branch also has a focused prompt under `.codex/tasks/`. Do not ask Codex to “finish Sprint 2”; use only the task prompt matching the current branch.

> 每个 Codex 会话都必须遵守仓库根目录的 [AGENTS.md](AGENTS.md)。该文件冻结公共接口、规定文件所有权、禁止跨功能重写，并定义统一验证命令。每个分支在 `.codex/tasks/` 下还有专用任务提示。不要笼统地让 Codex“完成 Sprint 2”，只能使用与当前分支匹配的任务提示。

Recommended first prompt for every teammate:

> 每位队友推荐使用的第一条提示：

```text
Read AGENTS.md, docs/ARCHITECTURE_CONTRACT.md, and the task file matching this branch. Implement only that assignment, stay inside owned paths, run ./scripts/verify.sh, and summarize any integration needs.
```

```text
阅读 AGENTS.md、docs/ARCHITECTURE_CONTRACT.md，以及与当前分支对应的任务文件。只实现该分支的任务，只修改允许的路径，运行 ./scripts/verify.sh，并总结所有需要集成的内容。
```

## Known limitations

> **已知限制**

- The player uses an original CC0 atlas. Other gallery sprites remain simple generated primitives so no copyrighted game assets are committed.

> - 玩家使用原创 CC0 图集；其他展示对象仍使用简单生成图形，因此仓库中没有提交受版权保护的游戏素材。

- Object galleries demonstrate behaviors independently. Collision, room transitions, inventory UI, audio, and a complete dungeon are outside this check-in scaffold.

> - 对象展示区只用于独立演示行为；碰撞、房间切换、背包 UI、音效和完整地牢不属于本次 check-in 骨架范围。

- Enemy and item motion is deterministic; Octorok projectiles are demonstrations and do not interact with other objects during Sprint 2.

> - 敌人与物品运动是确定性的；Octorok 投射物仅用于演示，在 Sprint 2 中不与其他对象交互。

- Menu and HUD text use a small built-in pixel alphabet. It supports English letters, digits, and the punctuation used by the controls; it is not a general-purpose localized font.

> - 菜单和 HUD 使用内置像素字形，支持英文字母、数字及操作说明所需符号；它不是通用的多语言字体。

## Documentation

> **项目文档**

- [Sprint 2 requirements checklist](docs/SPRINT2_CHECKLIST.md)

> - [Sprint 2 要求检查表](docs/SPRINT2_CHECKLIST.md)

- [Five-person task plan](docs/SPRINT2_TASKS.md)

> - [五人任务计划](docs/SPRINT2_TASKS.md)

- [Architecture and integration contract](docs/ARCHITECTURE_CONTRACT.md)

> - [架构与集成契约](docs/ARCHITECTURE_CONTRACT.md)

- [Code review template](docs/code-reviews/TEMPLATE.md)

> - [代码审查模板](docs/code-reviews/TEMPLATE.md)

- [Sprint reflection template](docs/SPRINT2_REFLECTION_TEMPLATE.md)

> - [Sprint 回顾模板](docs/SPRINT2_REFLECTION_TEMPLATE.md)

- [Input/menu/HUD acceptance and release checklist](docs/INPUT_QUALITY_CHECKLIST.md)

> - [输入／菜单／HUD 验收与发布检查表](docs/INPUT_QUALITY_CHECKLIST.md)

## Current verification

> **当前验证状态**

Run the integrated verification command from the repository root before submitting:

> 提交前，在仓库根目录运行集成验证命令：

```sh
bash scripts/verify.sh
```

The verification script checks repository hygiene, formatting, analyzers, the Release build, and every submitted headless feature suite: player states and sprites, enemies/NPCs/projectiles, and input/menu behavior.

> 验证脚本检查仓库整洁度、格式、分析器、Release 构建，以及已提交的玩家状态和精灵、敌人／NPC／投射物和输入／菜单无窗口测试。

GitHub Actions runs the same `./scripts/verify.sh` command for pull requests targeting `main` and pushes to `main`. Actual keyboard bindings and visual layout still require the manual checks in the acceptance checklists.

> GitHub Actions 会在目标为 `main` 的 PR 和推送到 `main` 时运行同一个 `./scripts/verify.sh`。实际键盘绑定与画面布局仍需按验收清单人工检查。
