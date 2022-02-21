(ns advent-of-code.2021.02.clojure.02
  (:require [clojure.string :as string]
            [punkislamist.core :as pi]))


(def direction-map {"forward" :forward
                    "up"      :up
                    "down"    :down})


(def start-position {:horizontal 0
                     :vertical   0
                     :aim        0})


(defn direction+amount [line]
  (let [[direction amount] (string/split line #" ")]
    [(direction-map direction) (pi/to-number amount)]))


(defn move-part-1 [current-position [direction amount]]
  (let [move (fn [axis op] (update current-position axis #(op % amount)))]
    (condp = direction
      :forward (move :horizontal +)
      :down (move :vertical +)
      :up (move :vertical -))))

(defn move-part-2 [pos [direction amount]]
  (let [move (fn [pos axis op] (update pos axis #(op % amount)))]
    (condp = direction
      :down (move pos :aim +)
      :up (move pos :aim -)
      :forward (-> pos
                   (move :horizontal +)
                   (update :vertical #(+ % (* (:aim pos) amount)))))))

(def movements
  (let [lines (string/split-lines (slurp "./apps/advent-of-code/2021/02/input.txt"))]
    (map direction+amount lines)))


(def solve-part-1
  (let [final-position (reduce move-part-1 start-position movements)]
    (* (:horizontal final-position) (:vertical final-position))))

(def solve-part-2
  (let [final-position (reduce move-part-2 start-position movements)]
    (* (:horizontal final-position) (:vertical final-position))))
