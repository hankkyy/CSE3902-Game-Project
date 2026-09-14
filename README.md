# CSE 3902 – Sprint 1 Team Setup

# CSE 3902 – Sprint 1 团队准备

## Sprint 1 Goal

Sprint 1 is mainly about getting our team ready for the semester project.

We need to make sure everyone knows how to:

* Use our shared GitHub repository
* Use GitHub Projects to track tasks
* Create and update a task
* Assign tasks to themselves
* Make changes to the project
* Commit and push code to GitHub
* Follow the same team workflow

We are using:

* **Source Control:** GitHub
* **Task Tracking:** GitHub Projects
* **Board Workflow:** `Todo → In Progress → Done`

## Sprint 1 目标

Sprint 1 主要是让我们的团队为整个学期的项目做好准备。

我们需要确保每个成员都知道如何:

* 使用我们的共享 GitHub 仓库
* 使用 GitHub Projects 跟踪任务
* 创建和更新任务
* 将任务分配给自己
* 修改项目内容
* 提交(commit)并推送(push)代码到 GitHub
* 遵循统一的团队工作流程

我们使用的工具:

* **源代码管理:** GitHub
* **任务跟踪:** GitHub Projects
* **看板工作流:** `Todo → In Progress → Done`

---

# 1. Join the Team Repository

The repo owner will invite everyone as a collaborator.

When you receive the GitHub invitation:

1. Accept the invitation.
2. Open this repository.
3. Make sure you can see the project files.
4. Make sure you can access the **Projects** tab.

If you cannot access or push to the repository, tell the team before continuing.

# 1. 加入团队仓库

仓库所有者会邀请每个人成为协作者。

收到 GitHub 邀请后:

1. 接受邀请。
2. 打开此仓库。
3. 确认你能看到项目文件。
4. 确认你能访问 **Projects** 标签页。

如果你无法访问或无法推送到仓库,请先告诉团队,再继续后面的步骤。

---

# 2. GitHub Project Board

We will use **GitHub Projects** to track our work.

Our board will have three main statuses:

| Status          | Meaning                            |
| --------------- | ---------------------------------- |
| **Todo**        | Task has not been started          |
| **In Progress** | Someone is currently working on it |
| **Done**        | Task is completed                  |

Every task should move through:

**Todo → In Progress → Done**

Do not create a task and immediately move it to Done. The purpose of Sprint 1 is to practice updating and tracking progress.

# 2. GitHub 项目看板

我们将使用 **GitHub Projects** 来跟踪工作。

我们的看板有三个主要状态:

| 状态            | 含义           |
| --------------- | -------------- |
| **Todo**        | 任务尚未开始   |
| **In Progress** | 有人正在处理中 |
| **Done**        | 任务已完成     |

每个任务都应按照以下流程移动:

**Todo → In Progress → Done**

不要创建任务后直接移到 Done。Sprint 1 的目的是练习更新和跟踪进度。

---

# 3. What Every Team Member Must Do

Each team member should complete all of the following.

### Step 1 — Create a Task

Open our GitHub Project board.

Create at least **one task/issue** for yourself.

Example tasks:

* Player movement
* Player sprite setup
* Enemy movement
* Collision system
* Room transition
* Item system
* HUD setup
* Sprint 1 Git practice

For Sprint 1, the task does **not necessarily need to be fully implemented**. The main goal is learning the task tracking workflow.

### 第 1 步 — 创建任务

打开我们的 GitHub Project 看板。

至少为自己创建一个**任务 / issue**。

示例任务:

* Player movement(玩家移动)
* Player sprite setup(玩家精灵设置)
* Enemy movement(敌人移动)
* Collision system(碰撞系统)
* Room transition(房间切换)
* Item system(道具系统)
* HUD setup(HUD 设置)
* Sprint 1 Git practice(Sprint 1 Git 练习)

Sprint 1 的任务**不一定要完整实现**。主要目标是学习任务跟踪的工作流程。

---

### Step 2 — Assign the Task to Yourself

Your task should have:

* A clear title
* Your GitHub account as the **Assignee**
* Status = **Todo**
* An **Effort** estimate

