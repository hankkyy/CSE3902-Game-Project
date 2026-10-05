#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

inside_git=false
if git rev-parse --is-inside-work-tree >/dev/null 2>&1; then
  inside_git=true

  if git ls-files | grep -E '(^|/)(bin|obj)/|\.user$|\.suo$|^\.vs/' >/dev/null; then
    echo "Generated build or IDE files are tracked. Remove them before merging." >&2
    exit 1
  fi
fi

dotnet restore GameProject/GameProject.csproj
dotnet format GameProject/GameProject.csproj --no-restore --verify-no-changes
dotnet build GameProject/GameProject.csproj --no-restore --configuration Release

test_projects=(
  "tests/PlayerStates/PlayerStates.Tests.csproj"
  "tests/PlayerItems/PlayerItems.Tests.csproj"
  "tests/PlayerSprites/PlayerSprites.Tests.csproj"
  "tests/Enemies/Enemies.Tests.csproj"
  "tests/InputTests/InputTests.csproj"
  "tests/ItemsBlocks/ItemsBlocks.csproj"
)

for project in "${test_projects[@]}"; do
  dotnet restore "$project"
  dotnet format "$project" --no-restore --verify-no-changes
  dotnet run --project "$project" --configuration Release --no-restore
done

if [[ "$inside_git" == true ]]; then
  git diff --check
fi

echo "Verification passed: hygiene, format, analyzers, Release build, and all headless tests."
