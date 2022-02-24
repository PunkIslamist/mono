(ns advent-of-code.2021.05.clojure.05
  (:require [clojure.math.numeric-tower :as math]
            [clojure.string :as string]
            [punkislamist.core :as pi]))


;; Pretty much taken from Slack
;; Just using an array instead of a map
(defn step [[a b c d e f g h i]]
  [b c d e f g (+ a h) i a])


(defn init-state [fishies]
  (reduce-kv (fn [vec k v] (assoc vec k v))
             [0 0 0 0 0 0 0 0 0]
             (frequencies fishies)))


(defn population [days]
  (->> (string/split (slurp "./apps/advent-of-code/2021/06/input.txt") #"\,")
       (map pi/to-number)
       (init-state)
       (iterate step)
       (take (inc days))
       (last)
       (apply +)))


(population 80) ; => 359999
(population 256) ; => 1631647919273
