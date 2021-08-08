# ML-Agents
 
## Install

- Install Python
- cd to Unity dir
- make a python venv with 
 ```sh
 python -m venv venv`
 ```
- aktivate venv with
```sh
venv\Scripts\activate
```
### Install stuff

```sh
python -m pip install --upgrade pip
```
```sh
pip3 install torch~=1.7.1 -f https://download.pytorch.org/whl/torch_stable.html`
```
```sh
python -m pip install mlagents==0.27.0
```


### Check

```sh
mlagents-learn --help
```
### Start learning
```sh
mlagents-learn --force 
```
- `mlagents-learn --run-id=Test2`
- `mlagents-learn config/config.yml --force --env=Build/Snake.exe --time-scale=1 --quality-level=0 --width=512 --height=512 --num-envs=10`

### Tensorboard
```sh
tensorboard --logdir results
```
- View Tensorboard on `localhost:6006`

