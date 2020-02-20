#!/bin/bash
set -euo pipefail

apt update
apt upgrade -y
apt install -y git ruby curl libunwind8 gettext apt-transport-https vim-nox mono-complete

git clone https://github.com/cgoehl/deluge-midi.git

cd deluge-midi/_delugeTools/raspberry

chmod +x bin/*
mkdir -p /usr/local/bin
cp -v bin/* /usr/local/bin/
cp -v systemd/* /lib/systemd/system/
cp -v udev/* /etc/udev/rules.d/

chown -R pi /usr/local

systemctl enable midi.service
systemctl enable midi-deluge.service

echo
echo "Success, rebooting 10s"
sleep 10s
reboot