#!/usr/bin/env bash 
set -euo pipefail

pathtoname() {
	udevadm info -p /sys/"$1" | awk -v FS== '/DEVNAME/ {print $2}'
}

onDevPlugged() {
	case "$1" in
	(/dev/sd?1)
		echo "launching script"
		DEVNAME=$1 /usr/local/bin/DelugeMidiJob.sh
		;;
	(*) echo "Ignoring $1";;
	esac
}

stdbuf -oL -- udevadm monitor --udev -s block | while read -r -- _ _ event devpath _; do
	if [ "$event" = add ]; then
		devname=$(pathtoname "$devpath")
		onDevPlugged $devname
	fi
done

