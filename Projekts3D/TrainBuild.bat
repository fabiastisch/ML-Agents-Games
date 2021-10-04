@echo on
call conda activate ml-agents
mlagents-learn Config/config.yaml --force --torch-device cuda --env=Build/Projekts3D.exe
call conda deactivate
:: @pause
cmd /k