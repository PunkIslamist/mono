(ns getting-clojure.core
  (:gen-class))

(defn harro
  "Must actually be defined before being used :O"
  [uwu]
  (println "Harro" uwu))

(defn -main
  "I don't do a whole lot ... yet."
  [& args]
  (harro ":3"))
