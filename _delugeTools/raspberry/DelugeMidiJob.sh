#!/bin/bash
set -euo pipefail

USER=pi
MOUNT_DIR=/tmp/DelugeMidiMount

echo "Creating mount point: $MOUNT_DIR..."
mkdir -p "$MOUNT_DIR"

USER_SCRIPT="/usr/local/bin/DelugeMidiExec.sh"
echo "Mounting $DEVNAME..."
echo mount -o uid=$(id -u $USER) "$DEVNAME" "$MOUNT_DIR"
mount -o uid=$(id -u $USER) "$DEVNAME" "$MOUNT_DIR"

echo "Executing $USER_SCRIPT as $USER"
CONFIG_SD_PATH="$MOUNT_DIR" sudo -E -u $USER "$USER_SCRIPT" 2>&1 >> /tmp/DelugeMidiExec.log || true
echo "Unmounting $DEVNAME"
umount "$DEVNAME"

