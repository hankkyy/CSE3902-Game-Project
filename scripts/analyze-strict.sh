#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

analysis_output="$(mktemp)"
trap 'rm -f "$analysis_output"' EXIT

dotnet build GameProject/GameProject.csproj \
  --configuration Release \
  --no-incremental \
  -p:RunAnalyzersDuringBuild=true \
  -p:AnalysisMode=AllEnabledByDefault \
  -p:TreatWarningsAsErrors=false | tee "$analysis_output"

echo
echo "Strict analyzer rule counts (the MSBuild summary repeats diagnostics, so counts are divided by two):"
grep 'warning CA[0-9]' "$analysis_output" |
  sed -E 's/.*warning (CA[0-9]+):.*/\1/' |
  sort |
  uniq -c |
  awk '{ printf "%4d %s\n", int($1 / 2), $2 }' |
  sort -nr

echo
echo "Compare the result with docs/STRICT_ANALYSIS.md and document every fix or suppression."
