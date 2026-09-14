# CSE 3902 – Sprint 1 Team Setup

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

---

# 1. Join the Team Repository

The repo owner will invite everyone as a collaborator.

When you receive the GitHub invitation:

1. Accept the invitation.
2. Open this repository.
3. Make sure you can see the project files.
4. Make sure you can access the **Projects** tab.

If you cannot access or push to the repository, tell the team before continuing.

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

---

### Step 4 — Complete the Task

After the task is finished:

Change:

**In Progress → Done**

At this point the task is considered completed.

---

# 4. Git / GitHub Practice

Every team member must also make at least **one change to the shared project and commit it to GitHub**.

The change can be small for Sprint 1.

For example:

```csharp
// Sprint 1 GitHub practice
```

or another small legitimate change to the project.

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

---

## Before Making Changes

Always pull the newest version first:

```bash
git pull
```

This helps avoid working on an old version of the project.

---

## Make Your Change

Open the project in Visual Studio / VS Code and make your change.

Then check what changed:

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
