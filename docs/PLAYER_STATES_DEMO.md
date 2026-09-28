# Player states demonstration

> **玩家状态演示**

This checklist demonstrates the progress on issue #8 without requiring debug tools.

> 这份检查表可以在不使用调试工具的情况下演示 Issue #8 的进展。

1. Run `dotnet run --project GameProject/GameProject.csproj`.

   > 运行 `dotnet run --project GameProject/GameProject.csproj`。

2. Hold each arrow key or `WASD`. Link enters Walking, faces the matching direction, animates, and remains inside the green panel.

   > 分别按住方向键或 `WASD`。Link 会进入 Walking 状态、朝向对应方向、播放动画，并保持在绿色区域内。

3. Face each direction and press `Z` or `N`. Link enters Attacking, briefly lunges in the facing direction, cannot move during the attack, and returns to Idle or Walking.

   > 面向四个方向分别按 `Z` 或 `N`。Link 会进入 Attacking 状态、短暂向当前方向突进、攻击期间不能移动，并在结束后返回 Idle 或 Walking。

4. Press `E`. The red health bar loses one segment, Link enters Damaged, flashes, ignores attacks and movement briefly, then recovers.

   > 按 `E`。红色生命条减少一格，Link 进入 Damaged 状态并闪烁，短时间内忽略攻击和移动，然后恢复。

5. Press `E` repeatedly during one damage flash. Health decreases only once because the damaged state prevents repeated damage.

   > 在一次受伤闪烁期间连续按 `E`。生命值只减少一次，因为 Damaged 状态会阻止重复受伤。

6. Press `1`, `2`, and `3`. The gold item-selection bar changes to one, two, and three segments.

   > 分别按 `1`、`2` 和 `3`。金色物品选择条会变为一格、两格和三格。

7. Press `R` during any action. Position, facing, state, health, selected item, timers, and animation reset to their initial values.

   > 在任意动作期间按 `R`。位置、朝向、状态、生命值、所选物品、计时器和动画都会恢复初始值。
