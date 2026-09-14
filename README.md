# CSE 3902 – Sprint 1 Team Setup(Sprint 1 团队准备)

## Sprint 1 Goal(Sprint 1 目标)

Sprint 1 is mainly about getting our team ready for the semester project.

> Sprint 1 主要是让我们的团队为整个学期的项目做好准备。

We need to make sure everyone knows how to:

> 我们需要确保每个成员都知道如何:

* Use our shared GitHub repository

  > 使用我们的共享 GitHub 仓库

* Use GitHub Projects to track tasks

  > 使用 GitHub Projects 跟踪任务

* Create and update a task

  > 创建和更新任务

* Assign tasks to themselves

  > 将任务分配给自己

* Make changes to the project

  > 修改项目内容

* Commit and push code to GitHub

  > 提交(commit)并推送(push)代码到 GitHub

* Follow the same team workflow

  > 遵循统一的团队工作流程

We are using:

> 我们使用的工具:

* **Source Control:** GitHub

  > 源代码管理:GitHub

* **Task Tracking:** GitHub Projects

  > 任务跟踪:GitHub Projects

* **Board Workflow:** `Todo → In Progress → Done`

  > 看板工作流:`Todo → In Progress → Done`

---

# 1. Join the Team Repository(加入团队仓库)

The repo owner will invite everyone as a collaborator.

> 仓库所有者会邀请每个人成为协作者。

When you receive the GitHub invitation:

> 收到 GitHub 邀请后:

1. Accept the invitation.

   > 接受邀请。

2. Open this repository.

   > 打开此仓库。

3. Make sure you can see the project files.

   > 确认你能看到项目文件。

4. Make sure you can access the **Projects** tab.

   > 确认你能访问 **Projects** 标签页。

If you cannot access or push to the repository, tell the team before continuing.

> 如果你无法访问或无法推送到仓库,请先告诉团队,再继续后面的步骤。

---

# 2. GitHub Project Board(GitHub 项目看板)

We will use **GitHub Projects** to track our work.

> 我们将使用 **GitHub Projects** 来跟踪工作。

Our board will have three main statuses:

> 我们的看板有三个主要状态:

| Status(状态)     | Meaning(含义)                        |
| ---------------- | ------------------------------------ |
| **Todo**         | Task has not been started<br>任务尚未开始 |
| **In Progress**  | Someone is currently working on it<br>有人正在处理中 |
| **Done**         | Task is completed<br>任务已完成 |

Every task should move through:

> 每个任务都应按照以下流程移动:

**Todo → In Progress → Done**

Do not create a task and immediately move it to Done. The purpose of Sprint 1 is to practice updating and tracking progress.

> 不要创建任务后直接移到 Done。Sprint 1 的目的是练习更新和跟踪进度。

---

# 3. What Every Team Member Must Do(每个团队成员必须完成的事项)

Each team member should complete all of the following.

> 每个团队成员都应完成以下所有步骤。

### Step 1 — Create a Task(第 1 步 — 创建任务)

Open our GitHub Project board.

> 打开我们的 GitHub Project 看板。

Create at least **one task/issue** for yourself.

> 至少为自己创建一个**任务 / issue**。

Example tasks:

> 示例任务:

* Player movement

  > 玩家移动

* Player sprite setup

  > 玩家精灵设置

* Enemy movement

  > 敌人移动

* Collision system

  > 碰撞系统

* Room transition

  > 房间切换

* Item system

  > 道具系统

* HUD setup

  > HUD 设置

* Sprint 1 Git practice

  > Sprint 1 Git 练习

For Sprint 1, the task does **not necessarily need to be fully implemented**. The main goal is learning the task tracking workflow.

> Sprint 1 的任务**不一定要完整实现**。主要目标是学习任务跟踪的工作流程。

---

### Step 2 — Assign the Task to Yourself(第 2 步 — 把任务分配给自己)

Your task should have:

> 你的任务应包含:

* A clear title

  > 清晰的标题

* Your GitHub account as the **Assignee**

  > 你的 GitHub 账号作为 **Assignee(负责人)**

* Status = **Todo**

  > 状态 = **Todo**

* An **Effort** estimate

  > 一个 **Effort(工作量)** 估计值

Example:

> 示例:

**Task:** Player Movement

> **任务:** 玩家移动

**Assignee:** `your-github-username`

> **负责人:** `your-github-username`

**Status:** Todo

> **状态:** Todo

**Effort:** 3

> **工作量:** 3

Our Effort field represents approximately how much work is left.

> 我们的 Effort 字段大致表示剩余工作量。

---

### Step 3 — Start the Task(第 3 步 — 开始任务)

When you begin working on it:

> 开始处理任务时:

Change:

> 把状态改为:

**Todo → In Progress**

Also update the remaining Effort.

> 同时更新剩余 Effort。

Example:

> 示例:

Original:

> 初始:

`Effort: 3`

After making progress:

> 有进展后:

`Effort: 2`

The remaining effort should decrease, but it should not immediately become 0.

> 剩余 Effort 应该减少,但不要立即变成 0。

---

### Step 4 — Complete the Task(第 4 步 — 完成任务)

After the task is finished:

> 任务完成后:

Change:

> 把状态改为:

**In Progress → Done**

At this point the task is considered completed.

> 此时任务即视为完成。

---

# 4. Git / GitHub Practice(Git / GitHub 练习)

Every team member must also make at least **one change to the shared project and commit it to GitHub**.

> 每个团队成员还必须**对共享项目做出至少一次修改并提交到 GitHub**。

The change can be small for Sprint 1.

> Sprint 1 的修改可以很小。

For example:

> 例如:

```csharp
// Sprint 1 GitHub practice
// Sprint 1 GitHub 练习
```

or another small legitimate change to the project.

