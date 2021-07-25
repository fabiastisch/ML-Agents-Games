# ML-Agents
 
## Install

- Install Python
- cd to Unity dir
- make a python venv with `python -m venv venv`
- aktivate venv with `venv\Scripts\activate` 

### Install stuff

- `python -m pip install --upgrade pip`
- `pip3 install torch~=1.7.1 -f https://download.pytorch.org/whl/torch_stable.html`
- `python -m pip install mlagents==0.27.0`

### Check

- `mlagents-learn --help`

### Start learning
- `mlagents-learn --force`
- `mlagents-learn --run-id=Test2`
- `mlagents-learn config/config.yml --force --env=Build/Snake.exe --time-scale=1 --quality-level=0 --width=512 --height=512 --num-envs=10`

### Tensorboard
- `tensorboard --logdir results`
- View Tensorboard on `localhost:6006`

