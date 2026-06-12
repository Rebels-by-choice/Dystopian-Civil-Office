#!/usr/bin/env bash
set -euo pipefail

echo "=== Dystopian Civil Office: starting development environment ==="

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" >/dev/null 2>&1 && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../../.." >/dev/null 2>&1 && pwd)"

SERVER_PATH="${REPO_ROOT}/apps/server/src/Dystopian-Civil-Office/Dystopian-Civil-Office"
CLIENT_PATH="${REPO_ROOT}/apps/client/Dystopian-Civil-Office"
LOGS_DIR="${REPO_ROOT}/logs"

BACKEND_PID=""
FRONTEND_PID=""
CONTAINER_LOGS_PID=""

TIMESTAMP="$(date +%Y-%m-%d_%H-%M-%S)"
CONTAINERS_LOG_FILE="${LOGS_DIR}/containers-${TIMESTAMP}.log"
BACKEND_LOG_FILE="${LOGS_DIR}/backend-${TIMESTAMP}.log"
FRONTEND_LOG_FILE="${LOGS_DIR}/frontend-${TIMESTAMP}.log"

start_step() {
  echo
  echo "==> $1"
}

assert_command_exists() {
  if ! command -v "$1" >/dev/null 2>&1; then
    echo "Required command '$1' was not found in PATH." >&2
    exit 1
  fi
}

cleanup() {
  echo
  echo "==> Shutting down background processes..."

  if [[ -n "${BACKEND_PID}" ]] && kill -0 "${BACKEND_PID}" 2>/dev/null; then
    kill "${BACKEND_PID}" 2>/dev/null || true
  fi

  if [[ -n "${FRONTEND_PID}" ]] && kill -0 "${FRONTEND_PID}" 2>/dev/null; then
    kill "${FRONTEND_PID}" 2>/dev/null || true
  fi

  if [[ -n "${CONTAINER_LOGS_PID}" ]] && kill -0 "${CONTAINER_LOGS_PID}" 2>/dev/null; then
    kill "${CONTAINER_LOGS_PID}" 2>/dev/null || true
  fi

  echo "Logs saved in: ${LOGS_DIR}"
}

trap cleanup EXIT INT TERM

assert_command_exists docker
assert_command_exists dotnet
assert_command_exists npm
assert_command_exists npx
assert_command_exists tee
assert_command_exists mkdir
assert_command_exists find

mkdir -p "${LOGS_DIR}"
find "${LOGS_DIR}" -type f -mtime +7 -delete

start_step "Starting Docker containers"
cd "$REPO_ROOT"
docker compose up -d --build

sleep 3
echo "Docker containers started successfully."

start_step "Restoring backend packages"
cd "$SERVER_PATH"
dotnet restore
echo "Backend packages restored."

start_step "Running EF Core migrations"
dotnet ef database update
echo "Database migration completed."

start_step "Starting containers logs in background"
cd "$REPO_ROOT"
(
  docker compose logs -f 2>&1 | tee -a "${CONTAINERS_LOG_FILE}"
) &
CONTAINER_LOGS_PID=$!
echo "Containers logs started. PID: ${CONTAINER_LOGS_PID}"
echo "Containers log file: ${CONTAINERS_LOG_FILE}"

start_step "Starting backend in background"
cd "$SERVER_PATH"
(
  dotnet watch run 2>&1 | tee -a "${BACKEND_LOG_FILE}"
) &
BACKEND_PID=$!
echo "Backend started. PID: ${BACKEND_PID}"
echo "Backend log file: ${BACKEND_LOG_FILE}"

start_step "Installing frontend dependencies"
cd "$CLIENT_PATH"
npm ci
echo "Frontend dependencies installed."

start_step "Starting frontend in background"
export NG_CLI_ANALYTICS="false"
(
  npx ng serve 2>&1 | tee -a "${FRONTEND_LOG_FILE}"
) &
FRONTEND_PID=$!
echo "Frontend started. PID: ${FRONTEND_PID}"
echo "Frontend log file: ${FRONTEND_LOG_FILE}"

echo
echo "Development environment is running."
echo "Containers logs PID : ${CONTAINER_LOGS_PID}"
echo "Backend PID         : ${BACKEND_PID}"
echo "Frontend PID        : ${FRONTEND_PID}"
echo
echo "Logs directory      : ${LOGS_DIR}"
echo "Press Ctrl + C to stop all processes."
echo

wait