Example:

**Task:** Player Movement
**Assignee:** `your-github-username`
**Status:** Todo
**Effort:** 3

Our Effort field represents approximately how much work is left.

### 第 2 步 — 把任务分配给自己

你的任务应包含:

* 清晰的标题
* 你的 GitHub 账号作为 **Assignee(负责人)**
* 状态 = **Todo**
* 一个 **Effort(工作量)** 估计值

示例:

**任务:** Player Movement
**负责人:** `your-github-username`
**状态:** Todo
**Effort:** 3

我们的 Effort 字段大致表示剩余工作量。

---

### Step 3 — Start the Task

When you begin working on it:

Change:

**Todo → In Progress**

Also update the remaining Effort.

Example:

Original:

`Effort: 3`

After making progress:

`Effort: 2`

The remaining effort should decrease, but it should not immediately become 0.

### 第 3 步 — 开始任务

开始处理任务时:

把状态改为:

**Todo → In Progress**

同时更新剩余 Effort。

示例:

初始:

`Effort: 3`

有进展后:

`Effort: 2`

剩余 Effort 应该减少,但不要立即变成 0。

---

### Step 4 — Complete the Task

After the task is finished:

Change:

**In Progress → Done**

At this point the task is considered completed.

### 第 4 步 — 完成任务

任务完成后:

把状态改为:

**In Progress → Done**

此时任务即视为完成。

---

# 4. Git / GitHub Practice

Every team member must also make at least **one change to the shared project and commit it to GitHub**.

The change can be small for Sprint 1.

For example:

```csharp
// Sprint 1 GitHub practice
```

or another small legitimate change to the project.

# 4. Git / GitHub 练习

每个团队成员还必须**对共享项目做出至少一次修改并提交到 GitHub**。

Sprint 1 的修改可以很小。

例如:

```csharp
// Sprint 1 GitHub practice
```

或者对项目做其他小的合理修改。

---

## Clone the Repository

If you do not already have the project on your computer:

```bash
git clone REPOSITORY-URL
```

Then enter the project folder:

```bash
cd PROJECT-FOLDER
```

## 克隆仓库

如果你的电脑上还没有这个项目:

```bash
git clone REPOSITORY-URL
```

然后进入项目文件夹:

```bash
cd PROJECT-FOLDER
```

---

## Before Making Changes

Always pull the newest version first:

```bash
git pull
```

This helps avoid working on an old version of the project.

## 修改之前

先拉取最新版本:

```bash
git pull
```

这有助于避免在旧版本上工作。

---

## Make Your Change

Open the project in Visual Studio / VS Code and make your change.

Then check what changed:

```bash
git status
```

## 做出修改

在 Visual Studio / VS Code 中打开项目并做出修改。

然后查看有哪些改动:

```bash
git status
```

---

## Commit Your Change

Add the changed files:

```bash
git add .
```

Create a commit:

```bash
git commit -m "Sprint 1 Git practice"
```

Push it to GitHub:

```bash
git push
```

After pushing, check the GitHub repository and make sure your commit appears in the commit history.

## 提交修改

添加修改过的文件:

```bash
git add .
```

创建一次提交:

```bash
git commit -m "Sprint 1 Git practice"
```

推送到 GitHub:

```bash
git push
```

推送后,去 GitHub 仓库确认你的提交出现在提交历史中。

---

# 5. Sprint 1 Checklist

Every team member should be able to check all of these boxes:

* [ ] Joined the team GitHub repository
* [ ] Can access the repository
* [ ] Can access the GitHub Project board
* [ ] Created at least one task
* [ ] Assigned the task to myself
* [ ] Added an Effort estimate
* [ ] Started the task as `Todo`
* [ ] Changed the task to `In Progress`
* [ ] Updated/reduced the remaining Effort
* [ ] Changed the task to `Done`
* [ ] Cloned/pulled the team repository
* [ ] Made at least one code change or comment
* [ ] Created at least one Git commit
* [ ] Successfully pushed the commit to GitHub
* [ ] Verified my commit appears on GitHub

# 5. Sprint 1 检查清单

