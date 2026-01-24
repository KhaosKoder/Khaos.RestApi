# Khaos.RestApi

Standardized multi‑API integration framework using four layers:

- External: transport-only Refit clients + handlers
- Domain: orchestration, typed exceptions, and audit persistence
- Internal: minimal HTTP endpoints hosted in Khaos.RestApi.Host.Internal
- Client: SDK for downstream callers

## Solution layout

- src/Khaos.RestApi.External.Sample – Refit contract + handlers (internal)
- src/Khaos.RestApi.Domain.Sample – orchestration + audit + exceptions
- src/Khaos.RestApi.Internal.Sample – minimal API endpoints
- src/Khaos.RestApi.Client.Sample – SDK that targets Khaos.RestApi.Host.Internal
- src/Khaos.RestApi.Host.Internal – app host that wires modules
- src/Khaos.RestApi.Common.* – shared abstractions, observability, and persistence
- tests/* – analyzers and persistence/observability tests
- tools/Khaos.RestApi.NginxConfigGen – nginx.conf generator

## Configuration

Host settings are read from appsettings.json and include:

- Apis:Sample (external base URL, timeout, user agent)
- Domains:Sample (auditing behavior)
- Persistence:Audit (schema/table configuration)
- Persistence:Audit:Redaction (sensitive field redaction)

## Build & test

- Build: scripts/Build.ps1
- Test: scripts/Test.ps1

## Docs

- Architecture overview: docs/1. Rough idea and specifications.md
- Detailed spec: docs/3. Specification.md