> 或者对项目做其他小的合理修改。

---

## Clone the Repository(克隆仓库)

If you do not already have the project on your computer:

> 如果你的电脑上还没有这个项目:

```bash
git clone REPOSITORY-URL
```

Then enter the project folder:

> 然后进入项目文件夹:

```bash
cd PROJECT-FOLDER
```

---

## Before Making Changes(修改之前)

Always pull the newest version first:

> 先拉取最新版本:

```bash
git pull
```

This helps avoid working on an old version of the project.

> 这有助于避免在旧版本上工作。

---

## Make Your Change(做出修改)

Open the project in Visual Studio / VS Code and make your change.

> 在 Visual Studio / VS Code 中打开项目并做出修改。

Then check what changed:

> 然后查看有哪些改动:

```bash
git status
```

---

## Commit Your Change(提交修改)

Add the changed files:

> 添加修改过的文件:

```bash
git add .
```

Create a commit:

> 创建一次提交:

```bash
git commit -m "Sprint 1 Git practice"
```

Push it to GitHub:

> 推送到 GitHub:

```bash
git push
```

After pushing, check the GitHub repository and make sure your commit appears in the commit history.

> 推送后,去 GitHub 仓库确认你的提交出现在提交历史中。

---

# 5. Sprint 1 Checklist(Sprint 1 检查清单)

Every team member should be able to check all of these boxes:

> 每个团队成员都应能勾选以下所有项目:

* [ ] Joined the team GitHub repository

  > 已加入团队 GitHub 仓库

* [ ] Can access the repository

  > 可以访问仓库

* [ ] Can access the GitHub Project board

  > 可以访问 GitHub Project 看板

* [ ] Created at least one task

  > 创建了至少一个任务

* [ ] Assigned the task to myself

  > 把任务分配给了自己

* [ ] Added an Effort estimate

  > 添加了 Effort 估计值

* [ ] Started the task as `Todo`

  > 任务初始状态为 `Todo`

* [ ] Changed the task to `In Progress`

  > 把任务改为 `In Progress`

* [ ] Updated/reduced the remaining Effort

  > 更新 / 减少了剩余 Effort

* [ ] Changed the task to `Done`

  > 把任务改为 `Done`

* [ ] Cloned/pulled the team repository

  > 克隆 / 拉取了团队仓库

* [ ] Made at least one code change or comment

  > 做了至少一处代码修改或注释

* [ ] Created at least one Git commit

  > 创建了至少一次 Git 提交

* [ ] Successfully pushed the commit to GitHub

  > 成功把提交推送到 GitHub

* [ ] Verified my commit appears on GitHub

  > 确认我的提交出现在 GitHub 上

---

# 6. Team Workflow(团队工作流程)

For the semester project, we will generally follow this workflow:

> 整个学期的项目中,我们大致遵循以下工作流程:

### New work(新工作)

Create a task:

> 创建任务:

`Todo`

↓

Assign it to the person responsible.

> 把任务分配给负责人。

↓

When work begins:

> 开始工作时:

`In Progress`

↓

Commit changes regularly to GitHub.

> 定期提交更改到 GitHub。

↓

When the work is finished:

> 工作完成时:

`Done`

---

# 7. Basic Git Rules(基本 Git 规则)

To reduce conflicts and lost work:

> 为了减少冲突和避免丢失工作成果:

### Before working(开始工作前)

```bash
git pull
```

### Check your changes(检查你的修改)

```bash
git status
```

### Save your changes to Git(把修改保存到 Git)

```bash
git add .
git commit -m "Describe what you changed"
```

### Upload your changes(上传你的修改)

```bash
git push
```

Use clear commit messages.

> 使用清晰的提交信息。

Good:

> 好的例子:

```text
Add player movement
添加玩家移动

Fix enemy collision
修复敌人碰撞

Add room transition logic
添加房间切换逻辑

Update player sprite
更新玩家精灵
```

Avoid messages like:

> 避免这样的信息:

```text
update
更新

stuff
东西

test123
测试123

asdf
```

---

# 8. Communication(沟通)

If you:

> 如果你:

* Cannot access the repo

  > 无法访问仓库

* Cannot push your code

  > 无法推送代码

* Have a merge conflict

  > 遇到合并冲突

* Do not understand your assigned task

  > 不理解分配给你的任务

* Cannot finish something before the Sprint deadline

  > 无法在 Sprint 截止前完成某些工作

tell the team as early as possible.

> 请尽早告诉团队。

Do not wait until the end of the Sprint.

> 不要等到 Sprint 结束时才说。

---

# 9. Sprint 1 Definition of Done(Sprint 1 完成标准)

Our Sprint 1 setup is complete when:

> 满足以下条件,Sprint 1 的准备工作即算完成:

1. Everyone is added to the GitHub repository.

   > 所有人都被添加到 GitHub 仓库。

2. Everyone can access the GitHub Project board.

   > 所有人都能访问 GitHub Project 看板。

3. Everyone has created and updated at least one task.

   > 所有人都创建并更新了至少一个任务。

4. Everyone has moved a task through the project workflow.

   > 所有人都让一个任务走完了项目工作流程。

5. Everyone has made at least one Git commit.

   > 所有人都做了至少一次 Git 提交。

6. Everyone has successfully pushed a change to the shared repository.

   > 所有人都成功把修改推送到了共享仓库。

7. GitHub shows contributions from all team members.

   > GitHub 上能看到所有团队成员的贡献。

8. The team understands how we will track work for future Sprints.

   > 团队理解了未来 Sprint 将如何跟踪工作。

After completing these steps, we should be ready to use the same workflow when development begins in Sprint 2.

> 完成这些步骤后,我们就可以在 Sprint 2 开始开发时使用同样的工作流程。
