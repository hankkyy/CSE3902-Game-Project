# Sprint 3 队员执行指南

每个人只做自己这一行，不要直接向 `main` 或 `integration/sprint3` 推送功能代码。

时间安排（America/New_York）：

- Sprint 开始：2026 年 10 月 7 日
- Functionality check-in：2026 年 10 月 19 日
- Sprint 3 最终截止：2026 年 10 月 26 日晚上 8:00

| 队员 | Issue | 分支 | Reviewer |
|---|---|---|---|
| Hank | [#26](https://github.com/hankkyy/CSE3902-Game-Project/issues/26) | `feature/s3-collision-core` | Xuanzhe |
| Xuanzhe | [#23](https://github.com/hankkyy/CSE3902-Game-Project/issues/23) | `feature/s3-room-loading` | Michael |
| Michael | [#22](https://github.com/hankkyy/CSE3902-Game-Project/issues/22) | `feature/s3-combat-collisions` | Leo |
| Leo | [#24](https://github.com/hankkyy/CSE3902-Game-Project/issues/24) | `feature/s3-items-blocks-collisions` | Ashley |
| Ashley | [#25](https://github.com/hankkyy/CSE3902-Game-Project/issues/25) | `feature/s3-room-navigation-integration` | Hank |

## 开始工作

把下面的 `YOUR_BRANCH` 换成表中的分支名：

```sh
git fetch origin
git switch --track origin/YOUR_BRANCH
git status
./scripts/verify.sh
```

开始写代码前必须完整阅读：

1. `AGENTS.md`
2. `docs/SPRINT3_REQUIREMENTS.md`
3. `docs/SPRINT3_ARCHITECTURE.md`
4. `docs/COLLISION_MATRIX.md`
5. 自己分支对应的 `.codex/tasks/s3-*.md`
6. 自己的 GitHub issue

然后把 project board 上自己的任务从 Todo 移到 In Progress。Start Date、
Functionality Check-in 和 Target Date 已统一设置；开始工作时不要自行更改团队截止日期。

## 每次工作结束

在 issue 留一条简短记录：

```text
Today:
- 完成了什么
- 跑了哪些测试

Actual time today: X hours
Total actual hours: X hours
Remaining hours: X hours
Blockers/API needs: none / 写清楚
```

同时更新 project board 的 Actual Hours 和 Remaining Hours。Remaining Hours 只能减少到真实剩余工作量，不能因为代码写完但测试/review 未完成就直接填 0。

## 提交 PR

```sh
./scripts/verify.sh
./scripts/analyze-strict.sh
git status
git push
```

PR 的 base 必须选择 `integration/sprint3`。在 PR 中：

- 写 `Closes #issue-number`；
- 给出可以重复执行的手工测试步骤；
- 对可见功能放截图或 GIF；
- 指定表中的 reviewer；
- 指定 reviewer 要检查的两个类；
- 填写 actual/remaining hours；
- 新的 analyzer warning 必须修复或逐条解释，不能整类 blanket suppress。

Reviewer 必须记录日期、花费分钟数、具体可读性意见、具体可维护性意见，以及一个 hypothetical future change。Review 完成后更新
`docs/code-reviews/SPRINT3_REVIEWS.md`。

## 合并规则

- 只有 CI、普通验证、严格分析记录、手工测试和指定 review 全部完成后才能合并。
- 功能 PR 合并进 `integration/sprint3`，不直接进 `main`。
- 需要改共享接口时，先在 issue 里说明，由 integrator 建一个小的 contract PR；不要在自己的分支偷偷改公共接口。
- 不要修改其他人的 owned paths，也不要把 `bin/`、`obj/`、zip、IDE 配置或未批准素材提交到仓库。

## Functionality check-in

所有人完成自己 task 文件中的 “Check-in required” 后，团队一起逐项检查
`docs/SPRINT3_CHECKLIST.md`。必须从 clean clone 运行并录制/记录完整演示，最后提交的 commit hash 和提交人要写入 checklist。
