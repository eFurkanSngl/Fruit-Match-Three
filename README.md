🍬 Match-3 Mobile Game
A Unity-powered Match-3 mobile game project optimized for a wide range of mobile devices.


🔧 Technologies Used
Unity 2022.3.x (LTS)

C#

DOTween (Demigiant): Tween animations

UnityEvents & UnityAction: Custom event handling

Custom Event System: For inter-system communication (Grid, UI, Timer, etc.)

Object Pooling: For particle effect optimization

Audio Mixer: Separate channels for SFX and Music

Safe Area + Aspect Ratio Scaling: UI adaptation for different devices

PlayerPrefs: For saving High Score, sound settings, level unlock status, etc.


🎮 Gameplay Features

5x5 tile-based match system

Match 3 or more tiles to destroy them

Power-Ups:

Match 4: Vertical bomb

Match 5: Horizontal bomb

Match 6: Bomb that destroys adjacent tiles

Timed gameplay (30s initial time, extended with matches)

Combo system with visual feedback (1x, 2x, 3x, ...)

Level progression based on reaching target score

Game Over if target score is not reached in time

Includes How to Play screen, audio settings, level selection UI


🧠 Code Structure & Architecture

Written with OOP (Object-Oriented Programming) principles

Each system has a single responsibility

Decoupled design using events

Code is modular and reusable

Singleton pattern applied where appropriate (e.g., GameManager)


🚀 Performance Optimization

Object Pooling used for destroy particle effects

DOTween capacity optimized to avoid allocation spikes

Shared AudioSource used for sound playback (avoiding per-tile overhead)


📱 Mobile Adaptation

Supports Safe Area for devices with notches

Dynamic camera scaling based on screen aspect ratio

Responsive UI layout and properly configured Canvas settings



Game Referance:

[Movie_006.webm](https://github.com/user-attachments/assets/22b4acc3-563d-4240-8eb7-cc726b3777d6)


[Movie_0010000Movie.webm](https://github.com/user-attachments/assets/26fa5572-f059-4b01-a59f-fff56fc20960)


![Screenshot 2025-05-01 181934](https://github.com/user-attachments/assets/a6d8bd27-11fa-4304-9eb5-1746e6a6a903)

![Screenshot 2025-05-01 183500](https://github.com/user-attachments/assets/44208b66-b32c-4088-b381-b86560d1ac7d)
