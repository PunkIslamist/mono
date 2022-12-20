# mono

One repo for all the things

To get all submodules, run `git submodule update --init --recursive`.


## Managing tool versions with asdf

[asdf](https://github.com/asdf-vm/asdf) is a version manager (pretty much) all runimtes. It allows managing runtime versions per folder. The version(s) to use are defined in `.tool-versions` files.

> Important! Always read the docs of a asdf plugin before installing it. They usually have dependencies that must be met first.


## Haskell

To build single file programs, run `runhs compile <file>`. Ensure you have `runhs` installed via `stack install runhs`. Ensure you have stack installed via `scoop install stack`.


## Elixir/Erlang

Install via [asdf](#managing-tool-versions-with-asdf). Make sure to read the docs as Erlang needs to be installed first.


## Clojure

To run single file programs, run `clj <file>`. Ensure you have `clj` installed, see: https://www.clojure.org/guides/getting_started#_clojure_installer_and_cli_tools.

### On folder structure

Resolving (local) dependencies relies on class path logic of Java. How it works in VS Code with Calva:

- Project root is the folder opened in VS Code
- That means a `deps.edn` file should be at this root/project folder
- If `deps.edn` does not define `:paths`, `./src` will be added to the class path, _not_ `.`
    - This means all source files must reside in a folder called `src` at the root level (e.g. `./src/a.clj`)
    - I am not sure who is adding `src` - Calva, Clojure clj, JVM...
- If `deps.edn`, however, does define `:paths`, `/src` will not be appended to these paths
    - E.g. `:paths ["app" "libs"]` adds `app` and `lib` to the class path, and not `app/src` and `lib/src`
- Whatever gets added to the class path, the structure within these folders must follow the class path rules
  - `a.clj` -> `(ns a)`
  - `a/b/c/d.clj` -> `(ns a.b.c.d)`
- If you have multiple projects, each project _must_ have a `deps.edn`
- Example:
  - `mono/`
    - `lib/`
        - `deps.edn` : `{:paths ["src"]}`
        - `src/my_lib.clj` : `(ns my-lib)`
    - `app/`
      - `deps.edn` : `{:deps {lib/lib {:local/root "../lib"}}}`
      - `app.clj` : `(ns app (:require [my-lib]))`
      - And no, I don't know if there are rules for the ID `lib/lib`