每个团队成员都应能勾选以下所有项目:

* [ ] 已加入团队 GitHub 仓库
* [ ] 可以访问仓库
* [ ] 可以访问 GitHub Project 看板
* [ ] 创建了至少一个任务
* [ ] 把任务分配给了自己
* [ ] 添加了 Effort 估计值
* [ ] 任务初始状态为 `Todo`
* [ ] 把任务改为 `In Progress`
* [ ] 更新 / 减少了剩余 Effort
* [ ] 把任务改为 `Done`
* [ ] 克隆 / 拉取了团队仓库
* [ ] 做了至少一处代码修改或注释
* [ ] 创建了至少一次 Git 提交
* [ ] 成功把提交推送到 GitHub
* [ ] 确认我的提交出现在 GitHub 上

---

# 6. Team Workflow

For the semester project, we will generally follow this workflow:

### New work

Create a task:

`Todo`

↓

Assign it to the person responsible.

↓

When work begins:

`In Progress`

↓

Commit changes regularly to GitHub.

↓

When the work is finished:

`Done`

# 6. 团队工作流程

整个学期的项目中,我们大致遵循以下工作流程:

### 新工作

创建任务:

`Todo`

↓

把任务分配给负责人。

↓

开始工作时:

`In Progress`

↓

定期提交更改到 GitHub。

↓

工作完成时:

`Done`

---

# 7. Basic Git Rules

To reduce conflicts and lost work:

### Before working

```bash
git pull
```

### Check your changes

```bash
git status
```

### Save your changes to Git

```bash
git add .
git commit -m "Describe what you changed"
```

### Upload your changes

```bash
git push
```

Use clear commit messages.

Good:

```text
Add player movement
Fix enemy collision
Add room transition logic
Update player sprite
```

Avoid messages like:

```text
update
stuff
test123
asdf
```

# 7. 基本 Git 规则

为了减少冲突和避免丢失工作成果:

### 开始工作前

```bash
git pull
```

### 检查你的修改

```bash
git status
```

### 把修改保存到 Git

```bash
git add .
git commit -m "描述你的修改"
```

### 上传你的修改

```bash
git push
```

使用清晰的提交信息。

好的例子:

```text
Add player movement
Fix enemy collision
Add room transition logic
Update player sprite
```

避免这样的信息:

```text
update
stuff
test123
asdf
```

---

# 8. Communication

If you:

* Cannot access the repo
* Cannot push your code
* Have a merge conflict
* Do not understand your assigned task
* Cannot finish something before the Sprint deadline

tell the team as early as possible.

Do not wait until the end of the Sprint.

# 8. 沟通

如果你:

* 无法访问仓库
* 无法推送代码
* 遇到合并冲突
* 不理解分配给你的任务
* 无法在 Sprint 截止前完成某些工作

请尽早告诉团队。

不要等到 Sprint 结束时才说。

---

# 9. Sprint 1 Definition of Done

Our Sprint 1 setup is complete when:

1. Everyone is added to the GitHub repository.
2. Everyone can access the GitHub Project board.
3. Everyone has created and updated at least one task.
4. Everyone has moved a task through the project workflow.
5. Everyone has made at least one Git commit.
6. Everyone has successfully pushed a change to the shared repository.
7. GitHub shows contributions from all team members.
8. The team understands how we will track work for future Sprints.

After completing these steps, we should be ready to use the same workflow when development begins in Sprint 2.

# 9. Sprint 1 完成标准(Definition of Done)

满足以下条件,Sprint 1 的准备工作即算完成:

1. 所有人都被添加到 GitHub 仓库。
2. 所有人都能访问 GitHub Project 看板。
3. 所有人都创建并更新了至少一个任务。
4. 所有人都让一个任务走完了项目工作流程。
5. 所有人都做了至少一次 Git 提交。
6. 所有人都成功把修改推送到了共享仓库。
7. GitHub 上能看到所有团队成员的贡献。
8. 团队理解了未来 Sprint 将如何跟踪工作。

完成这些步骤后,我们就可以在 Sprint 2 开始开发时使用同样的工作流程。
