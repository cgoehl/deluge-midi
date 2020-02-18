#!/bin/bash
set -euo pipefail

USER=pi
MOUNT_DIR=/tmp/DelugeMidiMount

echo "Creating mount point: $MOUNT_DIR..."
mkdir -p "$MOUNT_DIR"

BINARY="$MOUNT_DIR/_delugeTools/publish/raspberrypi/DelugeMidi"
echo "Mounting $DEVNAME..."
echo mount -o uid=$(id -u $USER) "$DEVNAME" "$MOUNT_DIR"
mount -o uid=$(id -u $USER) "$DEVNAME" "$MOUNT_DIR"

echo "Executing $BINARY as $USER"
sudo -u $USER "$BINARY" 2>&1 >> /tmp/DelugeMidiExec.log || true
echo "Unmounting $DEVNAME"
umount "$DEVNAME"

