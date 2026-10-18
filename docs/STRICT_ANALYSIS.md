# Strict analyzer record

## Baseline — Sprint 3 planning

- Date: 2026-10-18
- Command: `./scripts/analyze-strict.sh`
- Commit: Sprint 2 final baseline `6e6dfeb`
- Result: build succeeded with 109 warnings and 0 errors under `AnalysisMode=AllEnabledByDefault`.

| Rule | Count | Planned action |
|---|---:|---|
| CA1515 | 78 | Review API visibility. Keep public only where cross-project tests or contracts require it; document intentional exceptions. |
| CA1062 | 20 | Add appropriate public-boundary null validation. |
| CA1725 | 4 | Align implementation parameter names with interface declarations. |
| CA2213 | 2 | Review ownership/disposal of MonoGame resources. |
| CA1051 | 2 | Encapsulate visible fields where compatible. |
| CA2208 | 1 | Correct the invalid exception parameter name. |
| CA1859 | 1 | Use the concrete collection type when it does not harm design. |
| CA1001 | 1 | Correct or document disposable ownership in `SpriteFactory`. |

The normal analyzer configuration produced zero warnings, which explains the Sprint 2 report but is not sufficient for Sprint 3. Do not claim that the strict baseline is clean. Run this script at least weekly and append dated results below.

## Weekly runs

### Run 2

- Date:
- Commit:
- Counts:
- Warnings fixed:
- Suppressions and individual reasons:

### Run 3

- Date:
- Commit:
- Counts:
- Warnings fixed:
- Suppressions and individual reasons:
