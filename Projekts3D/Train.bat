@echo on
call conda activate ml-agents
mlagents-learn Config/config.yaml --force --torch-device cuda
call conda deactivate
:: @pause
cmd /k