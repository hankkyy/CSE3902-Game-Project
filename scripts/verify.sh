#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

if git ls-files | grep -E '(^|/)(bin|obj)/|\.user$|\.suo$|^\.vs/' >/dev/null; then
  echo "Generated build or IDE files are tracked. Remove them before merging." >&2
  exit 1
fi

dotnet restore GameProject/GameProject.csproj
dotnet format GameProject/GameProject.csproj --no-restore --verify-no-changes
dotnet build GameProject/GameProject.csproj --no-restore --configuration Release
git diff --check

echo "Verification passed: hygiene, format, analyzers, and Release build."
