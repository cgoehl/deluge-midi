#!/bin/bash
set -euo pipefail

apt update
apt upgrade
apt install -y git ruby curl libunwind8 gettext apt-transport-https vim-nox