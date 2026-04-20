# Voice Command Game

> *A 2D platformer where your voice is the controller.*

---

## Project Thumbnail

![Voice Command Game](https://placehold.co/1280x640/1a1a1a/9b30d9?text=Voice+Command+Game&font=monospace)

> *(Replace this with a screenshot or GIF of your game!)*

---

## Demo Video

[![Watch the Demo](https://placehold.co/640x360/1a1a1a/9b30d9?text=▶+Watch+Demo&font=monospace)](https://your-demo-link-here.com)

> *(Replace this link with your demo video URL)*

---

## Team

| Name | Role |
|---|---|
| John Philip Baldonado | Solo Developer |

---

## Project Description

A 2D platformer where you must navigate through obstacles with the help of a moving platform — controlled entirely by your voice. Direct the platform **left**, **right**, **up**, and **down** using simple voice commands to guide your character safely through each level.

This project was inspired by games like **Mage Arena** and **REPO**, which use voice as a core mechanic. I wanted to explore whether voice recognition could feel natural and fun in a platformer setting — and challenge myself to integrate browser-based speech recognition directly into a Unity WebGL game.

> *Requires Chrome or Edge. Allow microphone access when prompted.*

---

## How Does It Work?

```
🎙️  You speak a command
        ↓
🌐  Chrome's Web Speech API converts your voice to text
        ↓
🔗  A JavaScript .jslib plugin bridges the browser and Unity
        ↓
🤖  The robot platform receives the command and moves
```

| Command | Action |
|---|---|
| `"right"` | Move platform right |
| `"left"` | Move platform left |
| `"up"` | Move platform up |
| `"down"` | Move platform down |
| `"stop"` | Stop the platform |

---

## How Did the Idea Come About?

I've always been fascinated by games that use unconventional input methods. After playing **Mage Arena** and **REPO** — both of which use voice as a core gameplay mechanic — I started wondering: *could I build something like that myself?*

I wanted to see if voice recognition could feel responsive and fun in a platformer context, and whether I could pull it off entirely in the browser without the player needing to download anything. The result is this game — a cooperative puzzle platformer where one mechanic (your voice) controls everything.

---

## Art & Characters

The game features original pixel art characters hand-crafted for the project:

| Idle | Walk | Run |
|---|---|---|
| ![Idle](https://placehold.co/200x200/2a2a2a/ff9a3c?text=Idle&font=monospace) | ![Walk](https://placehold.co/200x200/2a2a2a/ff9a3c?text=Walk&font=monospace) | ![Run](https://placehold.co/200x200/2a2a2a/ff9a3c?text=Run&font=monospace) |

> *(Replace placeholders with your actual sprite screenshots from the presentation)*

---

## Challenges

### Voice Recognition Integration
The hardest part of this project was getting voice recognition to work inside a browser game. Unity has no built-in speech recognition, so I had to write a custom **JavaScript `.jslib` plugin** to bridge the browser's **Web Speech API** with Unity's game logic. Getting commands to fire reliably and in real time required a lot of testing and debugging — especially issues that only appeared in WebGL builds and not in the Unity Editor.

### Animation Synchronization
Animations proved surprisingly tricky. The robot platform needed smooth transitions between idle, moving, and stopping states. Syncing animations correctly with the state machine — so every movement felt responsive and polished — took significant tweaking.

### WebGL-Specific Debugging
Many bugs only surfaced in the final WebGL build and not during Editor play mode, which made debugging slower and more complex than usual.

---

## What I Learned

- How to connect the browser's Web Speech API to Unity using a custom JavaScript `.jslib` plugin
- Setting up a clean level system with smooth transitions between scenes using `DontDestroyOnLoad`
- How to diagnose and fix WebGL-specific issues that don't appear in the Unity Editor, including using `#if UNITY_WEBGL && !UNITY_EDITOR` guards

---

## Final Presentation

📊 [View the Final Presentation Slides](https://canva.link/mnfl6258kcsbk9f)

---

## Built With

![Unity](https://img.shields.io/badge/Unity-000000?style=for-the-badge&logo=unity&logoColor=white)
![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black)
![WebGL](https://img.shields.io/badge/WebGL-990000?style=for-the-badge&logo=webgl&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

- **Engine:** Unity 6 (WebGL / Web build)
- **Voice Recognition:** Web Speech API (Chrome/Edge)
- **JS Bridge:** Custom `.jslib` plugin
- **Platform:** Browser (itch.io)

---

## How to Play

1. Open the game in **Chrome** or **Edge** *(Firefox not supported)*
2. Click **Start** and allow microphone access when prompted
3. Say **"right"**, **"left"**, **"up"**, or **"down"** to move the robot platform
4. Guide your character through the obstacles to reach the goal
