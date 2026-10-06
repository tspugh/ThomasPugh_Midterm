# Kleinhalija

Kleinhalija is a 2D bullet-hell survival game built as a 2022 Unity coursework
project. The player moves through enemy waves and boss encounters while an
automatic radial weapon fires continuously. Between wave groups, the player
chooses temporary upgrades for health, projectile speed, projectile count,
fire interval, or orbiting turrets.

The project includes two zones, multiple enemy movement and projectile
patterns, difficulty variants, score and high-score tracking, unlockable goals,
and save/load support.

## Controls

- Move the mouse to move and aim the player within the play area.
- Firing is automatic.
- Click a pickup when upgrades appear between waves to choose it.
- Use the on-screen buttons to begin a run, select the player and difficulty,
  save or load progress, return to the menu, or quit.

## Open and run the project

1. Install Unity Editor **2021.3.6f1**. The project records this exact editor
   version in `ProjectSettings/ProjectVersion.txt`.
2. In Unity Hub, choose **Add project from disk** and select the nested
   `ThomasPugh_Midterm/` directory (the directory containing `Assets/`,
   `Packages/`, and `ProjectSettings/`).
3. Open `Assets/Scenes/SampleScene.unity` if Unity does not open it
   automatically. It is the only enabled scene in the build settings.
4. Press **Play** in the editor.

Unity will restore the packages declared in `Packages/manifest.json`, including
Universal Render Pipeline 12.1.7 and TextMesh Pro 3.0.6. No prebuilt standalone
game is committed.

## Implementation highlights

- ScriptableObject-based zones, waves, goals, and projectile patterns make the
  encounter data editable in Unity.
- Projectile patterns support spread, speed, acceleration, jerk, rotation, and
  player-targeted fire.
- Enemy behaviours include basic, sine-wave, gravitational, turret, and
  unpredictable movement or attack logic.
- Pickups update the active player's health, firing pattern, or turret set.
- The scoring system persists high score, currency, unlocked zones, and goal
  state through the game's save/load menu.

## Assets and attribution

The repository includes Unity's Universal Render Pipeline samples and TextMesh
Pro resources. The bundled Liberation Sans license is at
`Assets/TextMesh Pro/Fonts/LiberationSans - OFL.txt`, and the bundled EmojiOne
attribution is at `Assets/TextMesh Pro/Sprites/EmojiOne Attribution.txt`.
The UI art directory retains the source name `1. Free Hologram Interface
Wenrexa`.

This historical coursework repository also contains image and audio assets
without a single project-wide license file. Review the attribution files and
the provenance and terms of individual assets before redistributing them or a
built game.
