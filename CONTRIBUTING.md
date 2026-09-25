# Contributing

## First-time setup

```sh
git clone https://github.com/hankkyy/CSE3902-Game-Project.git
cd CSE3902-Game-Project
dotnet restore GameProject/GameProject.csproj
dotnet build GameProject/GameProject.csproj
```

## Start an assigned task

The five task branches are already on GitHub. Use the exact branch name in `docs/SPRINT2_TASKS.md`:

```sh
git fetch origin
git switch --track origin/feature/your-task
```

When using Codex, it automatically reads the root `AGENTS.md`. Before asking it to implement anything, make sure it also reads `docs/ARCHITECTURE_CONTRACT.md` and the matching `.codex/tasks/*.md` file. A good first prompt is:

> Read AGENTS.md, docs/ARCHITECTURE_CONTRACT.md, and the task file matching this branch. Implement only that assignment, stay inside owned paths, run ./scripts/verify.sh, and summarize any integration needs.

Do not share a branch. Do not push directly to `main`. If two tasks need the same API, agree on the interface first and merge that small change before the implementations.

## Before every push

```sh
./scripts/verify.sh
git status
```

If formatting fails, run `dotnet format GameProject/GameProject.csproj`, inspect the changes, and rerun the verification script.

## Pull requests and reviews

- Keep one task per PR and link its issue with `Closes #NUMBER`.
- Include manual test steps and a screenshot/GIF for visible behavior.
- Ask the reviewer listed in the task plan.
- Review behavior, readability, and maintainability; use `docs/code-reviews/TEMPLATE.md` if the PR itself does not preserve enough detail.
- Resolve feedback, wait for CI, then squash-merge.

Never commit generated `bin/`, `obj/`, IDE settings, secrets, or copyrighted assets without team approval.
