@echo on
call conda activate ml-agents
mlagents-learn --force
call conda deactivate
:: @pause
cmd /k