# Architecture contract

The current contract is defined in [Sprint 3 architecture](SPRINT3_ARCHITECTURE.md). Sprint 2's public constructors and controls remain backward compatible until the integration branch approves an additive migration.

Do not independently change shared interfaces. Propose the smallest additive contract on `integration/sprint3`, obtain approval from every affected owner, merge that contract first, and only then implement feature behavior.
