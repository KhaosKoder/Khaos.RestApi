#!/usr/bin/env bash
set -euo pipefail

if [ $# -lt 1 ]; then
  echo "Usage: $0 <ApiName> [DbSchema] [TableMode] [EnableAuditing] [SolutionFolder]" >&2
  exit 1
fi

API_NAME="$1"
DB_SCHEMA="${2:-Audit}"
TABLE_MODE="${3:-Single}"
ENABLE_AUDITING="${4:-true}"
SOLUTION_FOLDER="${5:-src}"

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
TEMPLATE_PATH="$REPO_ROOT/templates/dotnet-new-external"

echo "Installing local template pack from $TEMPLATE_PATH"
dotnet new install "$TEMPLATE_PATH" >/dev/null

echo "Scaffolding API module '$API_NAME'..."
dotnet new khaos-apimodule \
  --ApiName "$API_NAME" \
  --DbSchema "$DB_SCHEMA" \
  --TableMode "$TABLE_MODE" \
  --EnableAuditing "$ENABLE_AUDITING" \
  --SolutionFolder "$SOLUTION_FOLDER" \
  --output "$REPO_ROOT"
