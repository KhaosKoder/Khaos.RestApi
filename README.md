# Khaos.RestApi

## Aim

Provide a **standard, repeatable framework** for integrating 20–50 external REST APIs with a consistent developer experience, strong observability, and predictable governance.

## What it does

- Separates integration concerns into four layers: **External**, **Domain**, **Internal**, and **Client**.
- Enforces consistency via **Roslyn analyzers**, **architecture tests**, and **MSBuild** governance.
- Provides shared infrastructure for **audit persistence** and **observability**.
- Supplies **dotnet new** templates and scripts to scaffold new API modules.
- Generates **nginx.conf** from configuration via a build-time tool.

## Approach

1. **External**: transport-only Refit clients + handlers (internal visibility, no business logic).
2. **Domain**: orchestration, mapping, typed exceptions, and audit persistence.
3. **Internal**: minimal HTTP endpoints hosted by `Khaos.RestApi.Host.Internal`.
4. **Client**: SDK for downstream callers that talks only to Internal.

Governance is enforced through analyzers and architecture tests to keep boundaries intact and integrations deterministic.

## Solution layout

- src/Khaos.RestApi.External.Sample – Refit contract + handlers (internal)
- src/Khaos.RestApi.Domain.Sample – orchestration + audit + exceptions
- src/Khaos.RestApi.Internal.Sample – minimal API endpoints
- src/Khaos.RestApi.Client.Sample – SDK that targets Khaos.RestApi.Host.Internal
- src/Khaos.RestApi.Host.Internal – app host that wires modules
- src/Khaos.RestApi.Common.* – shared abstractions, observability, and persistence
- templates/* – dotnet new templates (client/domain/external/internal)
- tools/Khaos.RestApi.NginxConfigGen – nginx.conf generator
- tests/* – analyzers, architecture, persistence, and observability tests

## What is implemented

- Sample API modules across all four layers.
- Shared common packages (abstractions, persistence, observability).
- Roslyn analyzers enforcing External-layer rules.
- Architecture tests for assembly boundaries.
- NGINX config generator tool.
- CI workflow scaffolding and build/test scripts.
- dotnet new templates and scaffold scripts.

## Not implemented yet

- Real provider integrations (the solution currently contains **Sample** modules).
- OpenAPI-driven DTO generation for actual providers.
- Production host wiring beyond the sample API.
- Environment-specific deployment pipelines and publish automation.

## Configuration

Host settings are read from appsettings.json and include:

- Apis:Sample (external base URL, timeout, user agent)
- Domains:Sample (auditing behavior)
- Persistence:Audit (schema/table configuration)
- Persistence:Audit:Redaction (sensitive field redaction)

## How to use

1. Build the solution:
	- scripts/Build.ps1
2. Run tests:
	- scripts/Test.ps1
3. Run the internal host:
	- dotnet run --project src/Khaos.RestApi.Host.Internal

## How to extend

1. Scaffold a new API module:
	- scripts/New-Api.ps1 (or scripts/new-api.sh)
2. Generate Refit DTOs/methods from OpenAPI **inside** `Khaos.RestApi.External.<ApiName>`.
3. Implement Domain orchestration and audit persistence.
4. Expose Internal endpoints in `Khaos.RestApi.Internal.<ApiName>`.
5. Implement the Client SDK that calls the Internal endpoints.
6. Wire new endpoints in `Khaos.RestApi.Host.Internal`.
7. Add architecture tests for the new module.

## How to test

- scripts/Test.ps1
- scripts/Test-Coverage.ps1 (coverage + report)

Tests include analyzers, architecture rules, and infrastructure-level unit tests.

## Clone scripts

- scripts/Clone-DocsOnly.ps1 – creates a docs-only zip (Markdown files only)

## Docs

- Architecture overview: docs/1. Rough idea and specifications.md
- Detailed spec: docs/3. Specification.md
