#!/bin/bash

if [ $# -ne 1 ]
then
	echo "Usage: $0 {udp|mmap}"
	exit 1
fi

source config/config.bash

bash ${INS_DIR}/utils/create_empty.bash ${1} 

echo "### create config.json for unity"
bash config/scripts/create_config.bash ${1} unity

