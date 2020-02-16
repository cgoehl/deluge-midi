#!/bin/bash
set -euo pipefail
LOGFILE="/tmp/DelugeMidiUdev.log"
SCRIPT_DIR="$( dirname "${BASH_SOURCE[0]}" )"

echo "Entry was started" >> "$LOGFILE"
env > "$LOGFILE"
echo "Forking off job" >> "$LOGFILE"
/usr/local/bin/DelugeMidiJob.sh 2>&1 >> "/tmp/DelugeMidiJob.sh" & disown
echo "Job forked!" >> "$LOGFILE"
