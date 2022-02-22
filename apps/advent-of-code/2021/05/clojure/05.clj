(ns advent-of-code.2021.05.clojure.05
  (:require [clojure.string :as string]
    [punkislamist.core :as pi]))


(defn constrain [n]
  (cond
    (< n 0) -1
    (= n 0) 0
    :else 1))


(defn gen-points [p1 p2]
  (let [[delta-x delta-y] (map constrain (map - p2 p1))]
    (fn [p] (map + p [ delta-x delta-y]))))


(defn points-between [p1 p2]
  (let [next-p (gen-points p1 p2)]
    (pi/take-upto #(= p2 %) (iterate next-p p1))))


(defn mark [board lines]
  (reduce (fn [board [start end]]
          (reduce (fn [board point]
                    (update-in board point inc))
                  board
                  (points-between start end)))
          board
          lines))


(defn is-diagonal [[p1 p2]]
  (every? true? (map not= p1 p2)))
  
  
(defn string->points [s]
  (let [nums (string/split s #"(,| -> )")
      [x1 y1 x2 y2] (map pi/to-number nums)] 
     [[x1 y1] [x2 y2]]))


(def board (let [dimension 1000]
   (vec (repeat dimension (vec (repeat dimension 0))))))


(def points
  (->> "./apps/advent-of-code/2021/05/input.txt"
       (slurp)
       (string/split-lines)
       (map string/trim)
       (map string->points)))


(defn solve [points]
  (->> points
   (mark board)
   (flatten)
   (filter #(> % 1))
   count))


(def part-1
  (solve (filter (complement is-diagonal) points))) ; => 6189
(def part-2
  (solve points)); => 19164
