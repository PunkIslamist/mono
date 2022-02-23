# Sytem update - SUP

Main goal: have one command that updates the system and keeps it in sync. Aka run `scoop up` and sync settings.

## Plan

[x] Create a runnable script
[x] Debug the script in VS Code
[x] Write output of scoop to a file
[x] - Suitable formatting?
[x] Read output of scoop from a file
[x] Create diff of two sets of scoop info
[x] - Info can come from file
[x] - Info can come from calling scoop
[x] Get user decisions per diff
[x] Do code review
[x] Install scoop apps based on user decision
[x] Uninstall scoop apps based on user decision
[ ] Persist new state

### Next up

Call as shell cmd
- Requires build to work
- Figure out how to make it available via shell
- Figure out how to best run something via shell
  - Look at how scoop does it
- Try to have output as small as possible in terms of file count
- Have release publish it to correct folder automatically

Sync scoop state
- First step: just copy the export to current settings folder?

### Backlog

Unit tests
- Research available libraries and runners
  - https://github.com/haf/expecto/
- Write them
  - Refactor into cleany separated steps of pure - impure
  - Allows testability of pure parts
- Have them run during build
- Have them run in background?

Refactor to introduce types as early as possible
- Reading from file should return ServerApps (or something)
- Reading from `scoop export` should return InstalledApps (or something)

Rename to sup

Sync scoop state via git
- Do not rely on current settings folder anymore
- Create a "storage" local to the app
- Create a new git repo probably? Or use the current one?
- Get synced from repo
- After sync work, push new state to repo

VS Code app list sync
- Get list of installed
- Compare against synced
- Install missing
- Update synced

VS Code settings sync
All settings sync
Replace existing Nu scripts with PS scripts
Make it available globally
Improve functionality
- Have one command that runs everything
- Exclude apps from syncing (e.g. Krita)
  - When new app detected, ask if it should be added
  - Else add to some kind of ignore list
- Remove synced apps again
- Replace absolute paths to user dir with token (e.g. in `settings.json`)
- Copy synced files into a .settings folder so I don't need a "global" git repo anymore
- Show in command prompt when there are pending changes without running a command

### Done
