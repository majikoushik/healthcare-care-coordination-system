# Testing Strategy

Epic 0 adds placeholder test projects for each backend boundary and a frontend shell smoke test.

Future epics must add tests for validation, domain rules, repository behavior, API contracts, correlation ID behavior, safe logging expectations, and clinical AI provider behavior.

CI must default to `AI_PROVIDER=Mock` and must not require Azure credentials.
