(ns advent-of-code.2021.05.clojure.05
  (:require [clojure.math.numeric-tower :as math]
            [clojure.string :as string]
            [punkislamist.core :as pi]))


(defn candidates-pt-1 [coll]
  (let [nums (sort coll)
        length (count nums)
        mid (quot length 2)]
    (if (odd? length)
      [(nth nums mid)]
      [(nth nums mid) (nth nums (+ mid 1))])))


(defn cost-pt-1 [candidates position]
  (->> candidates
       (map (partial - position))
       (map math/abs)))


(defn candidates-pt-2 [coll]
  (let [nums (sort coll)
        length (count nums)
        avg (/ (reduce + nums) length)]
    [(math/floor avg) (math/ceil avg)]))



(defn cost-pt-2 [candidates position]
  (let [cost (fn [candidate]
               (->> [(max candidate position) (min candidate position)]
                    (apply -)
                    (pi/sum-range 1)))]
    (map cost candidates)))


(defn solve [candidate-f cost-f coll]
  (let [candidates (candidate-f coll)]
    (->> coll
         (map (partial cost-f candidates))
         (apply map vector)
         (map (partial apply +))
         (apply min))))


(def start-positions
  (->> (string/split (slurp "./apps/advent-of-code/2021/07/input.txt") #",")
       (map pi/to-number)))


(solve candidates-pt-1 cost-pt-1 start-positions) ; => 336120
(solve candidates-pt-2 cost-pt-2 start-positions) ; => 96864235
